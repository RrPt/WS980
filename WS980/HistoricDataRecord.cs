using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WS980
{
    class HistoricDataRecord
    {
        private DateTime recTime;
        private byte[] byteRecord;
        public DateTime RecTime { get => recTime; set => recTime = value; }
        public short WindDir { get; set; }
        public float Wind { get; set; }
        public float Gust { get; set; }
        public float RainTotal { get; set; }
        public short  HumidityIn { get; set; }
        public short HumidityOut { get; set; }
        public float TempIn { get; set; }
        public float TempOut { get; set; }
        public float Pressure { get; set; }
        public float Light { get; set; }
        public short Uv { get; set; }

        public HistoricDataRecord(DateTime recTime, byte[] byteRecord)
        {
            this.RecTime = recTime; // UTC
            this.byteRecord = byteRecord;
            // todo 0xff.. Werte als ungültig bearbeiten
            // hier noch den historischen record auftröseln
            WindDir = (short)(byteRecord[1] + (Tools.CheckBitSet(byteRecord[0],0)?256:0));
            Wind = (byteRecord[2] + (Tools.CheckBitSet(byteRecord[0], 1) ? 256 : 0))/10f;
            Gust = (byteRecord[3] + (Tools.CheckBitSet(byteRecord[0], 2) ? 256 : 0))/10f;
            RainTotal = (byteRecord[4] + 256 * byteRecord[5] + (Tools.CheckBitSet(byteRecord[0], 3) ? 256*256 : 0)) /10.0f;
            if(Tools.CheckBitSet(byteRecord[0], 3))
            {
                // todo Rain overflow
            }
            HumidityIn = byteRecord[6];
            HumidityOut = byteRecord[7];
            TempIn  = (byteRecord[8]  + Tools.LoNibble(byteRecord[9]) * 256) / 10.0f-40f;
            TempOut = (byteRecord[10] + Tools.HiNibble(byteRecord[9]) * 256) / 10.0f-40f;
            Pressure = (byteRecord[11] + 256 * byteRecord[12])/10f;
            Light = (byteRecord[13] + 256 * byteRecord[14] + 256 * 256 * byteRecord[15])/10.0f;
            Uv = (short)(byteRecord[16] + 256 * byteRecord[17]);


        }

        public override string ToString()
        {
            string erg = String.Format("{0}: {1}° {2}m/s {3}m/s {4}mm {5}%i {6}%o {7}°Ci {8}°Co {9}hpa {10}lux {11}uw/m2", 
                RecTime,WindDir,Wind,Gust,RainTotal,HumidityIn,HumidityOut,TempIn,TempOut,Pressure,Light,Uv);
            return erg;
        }
    }
}
