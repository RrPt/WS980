using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using RpToolsNs;

namespace WS980ToMqtt
{
    internal static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        static void Main(string[] args)
        {
            MyLogger.Init("Ws980ToMqttService");
            MyLogger.LogLevel = 5;
            MyLogger.AddEmailToInform("R.Petzoldt@web.de");
            try
            {
                if ((args.GetLength(0) > 0) && (args[0] == @"/debug" || args[0] == "-debug"))
                {
                    MainProcedure mp = new MainProcedure();
                    Console.WriteLine("\t\t+------------------------------+");
                    Console.WriteLine("\t\t|                              |");
                    Console.WriteLine("\t\t|     Ws980ToMqttService       |");
                    Console.WriteLine("\t\t|                              |");
                    Console.WriteLine("\t\t+------------------------------+");
                    Console.WriteLine("\t\t|                     (c) RrPt |");
                    Console.WriteLine("\t\t+------------------------------+");
                    MyLogger.Info("Debugmodus (kein Service)");
                    mp.Start();
                }
                else
                {
                    ServiceBase[] ServicesToRun;
                    ServicesToRun = new ServiceBase[]
                    {
                        new Ws980ToMqttService()
                    };
                    MyLogger.Info("Run Service");
                    ServiceBase.Run(ServicesToRun);
                }
            }
            catch (Exception eX)
            {
                MyLogger.Critical(eX.Message);
                if (eX.InnerException != null)
                {
                    MyLogger.CriticalM(eX.InnerException.Message);
                }
            }
        }
    }
}
