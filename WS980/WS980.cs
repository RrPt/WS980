
//Infos siehe auch 
//https://github.com/matthewwall/weewx-wh23xx/blob/master/bin/user/wh23xx.py
//https://www.elv.de/topic/protokolldefinition-zum-datenaustausch-ws980-zum-pc.html

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            if (ok) return Name +"  "+ IP + ":" + Port + " (" + MAC + ")";
            else return "<no ConnectionData>";
        }
    }
    class WS980
    {
        private WS980Parameter para = null;
        private ConnectionData connectionData;
        private string version = "V?";
        // List of  Sensors
        private SortedList<int, WS980Sensor> sensorList = new SortedList<int, WS980Sensor>();
        // Historic Data
        private byte[] pageFlags = new byte[111];
        private TableDataItem[] tableData = new TableDataItem[111];
        private List<HistoricDataRecord> historicDataRecordList = new List<HistoricDataRecord>();

        #region Historic Records
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
                if (tdi != null)
                {
                    Tools.WriteLine("Page:{0}  Start:{1}   Anz={2}", tdi.pageNo, tdi.startTime, tdi.numberOfRecords);
                    for (int i = 0; i < tdi.numberOfRecords; i++)
                    {
                        DateTime recTime = tdi.startTime.AddSeconds(i * tdi.samplingIntervalInSec);
                        var byteRecord = ReadEprom((ushort)(tdi.startDataAdr + 18 * i), 18);
                        HistoricDataRecord hdr = new HistoricDataRecord(recTime, byteRecord);
                        var rec = Tools.ToString(byteRecord, "{0:X2}={0:d3}  ");
                        //Tools.WriteLine("{0}:[0x{2:x4}] {1}", recTime,rec, tdi.startDataAdr + 18 * i);
                        Tools.WriteLine("    {0}", hdr.ToString());
                        historicDataRecordList.Add(hdr);
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
                if (pageFlags[page] < 0x20)
                {
                    int retry = 0;
                    bool ok = false;
                    while (!ok & retry < 3)
                    {
                        try
                        {
                            var arr = ReadEprom(adr, blockSize);
                            //Tools.WriteLine("TableData[{0}]: {1}", page, Tools.ToString(arr));
                            TableDataItem tableDataItem = new TableDataItem();
                            tableDataItem.pageNo = page;
                            tableDataItem.startDataAdr = (ushort)(0x0640 + page * 18 * 32);
                            tableDataItem.numberOfRecords = (byte)(pageFlags[page] + 1);
                            tableDataItem.startTime = new DateTime(2000 + arr[0], arr[1], arr[2], arr[3], arr[4], arr[5]); // UTC
                            tableDataItem.samplingIntervalInSec = (ushort)(arr[7] == 0x01 ? arr[6] : arr[6] * 60);
                            tableData[page] = tableDataItem;
                            ok = true;
                        }
                        catch (Exception)
                        {
                            retry++;
                        }
                        if (!ok) Tools.WriteLine("Error in GetTableData");
                    }
                    Tools.WriteLine("TableData {0} [0x{1:X4}] : {2}", page, adr, tableData[page]);
                }
                else
                {
                    tableData[page] = null;
                    Tools.WriteLine("TableData[{0}]:???", page);
                }

                adr += blockSize;
            };
        }

        internal WS980Parameter ReadParameter()
        {
            para = new WS980Parameter(this);
            return para;
        }
        internal bool WriteParameter()
        {
            return para.WriteParameter();
        }
        private void getPageFlags()
        {
            ushort firstAdr = 0x259;
            ushort lastAdr = 0x2c7;
            byte blockSize = 0x20;
            ushort adr = firstAdr;
            while (adr <= lastAdr)
            {
                if (adr + blockSize > lastAdr) blockSize = (byte)(lastAdr - adr + 1);
                var arr = ReadEprom(adr, blockSize);
                for (ushort i = 0; i < blockSize; i++)
                {
                    //if (arr[i]!=0xff)
                    string dataLine = String.Format("PageFlag[{0}]({1:X4}={2:X2})", adr + i - firstAdr, adr + i, arr[i]);
                    if (arr[i] == (byte)0xFF) Tools.WriteLine(dataLine + " empty");
                    else if (arr[i] > (byte)0x20) Tools.WriteLine(dataLine + " error");
                    else Tools.WriteLine(dataLine);
                    pageFlags[adr + i - firstAdr] = arr[i];
                }
                adr += blockSize;

            };
        }


        /// <summary>
        /// Parameter im EPROM hat sich geändert
        /// </summary>
        /// <param name="v"></param>
        internal void ChangedParameter(short v)
        {
            var bef = GetParamChangedArrayBef(v);
            var answer = getAnswer(bef);
        }

        public bool ClearAllHistory()
        {
            byte[] bef = GetBefArrayBef(0x0b);
            var answer = getAnswer(bef);
            return answer.SequenceEqual(bef);   // wenn alles ok wird der Befehl zurückgesendet
        }
        #endregion

        #region Konstroktor
        public WS980(ConnectionData connectionData)
        {
            this.connectionData = connectionData;
            SortedList<int, WS980DataItemDef> dataSetDef1 = WS980DataItemDef.dataItemList;
            new WS980DataItemDef(1, 2, "TempIn", "°C", 1);
            new WS980DataItemDef(2, 2, "TempOut", "°C", 1);
            new WS980DataItemDef(3, 2, "TempTau", "°C", 1);
            new WS980DataItemDef(4, 2, "TempGefühlt", "°C", 1);
            new WS980DataItemDef(5, 2, "TempHitzeIdx", "°C", 1);

            new WS980DataItemDef(6, 1, "FeuchteIn", "%");
            new WS980DataItemDef(7, 1, "FeuchteOut", "%");
            new WS980DataItemDef(8, 2, "DruckAbs", "hPa", 1);
            new WS980DataItemDef(9, 2, "DruckRel", "hPa", 1);
            new WS980DataItemDef(10, 2, "Windrichtung", "°", 0);
            new WS980DataItemDef(11, 2, "Windgeschw", "m/s", 1);
            new WS980DataItemDef(12, 2, "WindBö", "m/s", 1);
            new WS980DataItemDef(14, 4, "Regen/h", "mm", 1);
            new WS980DataItemDef(16, 4, "Regen/d", "mm", 1);
            new WS980DataItemDef(17, 4, "Regen/w", "mm", 1);
            new WS980DataItemDef(18, 4, "Regen/M", "mm", 1);
            new WS980DataItemDef(19, 4, "Regen/J", "mm", 1);
            new WS980DataItemDef(20, 4, "Regen/T", "mm", 1);
            new WS980DataItemDef(21, 4, "Licht", "lux", 1);
            new WS980DataItemDef(22, 2, "UvRaw", "uW/m2");
            new WS980DataItemDef(23, 1, "UvIdxRaw", "");


        }

        #endregion

        #region Zugriffsfunktionen

        internal ConnectionData ConnectionData { get => connectionData; set => connectionData = value; }
        public string Version { get => version; set => version = value; }
        internal SortedList<int, WS980Sensor> SensorList { get => sensorList; set => sensorList = value; }
        internal WS980Parameter Para { get => para; set => para = value; }

        #endregion

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
            // NameLen 17
            int nameLen = (int)receiveBytes[17];
            // Name 18.. 37 
            cd.Name = Tools.ByteArrayToString(receiveBytes.Skip(18).Take(nameLen).ToArray());
            // CRC 38
            byte crcIst = Tools.calcChecksum(receiveBytes, 2, receiveBytes.Length - 2);
            if (crcIst!=receiveBytes[18+nameLen])
            {
                Tools.WriteLine("Prüfsumme falsch");
                return cd;
            }
            cd.ok = true;
            string dbg = Tools.ToString(receiveBytes,"0x{0:X2} ");
            string dbgStr = Tools.ByteArrayToString(receiveBytes);
            return cd;
        }
        #endregion

        internal void getData()
        {
            byte[] befVersion =   { 0xff, 0xff, 0x50, 0x03, 0x53 };  // Version

            byte[] befActValues = WS980.GetBefArrayBef(4);   // Aktuell
            byte[] befMaxValues = WS980.GetBefArrayBef(5);   // MAX
            byte[] befMinValues = WS980.GetBefArrayBef(6);    // Min
            byte[] befDayMaxValues = WS980.GetBefArrayBef(7); // DayMax
            byte[] befDayMinValues = WS980.GetBefArrayBef(8);  // DayMin

            Tools.WriteLine("VER " + Tools.ToString(getValues(getAnswer(befVersion), ValueType.version)));
            Tools.WriteLine("AKT" + Tools.ToString(getValues(getAnswer(befActValues), ValueType.actual)));
            Tools.WriteLine("MAX" + Tools.ToString(getValues(getAnswer(befMaxValues), ValueType.max)));
            Tools.WriteLine("MIN" + Tools.ToString(getValues(getAnswer(befMinValues), ValueType.min)));
            Tools.WriteLine("DAX" + Tools.ToString(getValues(getAnswer(befDayMaxValues), ValueType.dayMax)));
            Tools.WriteLine("DIN" + Tools.ToString(getValues(getAnswer(befDayMinValues), ValueType.dayMin)));

            return;
        }


        private byte[] getAnswer(byte[] bef)
        {
            byte[] recBuf = new byte[0x100];
            try
            {
                TcpClient tcpclnt = new TcpClient();

                tcpclnt.Connect(connectionData.IP, connectionData.Port);
                Stream stm = tcpclnt.GetStream();
                stm.Write(bef, 0, bef.Length);

                stm.ReadTimeout = 1000;
                int k = stm.Read(recBuf, 0, recBuf.Length);  
                if (k==0)
                {
                    Tools.WriteLine("No Data received in getAnswer");
                    return null;
                }
                recBuf = recBuf.Take(k).ToArray();

                tcpclnt.Close();
            }
            catch (Exception e)
            {
                Tools.WriteLine("Error in getAnswer ..... " + e.StackTrace);
                return null;
            }
            return recBuf;
        }


        private byte[] getValues(byte[] receiveBytes, ValueType valueType)
        {
            string dbg = Tools.ToString(receiveBytes, "0x{0:X2} ");
            string dbgStr = Tools.ByteArrayToString(receiveBytes);
            //WriteDebugData(receiveBytes, valueType);
            //Tools.WriteLine("\n{0}: {1} {2}\n", valueType, BitConverter.ToString(receiveBytes), Encoding.ASCII.GetString(receiveBytes));
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
                //Tools.WriteLine("Prüfsummen: {0:X2}  {1:X2}",crcIst1,crcIst2  );
                if (crcIst1!= receiveBytes[idxCrc])
                {
                    Tools.WriteLine("1. Prüfsumme falsch CRCIst={0}  CrcSoll={1}\n", crcIst2, receiveBytes[idxCrc]);
                    return null;
                }
                if (crcIst2 != receiveBytes[idxCrc+1])
                {
                    Tools.WriteLine("2. Prüfsumme falsch CRCIst={0}  CrcSoll={1}\n", crcIst2, receiveBytes[idxCrc+1]);
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
                    if (idx<0)
                    {
                        return null;
                    }
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
                dataIdx -= 0x40;    // wenn die Zeit enthalten ist, ist der Index um 0x40 größer
                deltaIdx += 2;      // hier ist die Zeit noch zusätzlich angegeben
            }
           
            WS980Sensor sensor = GetSensor(dataIdx);    // sensor finden oder neu anlegen
            if (sensor == null) return -1;              // keine Sensordefinition vorhanden --> Abbruch

            deltaIdx += sensor.ItemDef.Length;
            sensor?.UpdateValue(receiveBytes.Skip(idx).Take(deltaIdx), valueType);
            // todo debugausgabe
            Console.WriteLine("{0}  {1}  {2}  Index des Sensors {2} = {3}  0x{2:X2}",idx-1,1,dataIdx,sensor.ItemDef.Name);
            Console.WriteLine("{0}  {1}  {2}{5}  Wert {3} in {6} {5}    {4}", idx, deltaIdx, sensor.ActualValue, sensor.ItemDef.Name, Tools.ToString(receiveBytes.Skip(idx).Take(deltaIdx).ToArray(), "0x{0:X2} "), sensor.ItemDef.Unit, sensor.ItemDef.Scale);

            idx += deltaIdx;
            return idx;
        }

        private WS980Sensor GetSensor(int dataIdx)
        {
            if (sensorList.ContainsKey(dataIdx)) return sensorList[dataIdx];    // vorhandenen Sensor zurückgeben
            if (!WS980DataItemDef.dataItemList.ContainsKey(dataIdx))            // prüfen ob Definition vorhanden
            {
                Tools.WriteLine("Sensor mit der ID " + dataIdx + " nicht definiert");
                return null;
            }
            var itemDef = WS980DataItemDef.dataItemList[dataIdx];               // definition abrufen
            var sensor = new WS980Sensor(itemDef);                              // sensor neu anlegen
            sensorList.Add(dataIdx, sensor);                                    // und in Liste aufnehmen
            return sensor;
        }

        internal string GetAllActualValues()
        {
            string erg = DateTime.Now.ToString()+";";
            foreach (var sensor in sensorList.Values)
            {
                erg += sensor.ActualValue.ToString() + ";";
            }
            return erg;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="adr">0 .. 65535</param>
        /// <param name="size">0 .. 246</param>
        /// <returns></returns>
        public byte[] ReadEprom(ushort adr, byte size)
        {
            if (size > 0xf6)    // maximum empirisch bestimmt
            {
                Tools.WriteLine("Size of " + size.ToString() + " is too large in ReadEprom. max is 246");
                return null;
            }
            var bef = GetReadEpromArrayBef(adr, size);
            var answer = getAnswer(bef);
            if (answer == null) return null;
            var data = answer.Skip(9).Take(size).ToArray();
            Tools.WriteLine("[{0:X4}..{1:X4}]: {2}", adr, adr+size-1, Tools.ToString(data));
            return data;
        }

        public bool WriteEprom(ushort adr, byte[] data)
        {
            if (data.Length > 19)    // maximum empirisch bestimmt
            {
                Tools.WriteLine("Size of data " + data.Length.ToString() + " is too large in WriteEprom. max is 19");
                var erg1 = WriteEprom(adr, data.Take(19).ToArray());
                var erg2 = WriteEprom((ushort)(adr + 19), data.Skip(19).ToArray());
                return erg1 & erg2;
            }
            var bef = GetWriteEpromArrayBef(adr, data);
            var answer = getAnswer(bef);

            // todo debugcode
            string dbgBef = Tools.ToString(bef, "0x{0:X2} ");
            string dbgAnt = Tools.ToString(answer, "0x{0:X2} ");
            Console.WriteLine("{0} --> \n{1}\n",dbgBef,dbgAnt);

            if (answer == null) return false;
            return true;
        }

        internal bool ClearMaxMinDay()
        {
            byte[] bef = GetBefArrayBef(0x09);
            var answer = getAnswer(bef);
            return answer.SequenceEqual(bef);   // wenn alles ok wird der Befehl zurückgesendet
        }

        internal bool SetTime(DateTime time)
        {
            byte[] bef = GetSetTimeArrayBef(time);
            var answer = getAnswer(bef);
            return answer.SequenceEqual(bef);   // wenn alles ok wird der Befehl zurückgesendet
        }



        #region Befehlsarrays erzeugen
        // Befcode 1
        private static byte[] GetSetTimeArrayBef(DateTime time)
        {
            //                               LenHi LenLo Bef     y     m     d     h     m    s   1/125   crc1  crc2
            byte[] arr = { 0xff, 0xff, 0x0b, 0x00, 0x0d, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,  0x00 };
            //arr[4] = (byte)(arr.Length - 2);
            arr[6] = (byte)(time.Year-2000);
            arr[7] = (byte)(time.Month);
            arr[8] = (byte)(time.Day);
            arr[9] = (byte)(time.Hour);
            arr[10] = (byte)(time.Minute);
            arr[11] = (byte)(time.Second);
            arr[12] = (byte)(0);
            arr[arr.Length - 2] = Tools.calcChecksum(arr.Skip(5).Take(arr.Length - 7));
            arr[arr.Length - 1] = Tools.calcChecksum(arr.Skip(2).Take(arr.Length - 3));
            return arr;
        }

        // Befcode 2
        private static byte[] GetReadEpromArrayBef(ushort adr, byte size)
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

        // Befcode 3
        private static byte[] GetWriteEpromArrayBef(ushort adr, byte[] dataArr)
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

        // Befcode 4..9, 11,12
        /// <summary>
        /// Erzeugt Befehlsarray ohne Parameter
        /// </summary>
        /// <param name="bef">4 bis 9</param>
        /// <returns></returns>
        private static byte[] GetBefArrayBef(byte bef)
        {
            //                               LenHi LenLo Bef  crc1  crc2
            byte[] arr = { 0xff, 0xff, 0x0b, 0x00, 0x06, 0x00, 0x00, 0x00 };
            //arr[4] = (byte)(arr.Length-2);
            arr[5] = bef;
            arr[arr.Length - 2] = Tools.calcChecksum(arr.Skip(5).Take(arr.Length - 7));
            arr[arr.Length - 1] = Tools.calcChecksum(arr.Skip(2).Take(arr.Length - 3));
            return arr;
        }

        // Befcode 10        
        private static byte[] GetParamChangedArrayBef(short param)
        {
            //                               LenHi LenLo Bef                crc1  crc2
            byte[] arr = { 0xff, 0xff, 0x0b, 0x00, 0x08, 0x0A, 0x00, 0x00, 0x00, 0x00 };
            arr[6] = (byte)(param & 0xFF);
            arr[7] = (byte)(param >> 8);
            arr[arr.Length - 2] = Tools.calcChecksum(arr.Skip(5).Take(arr.Length - 7));
            arr[arr.Length - 1] = Tools.calcChecksum(arr.Skip(2).Take(arr.Length - 3));
            return arr;
        }

        #endregion

        #region CompareEprom
        const int len = 0x259;
        byte[] oldEpromData = new byte[len];
        public string CompareEpromStart()
        {
            StringBuilder sb = new StringBuilder();
            byte[] newEpromData = GetEpromStart();
            if (newEpromData == null) return "no Data received";
            for (int i = 0; i < len; i++)
            {
                if (newEpromData[i] != oldEpromData[i])
                {
                    string txt = String.Format("changed:[{0:X4}]  {1:X2}->{2:X2}  {3:B2}->{4:B2}  ", i, oldEpromData[i], newEpromData[i],
                                                                                  Convert.ToString(oldEpromData[i],2), Convert.ToString(newEpromData[i], 2));
                    Tools.WriteLine(txt);
                    sb.AppendLine(txt);
                }
            }

            oldEpromData = newEpromData;
            Tools.WriteLine("ok");
            sb.AppendLine();
            return sb.ToString();
        }

        public byte[] GetEpromStart()
        {
            byte[] epromData = new byte[len];
            byte maxLen = 0x20;
            for (ushort i = 0; i < len; i += maxLen)
            {
                var arr = ReadEprom(i, maxLen); //todo auf null prüfen
                if (arr == null) return null;
                for (int j = 0; j < maxLen; j++)
                {
                    if (i+j<len)  epromData[i + j] = arr[j];
                }
            };
            return epromData;
        } 
        #endregion
    }
}
