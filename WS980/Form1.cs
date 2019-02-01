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
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SortedList<int, string>  list = ws980.getData();
            foreach (var item in list)
            {
                tBOut.AppendText(String.Format("{0:x}: {1}\n\r\n", item.Key, item.Value));
            }
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
