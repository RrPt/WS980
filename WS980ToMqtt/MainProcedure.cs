using MQTTnet;
using MQTTnet.Client;
using RpToolsNs;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WS980_NS;

namespace WS980ToMqtt
{
    internal class MainProcedure
    {
        public bool stopPending = false;
        //public TimeSpan WaitTime =  new TimeSpan(0,0,15);  // in h,min,s
        static DateTime lastMeasureTime = DateTime.UtcNow;
        static IMqttClient mqttClient;
        static MqttClientOptions mqttClientOptions;
        static readonly SemaphoreSlim _reconnectLock = new SemaphoreSlim(1, 1);
        //String topicBase = "weatherstation/";
        DateTime now;
        DateTime waitUntilTime;
        WS980 ws980 = null;
        //String mqttServer = "192.168.22.101";

        internal void Start()
        {
            DateTime buildDate = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).LastWriteTime;
            String Version = "BuildDate = " + buildDate;

            MyLogger.Info("-------------------------------------------------------------------------------------");
            MyLogger.Info("  ws980ToMqttService wurde gestartet. " + Version);
            MyLogger.Info("-------------------------------------------------------------------------------------");

            GenericConfig<Config>.importConfig();
            //GenericConfig<Config>.GenericInstance.exportConfig();   // nur zur Initialisierung



            var WS980List = WS980.RequestAllStations();

            while (WS980List.Count < 1)
            {
                MyLogger.Err("keine Wetterstation gefunden, nächster Versuch in 60s");
                Thread.Sleep(60000);
                WS980List = WS980.RequestAllStations();
            }
            MyLogger.Info("Folgende Wetterstationen gefunden:");
            int i = 1;
            foreach (var item in WS980List)
            {
                MyLogger.Info("  {0}. {1}", i++, item.ToString());
            }
            ws980 = new WS980(WS980List[0]);
            MyLogger.Info("1. Wetterstation ausgewählt");
            while (!ws980.DataValid)
            {
                ws980.ReadParameter();
                if (!ws980.DataValid)
                {
                    MyLogger.Err("ReadParameter nicht erfolgreich");
                    MyLogger.Notfallmeldung("WS8´989ToMqtt.MainProcedure.Start; ws980.ReadParameter() liefert Fehler");
                    Thread.Sleep(60000); // 60s warten dann wieder versuchen
                }
            }
            var mqttFactory = new MqttFactory();
            mqttClient = mqttFactory.CreateMqttClient();
            mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(Config.Instance.Mqtt_Broker)
                .WithCredentials(Config.Instance.Mqtt_User, Config.Instance.Mqtt_password.Decrypted)
                .WithKeepAlivePeriod(TimeSpan.FromSeconds(30))
                .Build();

            mqttClient.ConnectedAsync += async e =>
            {
                MyLogger.Info("MQTT connected.");
            };

