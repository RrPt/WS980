﻿using System;
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
using WS980_NS;


// todo:  TimeSync funktioniert noch nicht
// todo:  Befehl 0x=A PARAM Changed: untersuchen
// todo:  Befehl 0x=C READ_PARAM : untersuchen
// todo:  EPROM 0x0 bis 0x260 untersuchen

namespace WS980Test
{
    public partial class MainForm : Form
    {
        WS980 ws980 = null;
        string CsvFileName = "WS980-data.csv";

        public MainForm()
        {
            InitializeComponent();
            //timer1.Start();
        }

        byte size = 0xf5;
        private void button1_Click(object sender, EventArgs e)
        {
            //ws980.CompareEpromStart();
            //return;
            //DateTime time = new DateTime(2011, 1, 1, 12, 12, 12);
            //var erg = ws980.SetTime(time);

            // Test Zeitsync
            //byte[] bef;
            //byte[] erg;
            //byte[] bef = { 0xff, 0xff, 0x0b, 0x00, 13, 0x01, 19, 2, 16,17,30,00, 1, 0x82, 0x18 };  // 
            //bef[bef.Length - 2] = Tools.calcChecksum(bef.Skip(5).Take(bef.Length - 7));
            //bef[bef.Length - 1] = Tools.calcChecksum(bef.Skip(2).Take(bef.Length - 3));
            //erg = getAnswer(bef);


            // write Eprom Test
            //ushort adr = 0x8123;
            //var vor = ws980.ReadEprom(adr, 10);
            //byte[] dta = new byte[10];
            //bool ok = ws980.WriteEprom(adr, dta);
            //var nach = ws980.ReadEprom(adr, 10);
            //Console.WriteLine("{0} -> {1}", Tools.ToString(vor), Tools.ToString(nach));

            // Read Eprom Test
            //ushort adr = 0x8123;
            //var vor = ws980.ReadEprom(adr, 10);
            //Console.WriteLine("Read: {0} ", Tools.ToString(vor));

            ws980.getData();
            tBOut.AppendText(ws980.Version + Environment.NewLine);
            foreach (var sensor in ws980.SensorList.Values)
            {
                tBOut.AppendText(sensor.ToString() + Environment.NewLine);
            }
            tBOut.AppendText("---------------------------" + Environment.NewLine);

            //ws980.getHistory();
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
            ws980.ReadParameter();
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
            throw new Exception("Funktion gesperrt");
            var erg = ws980.ClearAllHistory();
            Tools.WriteLine("Ergebnis von ClearAllHistory: {0}", erg);
        }

        private void btnClearMaxMinDay_Click(object sender, EventArgs e)
        {
            var erg = ws980.ClearMaxMinDay();
            Tools.WriteLine("Ergebnis von ClearMaxMinDay: {0}", erg);
        }

        private void btnGetHistory_Click(object sender, EventArgs e)
        {
            ws980.getHistory();
        }

        private void btnReadPara_Click(object sender, EventArgs e)
        {
            var para = ws980.ReadParameter();
            tBOut.AppendText(para.GetRawData() + Environment.NewLine);
            tBOut.AppendText(para.ToString() + Environment.NewLine);

        }

        private void btnCompareEprom_Click(object sender, EventArgs e)
        {
            var txt = ws980.CompareEpromStart();
            tBOut.AppendText(txt + Environment.NewLine);
        }

        private void btnClr_Click(object sender, EventArgs e)
        {
            tBOut.Clear();
        }

        private void btnWritePara_Click(object sender, EventArgs e)
        {
            var erg = ws980.WriteParameter();
            tBOut.AppendText("WritePara="+erg+Environment.NewLine);
        }

        private void cBKeyBeep_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            ws980.Para.KeyBeep = cb.Checked;
            if (cb.Checked)
            {
                ws980.Para.TemperatureUnit = TemperatureUnitE.C;
                ws980.Para.KeyBeep = true;
            }
            else
            {
                ws980.Para.TemperatureUnit = TemperatureUnitE.F;
                ws980.Para.KeyBeep = false;
            }
        }

        private void btnRead_Click(object sender, EventArgs e)
        {

            ushort adr = Convert.ToUInt16(tBAdr.Text,16);
            var wertArr = ws980.ReadEprom(adr, 1);
            if (wertArr != null && wertArr.Length > 0)
            {
                byte wert = wertArr[0];
                tBWert.Text = String.Format("0x{0:X2}", wert);
            }
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            ushort adr = Convert.ToUInt16(tBAdr.Text, 16);
            byte wert = Convert.ToByte(tBWert.Text, 16);
            byte[] wertArr = new byte[1];
            wertArr[0] = wert;
            var erg =  ws980.WriteEprom(adr,wertArr);
            tBOut.AppendText("erg = " + erg.ToString() + Environment.NewLine);
        }

        private void btnChangeinfo_Click(object sender, EventArgs e)
        {
            ws980.ChangedParameter((short)0x0004);
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            byte[] part1 = new byte[] { 0x29 };//, 0x01, 0x04, 0x47, 0x88 };
            if (cBKeyBeep.Checked) part1[0] = 0x29;
            else part1[0] = 0x2B;
            ws980.WriteEprom(0x10, part1);

            byte[] part2 = new byte[] { 0x05};
            ws980.WriteEprom(0x18, part2);

            //byte[] part3 = new byte[] { 0x01 };
            //ws980.WriteEprom(0x1C, part3);

        }

        private void btnSetParameter_Click(object sender, EventArgs e)
        {
            var parameterForm = new ParameterForm(ws980);
            parameterForm.Show();
        }
    }
}
