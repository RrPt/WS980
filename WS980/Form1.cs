using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


// todo:  TimeSync funktioniert noch nicht
// todo:  Befehl 0x=A PARAM Changed: untersuchen
// todo:  Befehl 0x=C READ_PARAM : untersuchen
// todo:  EPROM 0x0 bis 0x260 untersuchen

namespace WS980
{
    public partial class Form1 : Form
    {
        WS980 ws980 = null;
        string CsvFileName = "WS980-data.csv";

        public Form1()
        {
            InitializeComponent();
            //timer1.Start();
        }

        byte size = 0xf5;
        private void button1_Click(object sender, EventArgs e)
        {
            //ws980.CompareEpromStart();
            //return;
            DateTime time = new DateTime(2011, 1, 1, 12, 12, 12);
            var erg = ws980.SetTime(time);

            ws980.getData();
            tBOut.AppendText(ws980.Version + Environment.NewLine);
            foreach (var sensor in ws980.SensorList.Values)
            {
                tBOut.AppendText(sensor.ToString() + Environment.NewLine);
            }
            tBOut.AppendText("---------------------------" + Environment.NewLine);

            ws980.getHistory();
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            var WS980List = WS980.RequestAllStations();
            foreach (var item in WS980List)
            {
                tBOut.AppendText(item.ToString() + Environment.NewLine);
            }
            if (WS980List.Count < 1)
            {
                tBOut.AppendText("keine Wetterstation gefunden" + Environment.NewLine);
                return;
            }
            ws980 = new WS980(WS980List[0]);
            button1_Click(null, null);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            button1_Click(null, null);
            WriteToCsv(ws980.GetAllActualValues());
        }

        private void WriteToCsv(string v)
        {
            File.AppendAllText(CsvFileName,v + Environment.NewLine);
        }

        private void btnClearHistory_Click(object sender, EventArgs e)
        {
            var erg = ws980.ClearAllHistory();
            Tools.WriteLine("Ergebnis von ClearAllHistory: {0}", erg);
        }

        private void btnClearMaxMinDay_Click(object sender, EventArgs e)
        {
            var erg = ws980.ClearMaxMinDay();
            Tools.WriteLine("Ergebnis von ClearMaxMinDay: {0}", erg);
        }
    }
}
