﻿using System;
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
            dataSetDef1.DataItemList.Add(1, new WS980DataItemDef(2, "TempIn", "°C",1));
            dataSetDef1.DataItemList.Add(2, new WS980DataItemDef(2, "TempOut", "°C",1));
            dataSetDef1.DataItemList.Add(3, new WS980DataItemDef(2, "TempTau", "°C",1));
            dataSetDef1.DataItemList.Add(4, new WS980DataItemDef(2, "TempX", "°C",1));
            dataSetDef1.DataItemList.Add(5, new WS980DataItemDef(2, "TempHitzeIdx", "°C",1));

            dataSetDef1.DataItemList.Add(6, new WS980DataItemDef(1, "FeuchteIn", "%"));
            dataSetDef1.DataItemList.Add(7, new WS980DataItemDef(1, "FeuchteOut", "%"));
            dataSetDef1.DataItemList.Add(8, new WS980DataItemDef(2, "Druck1", "hPa",1));
            dataSetDef1.DataItemList.Add(9, new WS980DataItemDef(2, "Druck2", "hPa", 1));
            dataSetDef1.DataItemList.Add(10, new WS980DataItemDef(2, "Windrichtung", "°", 0));
            dataSetDef1.DataItemList.Add(11, new WS980DataItemDef(2, "Windgeschw", "m/s", 1));
            dataSetDef1.DataItemList.Add(12, new WS980DataItemDef(2, "WindBö", "m/s", 1));
            dataSetDef1.DataItemList.Add(14, new WS980DataItemDef(4, "Regen/h", "mm",1));
            dataSetDef1.DataItemList.Add(16, new WS980DataItemDef(4, "Regen/d", "mm", 1));
            dataSetDef1.DataItemList.Add(17, new WS980DataItemDef(4, "Regen/w", "mm", 1));
            dataSetDef1.DataItemList.Add(18, new WS980DataItemDef(4, "Regen/M", "mm", 1));
            dataSetDef1.DataItemList.Add(19, new WS980DataItemDef(4, "Regen/J", "mm", 1));
            dataSetDef1.DataItemList.Add(20, new WS980DataItemDef(4, "Regen/T", "mm", 1));
            dataSetDef1.DataItemList.Add(21, new WS980DataItemDef(4, "Licht", "lux", 1));
            dataSetDef1.DataItemList.Add(22, new WS980DataItemDef(4, "Val22", ""));


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
            byte[] befActValues = { 0xff, 0xff, 0x0b, 0x00, 0x06, 0x04, 0x04, 0x19 };   // Aktuell
            byte[] befMaxValues = { 0xff, 0xff, 0x0b, 0x00, 0x06, 0x05, 0x05, 0x1b };   // MAX
            byte[] befMinValues = { 0xff, 0xff, 0x0b, 0x00, 0x06, 0x06, 0x06, 0x1d };     // Min
            byte[] bef7 = { 0xff, 0xff, 0x0b, 0x00, 0x06, 0x07, 0x07, 0x1f };  // 
            byte[] bef8 = { 0xff, 0xff, 0x0b, 0x00, 0x06, 0x08, 0x08, 0x21 };  // 
            //byte[] bef = { 0xff, 0xff, 0x0b, 0x00, 0x09, 0x02, 0x00, 0x00, 0x80, 0x82, 0x18 };  // 
            //byte[] bef = { 0xff, 0xff, 0x0b, 0x00, 0x09, 0x02, 0x80, 0x00, 0x80, 0x02, 0x18 };  // 
            //byte[] bef = { 0xff, 0xff, 0x0b, 0x00, 0x09, 0x02, 0x00, 0x01, 0x80, 0x83, 0x1a };  // 
            //byte[] bef = { 0xff, 0xff, 0x0b, 0x00, 0x09, 0x02, 0x80, 0x01, 0x80, 0x03, 0x1a };  // 

            //todo hier gehts weiter
            SortedList<int, string> valuesList = new SortedList<int, string>();
            getValues(getData(befActValues), dataSetDef1,0, valuesList);
            getValues(getData(befMaxValues), dataSetDef1,500, valuesList);
            getValues(getData(befMinValues), dataSetDef1, 600, valuesList);
            //getValues(getData(bef7), dataSetDef1, 700, valuesList);
            //getValues(getData(bef8), dataSetDef1, 800, valuesList);

            return valuesList;
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


        private SortedList<int, string> getValues(byte[] receiveBytes, WS980DataSetDef dataSetDef, int idxOffset, SortedList<int, string> valuesList)
        {
            if (receiveBytes[0] != 0xFF) return valuesList;
            if (receiveBytes[1] != 0xFF) return valuesList;
            if (receiveBytes[2] != 0x0b) return valuesList;   // todo evtl. mit dem Befehl[2] vergleichen
            int len = 256 * (int)receiveBytes[3] + receiveBytes[4];
            int para = receiveBytes[5];
            valuesList.Add(0+ idxOffset, BitConverter.ToString(receiveBytes));
            int idx = 6;

            while (idx< receiveBytes.Length-2)
            //foreach (var dataItemDef in dataSetDef1.DataItemList)
            {
                string erg = getNextDataItem(ref idx,receiveBytes,valuesList, idxOffset);
                //if (!list.ContainsKey(dataItemDef.Key))  list.Add(dataItemDef.Key, erg);
            }



            return valuesList;
        }

        private string getNextDataItem(ref int idx, byte[] receiveBytes, SortedList<int, string> list, int idxOffset)
        {
            string erg="?";
            int dataIdx = receiveBytes[idx++];
            if (!dataSetDef1.DataItemList.ContainsKey(dataIdx))
            {
                Console.WriteLine("SensorNo {0} nicht definiert", dataIdx);
                return "Err";
            }
            var itemDef = dataSetDef1.DataItemList[dataIdx];
            if (itemDef.Length==2)
            {
                float val =( 256 * (int)receiveBytes[idx ] + receiveBytes[idx + 1])* itemDef.Scale;
                erg = String.Format("\n\r{0,-15}={1,8:0.0}{2}", itemDef.Name, val, itemDef.Unit);
                Console.WriteLine("{0}={1}{2}", itemDef.Name,val , itemDef.Unit);
                idx += 2;
            }
            else if (itemDef.Length == 1)
            {
                float val =  receiveBytes[idx ] * itemDef.Scale;
                erg = String.Format("\n\r{0,-15}={1,8:0.0}{2}", itemDef.Name, val, itemDef.Unit);
                Console.WriteLine("{0}={1}{2}", itemDef.Name, val, itemDef.Unit);
                idx += 1;
            }
            else if (itemDef.Length == 4)
            {
                float val = 0;
                for (int i = 0; i < 4; i++)
                {
                    val = val * 256 + receiveBytes[idx+i];
                }
                val = val * itemDef.Scale;
                erg = String.Format("\n\r{0,-15}={1,8:0.0}{2}", itemDef.Name, val, itemDef.Unit);
                Console.WriteLine("{0}={1}{2}", itemDef.Name, val, itemDef.Unit);
                idx += 4;
            }
            if (list.ContainsKey(dataIdx + idxOffset))
            {
                list[dataIdx + idxOffset] += "; " + erg;
            }
            else
            {
                list.Add(dataIdx + idxOffset, erg);
            }
            return erg;
        }
    }
}