            mqttClient.DisconnectedAsync += async e =>
            {
                if (mqttClient.IsConnected)
                    return;
                if (!await _reconnectLock.WaitAsync(0))
                    return;
                try
                {
                    MyLogger.Err("MQTT disconnected: " + e.ReasonString);
                    int delaySeconds = 5;
                    while (!stopPending)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
                        if (mqttClient.IsConnected)
                            return;
                        try
                        {
                            MyLogger.Info("Trying MQTT reconnect...");
                            await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);
                            MyLogger.Info("MQTT reconnect successful.");
                            return;
                        }
                        catch (Exception ex)
                        {
                            MyLogger.Err("MQTT reconnect failed: " + ex.Message);
                            delaySeconds = Math.Min(delaySeconds * 2, 60);
                        }
                    }
                }
                finally
                {
                    _reconnectLock.Release();
                }
            };

            {
                int delaySeconds = 5;
                bool connected = false;
                while (!connected)
                {
                    try
                    {
                        mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None).GetAwaiter().GetResult();
                        connected = true;
                    }
                    catch (Exception ex)
                    {
                        MyLogger.Err("MQTT Verbindung zu {0} fehlgeschlagen: {1}, nächster Versuch in {2}s", Config.Instance.Mqtt_Broker, ex.Message, delaySeconds);
                        Thread.Sleep(delaySeconds * 1000);
                        delaySeconds = Math.Min(delaySeconds * 2, 60);
                    }
                }
            }
            MyLogger.Info("Sende MQTT an " + Config.Instance.Mqtt_Broker);
            int loopCounter = 0;
            int errorCounter = 0;
            do
            {
                try
                {   // warten auf Intervallgrenze

                    now = DateTime.UtcNow;

                    if (!ws980.getData())
                    {
                        errorCounter++;
                        if ((errorCounter % (4*15)) == 0) // nur wenn mindestens 15 Minuten in Folge, dann per email warnen
                        {
                            MyLogger.Err("WS980ToMqtt.MainProcedure.Start; ws980.getData() liefert Fehler, zum {0} ten mal in Folge M",errorCounter);
                        }
                        else
                        {
                            MyLogger.Err("WS980ToMqtt.MainProcedure.Start; ws980.getData() liefert Fehler, zum {0} ten mal in Folge",errorCounter);
                        }
                    }
                    else
                    {
                        errorCounter = 0;
                        bool erg = PublishMqtt("Time", now.ToString("o"));
                        if (!erg)
                            MyLogger.Err("Exeption in PublishMqtt('Time',{0})", now.ToString("o"));
                        // debugging
                        WS980Sensor sens = ws980.SensorList.Values[1];
                        //MyLogger.Info("{0:0.00}: {1:0.00} <{2:0.00} {3:0.00}>  Daymin:[{4:0.00} {5}] Daymax:[{6:0.00} {7}] JSON:{8}", sens.Name, sens.ActualValue, sens.MinValue, sens.MaxValue, sens.DayMinValue, sens.DayMinTime, sens.DayMaxValue, sens.DayMaxTime,sens.GetJson());


                        MyLogger.Debug("-- {0} --------", now.ToString("o"));
                        foreach (var sensor in ws980.SensorList.Values)
                        {
                            MyLogger.Debug(sensor.ToString());
                            erg = PublishMqtt(sensor.Name, sensor.GetJson());
                            if (!erg)
                                MyLogger.Err("Exeption in PublishMqtt({0},{1})", sensor.Name, sensor.GetJson());
                            else
                                MyLogger.Debug("PublishMQTT({0},{1})", sensor.Name, sensor.GetJson());
                        }

                        // zyklisch Mail senden wenn es einen Fehler gab
                        if (lastMeasureTime.Hour < now.Hour)
                        {
                            MyLogger.Info("Sammelemail senden");
                            MyLogger.SendSammelEmail();
                            //MyLogger.Warn("Sammelemail ausgeblendet, da unnötig");
                            MyLogger.Info("{0} Schleifendurchläufe", loopCounter);
                        }

                        lastMeasureTime = now;
                        loopCounter++;
                    }
                }
                catch (Exception eX)
                {
                    MyLogger.CriticalM("Ws980ToMqttService befindet sich in einem undefinierten Zustand.");
                    MyLogger.LogException("Start() in Do-loop", eX);
                    if (eX.InnerException != null)
                        MyLogger.LogException("inner exception", eX);
                }
                finally
                {
                    TimeSpan WaitTime = new TimeSpan(0, 0, Config.Instance.IntervalInSeconds);
                    waitUntilTime = new DateTime((now.Ticks + WaitTime.Ticks  - 1) / WaitTime.Ticks * WaitTime.Ticks, DateTimeKind.Utc);
                    while (DateTime.UtcNow < waitUntilTime)
                    {
                        Thread.Sleep(1000);
                    }
                }
            } while (!stopPending);
            MyLogger.Info("MainProcedure.Start: Schleife beendet");
        }

        private bool PublishMqtt(string topic, string payload)
        {
            try
            {
                if (!mqttClient.IsConnected)
                {
                    MyLogger.Err("PublishMqtt: MQTT nicht verbunden, Reconnect läuft über DisconnectedAsync.");
                    return false;
                }
                var applicationMessage = new MqttApplicationMessageBuilder().WithTopic(Config.Instance.Mqtt_TopicBase + topic).WithPayload(payload).Build();
                mqttClient.PublishAsync(applicationMessage, CancellationToken.None).GetAwaiter().GetResult();
                return true;
            }
            catch (Exception ex)
            {
                MyLogger.Err("PublishMqtt({0},{1}): {2}", topic, payload, ex.Message);
                return false;
            }
        }
    }
}