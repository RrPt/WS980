using System;

namespace WS980
{
    internal class WS980Parameter
    {
        private WS980 ws980;
        private byte [] rawData ;

        public WS980Parameter(WS980 wS980)
        {
            this.ws980 = wS980;
            ReadParameter();
        }

        public void ReadParameter()
        {
            //ReadDataDefinitions();
            //ReadRainIndex();
            //ReadAlarmSettings();
            //ReadTotalMaxMin()
            //ReadBarometricDataLast24h();

            rawData = ws980.ReadEprom(0,0x34 + 1);
            rawData = ws980.ReadEprom(0x40, 0x6b - 0x40 + 1);
            rawData = ws980.ReadEprom(0x100, 0x12A - 0x100 + 1);
            rawData = ws980.ReadEprom(0x130, 0x160 - 0x130 + 1);
            rawData = ws980.ReadEprom(0x170, 0x1F4 - 0x170 + 1);
            rawData = ws980.ReadEprom(0x200, 0x22f - 0x200 + 1);

        }

        public string GetRawData()
        {
            return Tools.ToString(rawData);
        }
    }
}