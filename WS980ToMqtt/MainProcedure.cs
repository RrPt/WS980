using RpToolsNs;
using System;
using System.Threading;
using MQTTnet.Client;
using MQTTnet;
using System.IO;
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
        String mqttServer = "192.168.22.100";

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
                MyLogger.Info("Folgnde Wetterstationen gefunden:");
                int i = 1;
                foreach (var item in WS980List)
                {
                    MyLogger.Info("  {0}. {1}",i,item.ToString());
                }
            }
            ws980 = new WS980(WS980List[0]);
            MyLogger.Info("1. Wetterstation ausgewählt");
            ws980.ReadParameter();

            var mqttFactory = new MqttFactory();
            mqttClient = mqttFactory.CreateMqttClient();
            mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(mqttServer)
                .WithCredentials("hv", "Mqtt4hv")
                .Build();

            var t = mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None).GetAwaiter().GetResult();
            MyLogger.Info("Sende MQTT an " + mqttServer);
            int loopCounter = 0;
            do
            {
                try
                {   // warten auf Intervallgrenze

                    now = DateTime.UtcNow;

                    ws980.getData();

                    PublishMqtt("Time",now.ToString("o"));
                    // debugging
                    WS980Sensor sens = ws980.SensorList.Values[1];
                    //MyLogger.Info("{0:0.00}: {1:0.00} <{2:0.00} {3:0.00}>  Daymin:[{4:0.00} {5}] Daymax:[{6:0.00} {7}] JSON:{8}", sens.Name, sens.ActualValue, sens.MinValue, sens.MaxValue, sens.DayMinValue, sens.DayMinTime, sens.DayMaxValue, sens.DayMaxTime,sens.GetJson());


                    MyLogger.Debug("-- {0} --------", now.ToString("o"));
                    foreach (var sensor in ws980.SensorList.Values)
                    {
                        MyLogger.Debug(sensor.ToString() );
                        PublishMqtt(sensor.Name, sensor.GetJson());
                        MyLogger.Debug("PublishMQTT({0},{1})", sensor.Name, sensor.GetJson());
                    }

                    // zyklisch Mail senden wenn es einen Fehler gab
                    if (lastMeasureTime.Hour < now.Hour)
                    {
                        MyLogger.SendSammelEmail();
                        MyLogger.Info("{0} Schleifendurchläufe", loopCounter);    
                    }

                    lastMeasureTime = now;
                    loopCounter++;
                }
                catch (Exception eX)
                {
                    MyLogger.CriticalM("s980ToMqttService befindet sich in einem undefinierten Zustand.");
                    MyLogger.CriticalM(eX.Message);
                    if (eX.InnerException != null)
                    {
                        MyLogger.CriticalM(eX.InnerException.Message);
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

        private void PublishMqtt(string topic, string payload)
        {
            var applicationMessage = new MqttApplicationMessageBuilder().WithTopic(topicBase+topic).WithPayload(payload).Build();
            mqttClient.PublishAsync(applicationMessage, CancellationToken.None).GetAwaiter().GetResult();
        }
    }
}