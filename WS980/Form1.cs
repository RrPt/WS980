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
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetComParameter();
        }

        private void GetComParameter()
        {
            var sendClient = new UdpClient("192.168.22.255",46000);
            sendClient.Send(new byte[] { 0xff, 0xff, 0x12, 0x00, 0x04, 0x16 }, 6);
            IPEndPoint lep = (IPEndPoint)sendClient.Client.LocalEndPoint;
            sendClient.Close();


            var receiveClient = new UdpClient(lep.Port);
            IPEndPoint RemoteIpEndPoint = lep ;// new IPEndPoint(IPAddress.Any, 0);

            //// Blocks until a message returns on this socket from a remote host.
            Byte[] receiveBytes = receiveClient.Receive(ref RemoteIpEndPoint);
            String outstr = BitConverter.ToString(receiveBytes);
            tBOut.AppendText(outstr + Environment.NewLine);

        }
    }
}
