
//Infos siehe auch 
//https://github.com/matthewwall/weewx-wh23xx/blob/master/bin/user/wh23xx.py
//https://www.elv.de/topic/protokolldefinition-zum-datenaustausch-ws980-zum-pc.html

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
        actual, min, max, version,
        dayMax,
        dayMin
    }

    class TableDataItem
    {
        public int pageNo;
        public DateTime startTime;
        public ushort samplingIntervalInSec;
        public byte numberOfRecords;
        public ushort startDataAdr;

        public override string ToString()
        {
            return String.Format("Page[{0}]: Adr:{1:X4} Start:{2} Interv:{3}s Anz:{4}",pageNo,startDataAdr,startTime,samplingIntervalInSec,numberOfRecords);
        }
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
        private byte[] pageFlags = new byte[111];
        private TableDataItem[] tableData = new TableDataItem[111];
        private List<HistoricDataRecord> historicDataRecordList = new List<HistoricDataRecord>(); 

        internal void getHistory()
        {
            getPageFlags();
            getTableData();
            getAllHistoricRecords();
        }

        private void getAllHistoricRecords()
        {
            foreach (var tdi in tableData)
            {
                if (tdi!=null)
                {
                    Console.WriteLine("Page:{0}  Start:{1}   Anz={2}",tdi.pageNo,tdi.startTime,tdi.numberOfRecords);
                    for (int i = 0; i < tdi.numberOfRecords; i++)
                    {
                        DateTime recTime = tdi.startTime.AddSeconds(i * tdi.samplingIntervalInSec);
                        var byteRecord = ReadEprom(tdi.startDataAdr, 18);
                        HistoricDataRecord hdr = new HistoricDataRecord(recTime,byteRecord);
                        historicDataRecordList.Add(hdr);
                        var rec = Tools.ToString(byteRecord);
                        Console.WriteLine("{0}:{1}", recTime,rec);
                    }
                }
            }
        }

        private void getTableData()
        {
            ushort firstAdr = 0x2C8;
            ushort lastAdr = 0x63F;
            byte blockSize = 0x8;
            ushort adr = firstAdr;
            while (adr <= lastAdr)
            {
                int page = (adr - firstAdr) / 8;
                if (pageFlags[page] <= 0x20)
                {
                    var arr = ReadEprom(adr, blockSize);
                    // todo wenn exception bei Datum, dann record nochmal einlesen
                    TableDataItem tableDataItem = new TableDataItem();
                    tableDataItem.pageNo = page;
                    tableDataItem.startDataAdr = (ushort)(0x0640 + page * 18);
                    tableDataItem.numberOfRecords = (byte)(pageFlags[page] + 1);
                    tableDataItem.startTime = new DateTime(2000 + arr[0], arr[1], arr[2], arr[3], arr[4], arr[5]);
                    tableDataItem.samplingIntervalInSec =(ushort)( arr[7] == 0x01 ?  arr[6] : arr[6] * 60);
                    tableData[page] = tableDataItem;
                    Console.WriteLine("TableData[{0}]:{1}", page, tableData[page]);
                }
                else Console.WriteLine("TableData[{0}]:???", page);
                adr += blockSize;
            };
        }

        private void getPageFlags()
        {
            ushort firstAdr = 0x259;
            ushort lastAdr = 0x2c7;
            byte blockSize = 0x20;
            ushort adr = firstAdr;
            while (adr <= lastAdr )
            {
                if (adr + blockSize > lastAdr) blockSize = (byte)(lastAdr - adr+1);
                var arr = ReadEprom(adr, blockSize);
                for (ushort i = 0; i < blockSize; i++)
                {
                    //if (arr[i]!=0xff)
                    Console.Write("PageFlag[{0}]({1:X4}={2:X2})", adr + i- firstAdr, adr+i,arr[i]);
                    if (arr[i] == (byte)0xFF) Console.WriteLine(" empty");
                    else if (arr[i] > (byte)0x20) Console.WriteLine(" error");
                    else  Console.WriteLine();
                    pageFlags[adr + i - firstAdr] = arr[i];
                }
                adr += blockSize;
                Console.WriteLine();
            };
        }

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
            new WS980DataItemDef(21,4, "Licht", "lux", 1);
            new WS980DataItemDef(22,2, "UvRaw", "uW/m2");
            new WS980DataItemDef(23,1, "UvIdxRaw", "");


        }



        internal ConnectionData ConnectionData { get => connectionData; set => connectionData = value; }
        public string Version { get => version; set => version = value; }
        internal SortedList<int, WS980Sensor> SensorList { get => sensorList; set => sensorList = value; }

        #region get Connection Infos (static)
        static UdpClient receiveClient;
        static IPEndPoint RemoteIpEndPoint;
        static List<ConnectionData> connectionDataList = new List<ConnectionData>();
        private byte lcdContrast;

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
            byte[] befVersion =   { 0xff, 0xff, 0x50, 0x03, 0x53 };  // Version

            byte[] befActValues = Tools.GetBefArray(4);   // Aktuell
            byte[] befMaxValues = Tools.GetBefArray(5);   // MAX
            byte[] befMinValues = Tools.GetBefArray(6);    // Min
            byte[] befDayMaxValues = Tools.GetBefArray(7); // DayMax
            byte[] befDayMinValues = Tools.GetBefArray(8);  // DayMin

            // Eprom auslesen
            // History Records ab 0x2140..8200  zeiten ab 0x0300..0x0600 ???
            //byte[] bef = { 0xff, 0xff, 0x0b, 0x00, 0x09, 0x02, 0x00, 0x00, 0x80, 0x82, 0x18 };  // 
            //byte[] bef = { 0xff, 0xff, 0x0b, 0x00, 0x09, 0x02, 0x80, 0x00, 0x80, 0x02, 0x18 };  // 
            //...

            //byte[] bef = { 0xff, 0xff, 0x0b, 0x00, 0x09, 0x02, 0xFA, 0xF3, 0x90, 0x7F, 0x12 };  // 
            //byte[] bef = { 0xff, 0xff, 0x0b, 0x00, 0x09, 0x02, 0x8A, 0x4F, 0x90, 0x10, 0x34 };  // 

            Console.WriteLine("VER " + Tools.ToString(getValues(getAnswer(befVersion), ValueType.version)));
            Console.WriteLine("AKT" + Tools.ToString(getValues(getAnswer(befActValues), ValueType.actual)));
            Console.WriteLine("MAX" + Tools.ToString(getValues(getAnswer(befMaxValues), ValueType.max)));
            Console.WriteLine("MIN" + Tools.ToString(getValues(getAnswer(befMinValues), ValueType.min)));
            Console.WriteLine("DAX" + Tools.ToString(getValues(getAnswer(befDayMaxValues), ValueType.dayMax)));
            Console.WriteLine("DIN" + Tools.ToString(getValues(getAnswer(befDayMinValues), ValueType.dayMin)));

            //byte[] bef;
            byte[] erg;
            byte[] bef = { 0xff, 0xff, 0x0b, 0x00, 13, 0x01, 19, 2, 16,17,30,00, 1, 0x82, 0x18 };  // 
            bef[bef.Length - 2] = Tools.calcChecksum(bef.Skip(5).Take(bef.Length - 7));
            bef[bef.Length - 1] = Tools.calcChecksum(bef.Skip(2).Take(bef.Length - 3));
            erg = getAnswer(bef);
            //for (ushort adr = 0; adr < 0x9780; adr+=0x10)
            //{
            //    bef = Tools.GetReadEpromArray(adr, 0x20);
            //    erg = getData(bef);
            //    var data = erg.Skip(9).Take(erg.Length - 11).ToArray();
            //    string ergStr =  System.Text.Encoding.Default.GetString(data).Replace('\n', '.').Replace('\r', '.');
            //    Console.WriteLine("{0:X4}: {1}  {2}",adr,Tools.ToString(data),ergStr);
            //}

            // write

            //byte[] dta = new byte[] { lcdContrast };
            //bef = Tools.GetWriteEpromArray(0x1b,dta );
            //erg = getData(bef);

            //bef = Tools.GetReadEpromArray(0x1b, 1);
            //erg = getData(bef);
            //Console.WriteLine("lcd={0}",erg[9]);

            return;
        }


        private byte[] getAnswer(byte[] bef)
        {
            byte[] recBuf = new byte[100];
            try
            {
                TcpClient tcpclnt = new TcpClient();
                //Console.WriteLine("Connecting.....");

                tcpclnt.Connect(connectionData.IP, connectionData.Port);

                //Console.WriteLine("Connected");
                Stream stm = tcpclnt.GetStream();
                //Console.WriteLine("Transmitting.....");

                stm.Write(bef, 0, bef.Length);
                //Console.WriteLine("Sent: "+Tools.ToString(bef));
                byte crcIst1 = Tools.calcChecksum(bef.Skip(5).Take(bef.Length - 7));
                byte crcIst2 = Tools.calcChecksum(bef.Skip(2).Take(bef.Length - 3));
                //Console.WriteLine("Prüfsummen: {0:X2}  {1:X2}", crcIst1, crcIst2);
                stm.ReadTimeout = 1000;
                int k = stm.Read(recBuf, 0, recBuf.Length);  //todo hier noch timeout einbauen und behandeln
                recBuf = recBuf.Take(k).ToArray();

                //Console.WriteLine(Tools.ToString(recBuf));
                //for (int i = 0; i < k; i++)
                //    Console.Write("{0:X2} ",recBuf[i]);

                tcpclnt.Close();
            }

            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
            return recBuf;
        }


        private byte[] getValues(byte[] receiveBytes, ValueType valueType)
        {
            WriteDebugData(receiveBytes, valueType);
            //Console.WriteLine("\n{0}: {1} {2}\n", valueType, BitConverter.ToString(receiveBytes), Encoding.ASCII.GetString(receiveBytes));
            if (receiveBytes[0] != 0xFF) return null;
            if (receiveBytes[1] != 0xFF) return null;
            if (valueType == ValueType.version)
            {
                if (receiveBytes[2] != 0x50) return null;
                int len =  (int)receiveBytes[3];
                version = Encoding.ASCII.GetString(receiveBytes.Skip(5).ToArray());
            }
            else
            {
                // Prüfsumme berechnen
                int idxCrc = receiveBytes.Length - 2;
                byte crcIst1 = Tools.calcChecksum(receiveBytes.Skip(5).Take(receiveBytes.Length - 7));
                byte crcIst2 = Tools.calcChecksum(receiveBytes.Skip(2).Take(receiveBytes.Length - 3));
                //Console.WriteLine("Prüfsummen: {0:X2}  {1:X2}",crcIst1,crcIst2  );
                if (crcIst1!= receiveBytes[idxCrc])
                {
                    Console.WriteLine("1. Prüfsumme falsch CRCIst={0}  CrcSoll={1}\n", crcIst2, receiveBytes[idxCrc]);
                    return null;
                }
                if (crcIst2 != receiveBytes[idxCrc+1])
                {
                    Console.WriteLine("2. Prüfsumme falsch CRCIst={0}  CrcSoll={1}\n", crcIst2, receiveBytes[idxCrc+1]);
                    return null;
                }
                // Befehl prüfen
                if (receiveBytes[2] != 0x0b) return null;       
                int len = 256 * (int)receiveBytes[3] + receiveBytes[4];
                int para = receiveBytes[5];
                int idx = 6;

                while (idx < receiveBytes.Length - 2)
                {
                    idx = getNextDataItem(idx, receiveBytes, valueType);
                }
            }
            return receiveBytes;
        }

        private void WriteDebugData(byte[] receiveBytes, ValueType valueType)
        {
            string fn = String.Format("WS980_RAW_DATA_{0}.txt", valueType, ToString());
            string v = DateTime.Now.ToString() + ";"+ BitConverter.ToString(receiveBytes).Replace("-"," ");
            File.AppendAllText(fn, v + Environment.NewLine);
        }

        private int getNextDataItem(int idx, byte[] receiveBytes, ValueType valueType)
        {
            int deltaIdx = 0;
            int dataIdx = receiveBytes[idx];
            idx++;
            if (valueType == ValueType.dayMax | valueType == ValueType.dayMin)
            {
                dataIdx -= 0x40;
                deltaIdx += 2;  // hier ist die Zeit noch zusätzlich angegeben
            }

            if (!WS980DataItemDef.dataItemList.ContainsKey(dataIdx))
            {
                Console.WriteLine("SensorNo {0} nicht definiert", dataIdx);
                return -1;
            }
           
            WS980Sensor sensor = GetSensor(dataIdx);    // sensor finden oder neu anlegen
            deltaIdx += sensor.ItemDef.Length;
            sensor?.UpdateValue(receiveBytes.Skip(idx).Take(deltaIdx), valueType);
            idx += deltaIdx;
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

        internal string ToDataLine()
        {
            string erg = DateTime.Now.ToString()+";";
            foreach (var sensor in sensorList.Values)
            {
                erg += sensor.ActualValue.ToString() + ";";
            }
            return erg;
        }

        public byte[] ReadEprom(ushort adr, byte size)
        {
            var bef = GetReadEpromArrayBef(adr, size);
            var answer = getAnswer(bef);
            return answer.Skip(9).Take(size).ToArray();
        }

        public static byte[] GetReadEpromArrayBef(ushort adr, byte size)
        {
            //                               LenHi LenLo Bef   ardLo adrHi size  crc1  crc2
            byte[] arr = { 0xff, 0xff, 0x0b, 0x00, 0x09, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00 };
            //arr[4] = (byte)(arr.Length - 2);
            arr[6] = (byte)(adr & 0xFF);
            arr[7] = (byte)(adr >> 8);
            arr[8] = size;
            arr[arr.Length - 2] = Tools.calcChecksum(arr.Skip(5).Take(arr.Length - 7));
            arr[arr.Length - 1] = Tools.calcChecksum(arr.Skip(2).Take(arr.Length - 3));
            return arr;
        }

        public static byte[] GetWriteEpromArrayBef(ushort adr, byte[] dataArr)
        {
            byte[] arr = new byte[11 + dataArr.Length];

            //                                 LenHi LenLo Bef   ardLo adrHi size            crc1  crc2
            //byte[] arr = { 0xff, 0xff, 0x0b, 0x00, 0x09, 0x03, 0x00, 0x00, 0x00, dataArr ,   0x00, 0x00 };
            int size = 9 + dataArr.Length;
            arr[0] = (byte)0xFF;
            arr[1] = (byte)0xFF;
            arr[2] = (byte)0x0b;
            arr[3] = (byte)(size >> 8);
            arr[4] = (byte)(size & 0xFF);
            arr[5] = (byte)0x03;    // write Eprom
            //arr[4] = (byte)(arr.Length - 2);
            arr[6] = (byte)(adr & 0xFF);
            arr[7] = (byte)(adr >> 8);
            arr[8] = (byte)dataArr.Length;
            for (int i = 0; i < dataArr.Length; i++)
            {
                arr[9 + i] = dataArr[i];
            }
            arr[arr.Length - 2] = Tools.calcChecksum(arr.Skip(5).Take(arr.Length - 7));
            arr[arr.Length - 1] = Tools.calcChecksum(arr.Skip(2).Take(arr.Length - 3));
            return arr;
        }
    }
}
