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
        public TimeSpan WaitTime =  new TimeSpan(0,0,15);  // in h,min,s
        static DateTime lastMeasureTime = DateTime.UtcNow;
        static IMqttClient mqttClient;
        static MqttClientOptions mqttClientOptions;
        String topicBase = "weatherstation/";
        DateTime now;
        DateTime waitUntilTime;
        WS980 ws980 = null;
        String mqttServer = "192.168.22.101";

        internal void Start()
        {
            DateTime buildDate = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).LastWriteTime;
            String Version = "BuildDate = " + buildDate;

            MyLogger.Info("-------------------------------------------------------------------------------------");
            MyLogger.Info("  ws980ToMqttService wurde gestartet. " + Version);
            MyLogger.Info("-------------------------------------------------------------------------------------");


            var WS980List = WS980.RequestAllStations();

            if (WS980List.Count < 1)
            {
                MyLogger.Err("keine Wetterstation gefunden");
            }
            else
            {
                MyLogger.Info("Folgende Wetterstationen gefunden:");
                int i = 1;
                foreach (var item in WS980List)
                {
                    MyLogger.Info("  {0}. {1}",i,item.ToString());
                }
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
                .WithTcpServer(mqttServer)
                .WithCredentials("hv", "Mqtt4hv")
                .Build();

            mqttClient.ConnectedAsync += async e =>
            {
                MyLogger.Info("MQTT connected.");
            };

            mqttClient.DisconnectedAsync += async e =>
            {
                MyLogger.Err("MQTT disconnected: " + e.ReasonString);

                // Warte kurz, damit wir nicht in eine Reconnect-Schleife rasen
                await Task.Delay(TimeSpan.FromSeconds(5));

                try
                {
                    MyLogger.Info("Trying MQTT reconnect...");
                    await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);
                    MyLogger.Info("MQTT reconnect successful.");
                }
                catch (Exception ex)
                {
                    MyLogger.Err("MQTT reconnect failed: " + ex.Message);
                }
            };

            MqttClientConnectResult t = mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None).GetAwaiter().GetResult();
            MyLogger.Info("Sende MQTT an " + mqttServer);
            int loopCounter = 0;
            do
            {
                try
                {   // warten auf Intervallgrenze

                    now = DateTime.UtcNow;

                    if (!ws980.getData())
                        MyLogger.ErrM("WS8´989ToMqtt.MainProcedure.Start; ws980.getData() liefert Fehler");
                    else
                    {

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
                            MyLogger.Warn("Sammelemail senden");
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
                    {
                        MyLogger.LogException("inner exception",eX);
                        MqttClientConnectResult erg = mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None).GetAwaiter().GetResult();
                        MyLogger.Info("Versuch MQTT reconnect zu {0} : {1}",  mqttServer,erg?.ReasonString);
                    }
                }
                finally
                {
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
                var applicationMessage = new MqttApplicationMessageBuilder().WithTopic(topicBase + topic).WithPayload(payload).Build();
                mqttClient.PublishAsync(applicationMessage, CancellationToken.None).GetAwaiter().GetResult();
                return true;
            }
            catch (Exception ex)
            {
                MyLogger.LogException("PublishMqtt",ex);
                return false;
            }
        }
    }
}