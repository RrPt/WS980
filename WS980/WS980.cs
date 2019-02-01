using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WS980
{
    class ConnectionData
    {
        public String MAC = "mac";
        public String IP = "ip";
        public int Port = 45000;
        public String Name = "noName";
        public bool ok=false;

        public override string ToString()
        {
            if (ok) return IP + ":" + Port + " (" + MAC + ")";
            else return "<no ConnectionData>";
        }
    }
    class WS980
    {
        private ConnectionData connectionData;
        WS980DataSetDef dataSetDef1;

        public WS980(ConnectionData connectionData)
        {
            this.connectionData = connectionData;

            dataSetDef1 = new WS980DataSetDef();
            dataSetDef1.DataItemList.Add(1, new WS980DataItemDef(2, "TempIn", "°C"));
            dataSetDef1.DataItemList.Add(2, new WS980DataItemDef(2, "TempOut", "°C"));
            dataSetDef1.DataItemList.Add(3, new WS980DataItemDef(2, "TempTau", "°C"));
            dataSetDef1.DataItemList.Add(4, new WS980DataItemDef(2, "TempX", "°C"));
            dataSetDef1.DataItemList.Add(5, new WS980DataItemDef(2, "TempHitzeIdx", "°C"));

            dataSetDef1.DataItemList.Add(6, new WS980DataItemDef(1, "FeuchteIn", "%"));
            dataSetDef1.DataItemList.Add(7, new WS980DataItemDef(1, "FeuchteOut", "%"));
            dataSetDef1.DataItemList.Add(8, new WS980DataItemDef(2, "Val8", "?"));
            dataSetDef1.DataItemList.Add(9, new WS980DataItemDef(2, "Val9", "?"));
            dataSetDef1.DataItemList.Add(10, new WS980DataItemDef(2, "Val10", "?"));
            dataSetDef1.DataItemList.Add(11, new WS980DataItemDef(2, "Val11", "?"));
            dataSetDef1.DataItemList.Add(12, new WS980DataItemDef(2, "Val12", "?"));
            dataSetDef1.DataItemList.Add(14, new WS980DataItemDef(4, "Val14", "?"));
            dataSetDef1.DataItemList.Add(16, new WS980DataItemDef(4, "Val16", "?"));
            dataSetDef1.DataItemList.Add(17, new WS980DataItemDef(4, "Val17", "?"));
            dataSetDef1.DataItemList.Add(18, new WS980DataItemDef(4, "Val18", "?"));
            dataSetDef1.DataItemList.Add(19, new WS980DataItemDef(4, "Val19", "?"));
            dataSetDef1.DataItemList.Add(20, new WS980DataItemDef(4, "Val20", "?"));
            dataSetDef1.DataItemList.Add(21, new WS980DataItemDef(4, "Val21", "?"));
            dataSetDef1.DataItemList.Add(22, new WS980DataItemDef(4, "Val22", "Pa"));


        }

        internal ConnectionData ConnectionData { get => connectionData; set => connectionData = value; }

        #region get Connection Infos (static)
        static UdpClient receiveClient;
        static IPEndPoint RemoteIpEndPoint;
        static List<ConnectionData> connectionDataList = new List<ConnectionData>();

        static public List<ConnectionData> RequestAllStations()
        {

            int timeout = 2000;
            connectionDataList.Clear();

            // todo subnetz vom Rechner abfragen
            var sendClient = new UdpClient("192.168.22.255", 46000);
            sendClient.Send(new byte[] { 0xff, 0xff, 0x12, 0x00, 0x04, 0x16 }, 6);
            IPEndPoint lep = (IPEndPoint)sendClient.Client.LocalEndPoint;
            sendClient.Close();

            receiveClient = new UdpClient(lep.Port);
            RemoteIpEndPoint = lep;


            receiveClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);

            //// Blocks until a message returns on this socket from a remote host.
            DateTime startTime = DateTime.UtcNow;
            while ((DateTime.UtcNow - startTime).TotalMilliseconds < timeout)
            {
                Thread.Sleep(100);
            }
            return connectionDataList;
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            //UdpClient u = ((UdpState)(ar.AsyncState)).u;
            //IPEndPoint e = ((UdpState)(ar.AsyncState)).e;

            byte[] receiveBytes = receiveClient.EndReceive(ar, ref RemoteIpEndPoint);
            connectionDataList.Add(getConnectionData(receiveBytes));
        }

        private static ConnectionData getConnectionData(byte[] receiveBytes)
        {
            ConnectionData cd = new ConnectionData();
            String outstr = BitConverter.ToString(receiveBytes);
            cd.ok = false;
            if (receiveBytes.Length != 0x27) return cd;
            if (receiveBytes[0] != 0xFF) return cd;
            if (receiveBytes[1] != 0xFF) return cd;
            if (receiveBytes[2] != 0x12) return cd;
            if (receiveBytes[3] != 0x00) return cd;
            if (receiveBytes[4] != 0x27) return cd;
            // MAC 5-10
            cd.MAC = BitConverter.ToString(receiveBytes.Skip(5).Take(6).ToArray()).Replace("-", ":");
            // IP 11-14
            cd.IP = String.Format("{0}.{1}.{2}.{3}", receiveBytes[11], receiveBytes[12], receiveBytes[13], receiveBytes[14]);
            // Port 15-16
            cd.Port = 256 * (int)receiveBytes[15] + receiveBytes[16];
            // Name 17..
            cd.Name = "?";
            cd.ok = true;
            return cd;
        }
        #endregion

        internal SortedList<int, string> getData()
        {
            byte[] bef = { 0xff, 0xff, 0x0b, 0x00, 0x06, 0x04, 0x04, 0x19 };    // Aktuell
            //byte[] bef = { 0xff, 0xff, 0x0b, 0x00, 0x06, 0x05, 0x05, 0x1b };  // MAX
            //byte[] bef = { 0xff, 0xff, 0x0b, 0x00, 0x06, 0x06, 0x06, 0x1d };  // 
            //byte[] bef = { 0xff, 0xff, 0x0b, 0x00, 0x06, 0x07, 0x07, 0x1f };  // 
            //byte[] bef = { 0xff, 0xff, 0x0b, 0x00, 0x06, 0x08, 0x08, 0x21 };  // 
            //byte[] bef = { 0xff, 0xff, 0x0b, 0x00, 0x09, 0x02, 0x00, 0x00, 0x80, 0x82, 0x18 };  // 
            //byte[] bef = { 0xff, 0xff, 0x0b, 0x00, 0x09, 0x02, 0x80, 0x00, 0x80, 0x02, 0x18 };  // 
            //byte[] bef = { 0xff, 0xff, 0x0b, 0x00, 0x09, 0x02, 0x00, 0x01, 0x80, 0x83, 0x1a };  // 
            //byte[] bef = { 0xff, 0xff, 0x0b, 0x00, 0x09, 0x02, 0x80, 0x01, 0x80, 0x03, 0x1a };  // 

            //byte[] bef = { 0xff, 0xff, 0x0b, 0x00, 0x09, 0x02, 0x00, 0x02, 0x80, 0x84, 0x1c };  // 
            //byte[] bef = { 0xff, 0xff, 0x0b, 0x00, 0x09, 0x02, 0x80, 0x02, 0x80, 0x04, 0x1c };  // 
            //byte[] bef = { 0xff, 0xff, 0x0b, 0x00, 0x09, 0x02, 0x00, 0x03, 0x80, 0x85, 0x1e };  // 
            //byte[] bef = { 0xff, 0xff, 0x0b, 0x00, 0x09, 0x02, 0x80, 0x03, 0x80, 0x05, 0x1e };  // 

            //byte[] bef = { 0xff, 0xff, 0x0b, 0x00, 0x09, 0x02, 0x00, 0x04, 0x80, 0x86, 0x20 };  // 
            // .....


            //todo hier gehts weiter
            byte[] erg = getData(bef);
            SortedList<int, string> valueList = getValues(erg,dataSetDef1);

            return valueList;
        }


        private byte[] getData(byte[] bef)
        {
            byte[] recBuf = new byte[100];
            try
            {
                TcpClient tcpclnt = new TcpClient();
                Console.WriteLine("Connecting.....");

                tcpclnt.Connect(connectionData.IP, connectionData.Port);

                Console.WriteLine("Connected");
                Stream stm = tcpclnt.GetStream();
                Console.WriteLine("Transmitting.....");

                stm.Write(bef, 0, bef.Length);

                int k = stm.Read(recBuf, 0, recBuf.Length);
                recBuf = recBuf.Take(k).ToArray();

                for (int i = 0; i < k; i++)
                    Console.Write("{0:X2} ",recBuf[i]);

                tcpclnt.Close();
            }

            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
            return recBuf;
        }


        private SortedList<int, string> getValues(byte[] receiveBytes, WS980DataSetDef dataSetDef)
        {
            SortedList<int, string> list = new SortedList<int, string>();

            if (receiveBytes[0] != 0xFF) return list;
            if (receiveBytes[1] != 0xFF) return list;
            if (receiveBytes[2] != 0x0b) return list;   // todo evtl. mit dem Befehl[2] vergleichen
            int len = 256 * (int)receiveBytes[3] + receiveBytes[4];
            int para = receiveBytes[5];
            list.Add(0, BitConverter.ToString(receiveBytes));
            int idx = 6;

            //while (idx< receiveBytes.Length-2)
            foreach (var dataItemDef in dataSetDef1.DataItemList)
            {
                string erg = getDataItem(ref idx,receiveBytes,dataItemDef );
                if (!list.ContainsKey(dataItemDef.Key))  list.Add(dataItemDef.Key, erg);
            }



            return list;
        }

        private string getDataItem(ref int idx, byte[] receiveBytes, KeyValuePair<int, WS980DataItemDef> dataItemDef)
        {
            string erg="?";
            int dataIdx = receiveBytes[idx++];
            if (dataIdx != dataItemDef.Key) Console.WriteLine("falsche Datenlänge");
            if (dataItemDef.Value.Length==2)
            {
                float val =( 256 * (int)receiveBytes[idx ] + receiveBytes[idx + 1])/10.0f;
                erg = String.Format("\n\r{0}={1}{2}", dataItemDef.Value.Name, val, dataItemDef.Value.Unit);
                Console.WriteLine("\n\r{0}={1}{2}",dataItemDef.Value.Name,val ,dataItemDef.Value.Unit);
                idx += 2;
            }
            else if (dataItemDef.Value.Length == 1)
            {
                float val =  receiveBytes[idx ];
                erg = String.Format("\n\r{0}={1}{2}", dataItemDef.Value.Name, val, dataItemDef.Value.Unit);
                Console.WriteLine("\n\r{0}={1}{2}", dataItemDef.Value.Name, val, dataItemDef.Value.Unit);
                idx += 1;
            }
            else if (dataItemDef.Value.Length == 4)
            {
                float val = 0;
                for (int i = 0; i < 4; i++)
                {
                    val = val * 256 + receiveBytes[idx+i];
                }
                erg = String.Format("\n\r{0}={1}{2}", dataItemDef.Value.Name, val, dataItemDef.Value.Unit);
                Console.WriteLine("\n\r{0}={1}{2}", dataItemDef.Value.Name, val, dataItemDef.Value.Unit);
                idx += 4;
            }

            return erg;
        }
    }
}
