using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WS980
{
    public partial class Form1 : Form
    {
        WS980 ws980 = null;
        SortedList<int, string> dataList;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataList = ws980.getData();
            foreach (var item in dataList)
            {
                if (item.Key > 99) continue;
                int SensId = item.Key;
                tBOut.AppendText(String.Format("{0,3}: {1}  <{2} , {3}>\r\n", item.Key, getVal(SensId), getVal(SensId+600), getVal(SensId+500)));
            }

        }

        private object getVal(int sensId)
        {
            if (dataList.ContainsKey(sensId)) return dataList[sensId];
            return "";
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
        }
    }
}
