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
    enum ValueType
    {
        actual, min, max, version
    }

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
        private string version = "V?";
        // List of  Sensors
        private SortedList<int, WS980Sensor> sensorList = new SortedList<int, WS980Sensor>();

        public WS980(ConnectionData connectionData)
        {
            this.connectionData = connectionData;
            SortedList<int, WS980DataItemDef> dataSetDef1 = WS980DataItemDef.dataItemList;
            new WS980DataItemDef(1,2, "TempIn", "°C",1);
            new WS980DataItemDef(2,2, "TempOut", "°C",1);
            new WS980DataItemDef(3,2, "TempTau", "°C",1);
            new WS980DataItemDef(4,2, "TempGefühlt", "°C",1);
            new WS980DataItemDef(5,2, "TempHitzeIdx", "°C",1);

            new WS980DataItemDef(6,1, "FeuchteIn", "%");
            new WS980DataItemDef(7,1, "FeuchteOut", "%");
            new WS980DataItemDef(8,2, "DruckAbs", "hPa",1);
            new WS980DataItemDef(9,2, "DruckRel", "hPa", 1);
            new WS980DataItemDef(10,2, "Windrichtung", "°", 0);
            new WS980DataItemDef(11,2, "Windgeschw", "m/s", 1);
            new WS980DataItemDef(12,2, "WindBö", "m/s", 1);
            new WS980DataItemDef(14,4, "Regen/h", "mm",1);
            new WS980DataItemDef(16,4, "Regen/d", "mm", 1);
            new WS980DataItemDef(17,4, "Regen/w", "mm", 1);
            new WS980DataItemDef(18,4, "Regen/M", "mm", 1);
            new WS980DataItemDef(19,4, "Regen/J", "mm", 1);
            new WS980DataItemDef(20,4, "Regen/T", "mm", 1);
            new WS980DataItemDef(21,4, "Licht", "lux", 2);
            new WS980DataItemDef(22,2, "Val22", "");
            new WS980DataItemDef(23,2, "Val23", "");


        }

        internal ConnectionData ConnectionData { get => connectionData; set => connectionData = value; }
        public string Version { get => version; set => version = value; }
        internal SortedList<int, WS980Sensor> SensorList { get => sensorList; set => sensorList = value; }

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

        internal void getData()
        {
            byte[] befVersion =   { 0xff, 0xff, 0x50, 0x03, 0x53 };  // 
            byte[] befActValues = { 0xff, 0xff, 0x0b, 0x00, 0x06, 0x04, 0x04, 0x19 };   // Aktuell
            byte[] befMaxValues = { 0xff, 0xff, 0x0b, 0x00, 0x06, 0x05, 0x05, 0x1b };   // MAX
            byte[] befMinValues = { 0xff, 0xff, 0x0b, 0x00, 0x06, 0x06, 0x06, 0x1d };     // Min
            byte[] bef7 = { 0xff, 0xff, 0x0b, 0x00, 0x06, 0x07, 0x07, 0x1f };  // 
            byte[] bef8 = { 0xff, 0xff, 0x0b, 0x00, 0x06, 0x08, 0x08, 0x21 };  // 
            //byte[] bef = { 0xff, 0xff, 0x0b, 0x00, 0x09, 0x02, 0x00, 0x00, 0x80, 0x82, 0x18 };  // 
            //byte[] bef = { 0xff, 0xff, 0x0b, 0x00, 0x09, 0x02, 0x80, 0x00, 0x80, 0x02, 0x18 };  // 
            //byte[] bef = { 0xff, 0xff, 0x0b, 0x00, 0x09, 0x02, 0x00, 0x01, 0x80, 0x83, 0x1a };  // 
            //byte[] bef = { 0xff, 0xff, 0x0b, 0x00, 0x09, 0x02, 0x80, 0x01, 0x80, 0x03, 0x1a };  // 

            getValues(getData(befVersion), ValueType.version);
            getValues(getData(befActValues), ValueType.actual);
            getValues(getData(befMaxValues), ValueType.max);
            getValues(getData(befMinValues), ValueType.min);

            return ;
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


        private void getValues(byte[] receiveBytes, ValueType valueType)
        {
            Console.WriteLine("{0}: {1} {2}\n", valueType, BitConverter.ToString(receiveBytes), Encoding.ASCII.GetString(receiveBytes));
            if (receiveBytes[0] != 0xFF) return ;
            if (receiveBytes[1] != 0xFF) return ;
            if (valueType == ValueType.version)
            {
                if (receiveBytes[2] != 0x50) return;
                int len =  (int)receiveBytes[3];
                version = Encoding.ASCII.GetString(receiveBytes.Skip(5).ToArray());
            }
            else
            {
                if (receiveBytes[2] != 0x0b) return;
                // todo evtl. mit dem Befehl[2] vergleichen
                int len = 256 * (int)receiveBytes[3] + receiveBytes[4];
                int para = receiveBytes[5];
                int idx = 6;

                while (idx < receiveBytes.Length - 2)
                {
                    idx = getNextDataItem(idx, receiveBytes, valueType);
                }
            }
        }

        private int getNextDataItem(int idx, byte[] receiveBytes, ValueType valueType)
        {
            int dataIdx = receiveBytes[idx++];
            if (!WS980DataItemDef.dataItemList.ContainsKey(dataIdx))
            {
                Console.WriteLine("SensorNo {0} nicht definiert", dataIdx);
                return -1;
            }
           
            WS980Sensor sensor = GetSensor(dataIdx);    // sensor finden oder neu anlegen

            sensor?.UpdateValue(receiveBytes.Skip(idx).Take(sensor.ItemDef.Length), valueType);
            idx += sensor.ItemDef.Length;

            return idx;
        }

        private WS980Sensor GetSensor(int dataIdx)
        {
            if (!WS980DataItemDef.dataItemList.ContainsKey(dataIdx)) return null ;
            var itemDef = WS980DataItemDef.dataItemList[dataIdx];

            if (sensorList.ContainsKey(dataIdx)) return sensorList[dataIdx];    // existiert
            // neu anlegen

            var sensor = new WS980Sensor(itemDef);
            sensorList.Add(dataIdx, sensor);
            return sensor;
        }
    }
}
