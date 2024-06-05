using RpToolsNs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WS980ToMqtt
{
    public partial class Ws980ToMqttService : ServiceBase
    {
        MainProcedure mp = new MainProcedure();
        Thread workThread;
        const int sleepTime = 10000;
        public Ws980ToMqttService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            MyLogger.Info("Ws980ToMqttService.OnStart: Starte Thread für DoJob");
            workThread = new Thread(new ThreadStart(DoJob));
            mp.stopPending = false;
            workThread.Start();
            MyLogger.Info("Ws980ToMqttService.OnStart: Service gestartet");
        }

        protected override void OnStop()
        {
            MyLogger.Info("Ws980ToMqttService.OnStop");
            mp.stopPending = true;
        }

        protected void DoJob()
        {
            MyLogger.Info("Ws980ToMqttService.DoJob Start");
            mp.Start();
            MyLogger.Info("Ws980ToMqttService.DoJob Ende");
        }
    }
}
