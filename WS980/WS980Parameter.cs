using System;

namespace WS980
{
    internal class WS980Parameter
    {
        private WS980 ws980;
        private byte[] rawDataDefinitions;
        private bool UsTimeDisplay;

        public int Model { get; private set; }
        public byte Version { get; private set; }
        public int ID { get; private set; }
        public string TemperatureUnit
        {
            get => _temperatureUnit;
            set  {
                _temperatureUnit = value;
                if (value == "F") Tools.SetBit(ref rawDataDefinitions[0x10], 1,true);
                else Tools.SetBit(ref rawDataDefinitions[0x10], 1,false); 

            }

        }
        public string PressureUnit { get; private set; }
        public string LightUnit { get; private set; }
        public string WindUnit { get; private set; }
        public string RainUnit { get; private set; }
        public string EarthPart { get; private set; }
        public RainDisplayTyp RainDisplay { get; private set; }
        public PreasureDisplayTyp PreasureDisplay { get; private set; }
        public int PreasureGraphDurationDisplay { get; private set; }
        public WindDisplayTyp WindDisplay { get; private set; }
        public TemperatureDisplayTyp TempDisplay { get; private set; }
        public string TimeFormatDisplay { get; private set; }
        public bool DstDisplay { get; private set; }
        public bool KeyBeep
        {
            get => _keyBeep;
            set
            {
                _keyBeep = value;
                Tools.SetBit(ref rawDataDefinitions[0x14], 7, value);
            }
        }





        private AlarmEnableState alarmEnableState;
        private bool _keyBeep;
        private string _temperatureUnit;

        public int RainSeasonBegin { get; private set; }
        public int HistoryStoreInterval_s { get; private set; }
        public byte TimeZone { get; private set; }
        public float InTempOffset { get; private set; }
        public byte InHumidityOffset { get; private set; }
        public float OutTempOffset { get; private set; }
        public byte OutHumidityOffset { get; private set; }
        public float AbsPressureOffset { get; private set; }
        public float RelPressureOffset { get; private set; }
        public short WindDirectionOffset { get; private set; }
        public float WindFactor { get; private set; }
        public float RainfallFactor { get; private set; }

        public WS980Parameter(WS980 wS980)
        {
            this.ws980 = wS980;
            ReadParameter();
        }

        public void ReadParameter()
        {
            ReadDataDefinitions();
            // todo  folgende Funktionen noch implementieren
            //ReadRainIndex();
            //ReadAlarmSettings();
            //ReadTotalMaxMin()
            //ReadBarometricDataLast24h();

            //rawData = ws980.ReadEprom(0x40, 0x6b - 0x40 + 1);
            //rawData = ws980.ReadEprom(0x100, 0x12A - 0x100 + 1);
            //rawData = ws980.ReadEprom(0x130, 0x160 - 0x130 + 1);
            //rawData = ws980.ReadEprom(0x170, 0x1F4 - 0x170 + 1);
            //rawData = ws980.ReadEprom(0x200, 0x22f - 0x200 + 1);

        }

        private bool ReadDataDefinitions()
        {
            rawDataDefinitions = ws980.ReadEprom(0, 0x35);
            var rdd = rawDataDefinitions;
            if (rdd[0] != 0x55 & rdd[1] != 0xAA) { Tools.WriteLine("EPROM nicht initialisiert"); return false; }
            Model = rdd[2] + rdd[3] * 254;
            Version = rdd[4];
            ID = rdd[5] + rdd[6] * 256 + rdd[7] * 65536 + rdd[8] * 256 * 65536;
            if (Tools.CheckBitSet(rdd[0x10], 1)) TemperatureUnit = "F";
            else TemperatureUnit = "°C";
            if (Tools.CheckBitSet(rdd[0x10], 2)) LightUnit = "fc";
            if (Tools.CheckBitSet(rdd[0x10], 3)) LightUnit = "lux";
            if (Tools.CheckBitSet(rdd[0x10], 4)) LightUnit = "w/m2";
            if (Tools.CheckBitSet(rdd[0x10], 5)) PressureUnit = "hpa";
            if (Tools.CheckBitSet(rdd[0x10], 6)) PressureUnit = "inHg";
            if (Tools.CheckBitSet(rdd[0x10], 7)) PressureUnit = "mmHg";

            if (Tools.CheckBitSet(rdd[0x11], 0)) WindUnit = "km/h";
            if (Tools.CheckBitSet(rdd[0x11], 1)) WindUnit = "mp";
            if (Tools.CheckBitSet(rdd[0x11], 2)) WindUnit = "knots";
            if (Tools.CheckBitSet(rdd[0x11], 3)) WindUnit = "m/s";
            if (Tools.CheckBitSet(rdd[0x11], 4)) WindUnit = "bft";
            if (Tools.CheckBitSet(rdd[0x11], 5)) RainUnit = "inch";
            else RainUnit = "mm";
            if (Tools.CheckBitSet(rdd[0x11], 6)) EarthPart = "south";
            else EarthPart = "north";

            if (Tools.CheckBitSet(rdd[0x12], 0)) RainDisplay = RainDisplayTyp.rate;
            if (Tools.CheckBitSet(rdd[0x12], 1)) RainDisplay = RainDisplayTyp.rainEvent;
            if (Tools.CheckBitSet(rdd[0x12], 2)) RainDisplay = RainDisplayTyp.day;
            if (Tools.CheckBitSet(rdd[0x12], 3)) RainDisplay = RainDisplayTyp.week;
            if (Tools.CheckBitSet(rdd[0x12], 4)) RainDisplay = RainDisplayTyp.month;
            if (Tools.CheckBitSet(rdd[0x12], 5)) RainDisplay = RainDisplayTyp.year;
            if (Tools.CheckBitSet(rdd[0x12], 6)) RainDisplay = RainDisplayTyp.total;

            if (Tools.CheckBitSet(rdd[0x13], 0)) PreasureDisplay = PreasureDisplayTyp.abs;
            else PreasureDisplay = PreasureDisplayTyp.rel;
            if (Tools.CheckBitSet(rdd[0x13], 1)) PreasureGraphDurationDisplay = 24;
            else PreasureGraphDurationDisplay = 12;
            if (Tools.CheckBitSet(rdd[0x13], 2)) WindDisplay = WindDisplayTyp.wind;
            if (Tools.CheckBitSet(rdd[0x13], 3)) WindDisplay = WindDisplayTyp.gust;
            if (Tools.CheckBitSet(rdd[0x13], 4)) WindDisplay = WindDisplayTyp.windDir;
            if (Tools.CheckBitSet(rdd[0x13], 5)) TempDisplay = TemperatureDisplayTyp.windChill;
            if (Tools.CheckBitSet(rdd[0x13], 6)) TempDisplay = TemperatureDisplayTyp.drewPoint;
            if (Tools.CheckBitSet(rdd[0x13], 7)) TempDisplay = TemperatureDisplayTyp.heatIndex;

            UsTimeDisplay = Tools.CheckBitSet(rdd[0x14], 0);
            if (Tools.CheckBitSet(rdd[0x14], 1)) TimeFormatDisplay = "YYYY/MM/DD";
            if (Tools.CheckBitSet(rdd[0x14], 2)) TimeFormatDisplay = "MM/DD/YYYY";
            if (Tools.CheckBitSet(rdd[0x14], 3)) TimeFormatDisplay = "DD/MM/YYYY";
            DstDisplay = Tools.CheckBitSet(rdd[0x14], 4);       // Sommerzeit

            KeyBeep = Tools.CheckBitSet(rdd[0x14], 7);

            alarmEnableState = (AlarmEnableState)(rdd[0x15] + rdd[0x16] * 256 + rdd[0x17] * 65536);

            RainSeasonBegin = rdd[0x18];

            HistoryStoreInterval_s = rdd[0x1A] == 0x01 ? rdd[0x18] : rdd[0x18] * 60;

            TimeZone = rdd[0x1C];

            InTempOffset = ((short)(rdd[0x23] + rdd[0x24] * 256)) / 10f;
            InHumidityOffset = rdd[0x25];

            OutTempOffset = ((short)(rdd[0x26] + rdd[0x27] * 256)) / 10f;
            OutHumidityOffset = rdd[0x28];

            AbsPressureOffset = ((short)(rdd[0x29] + rdd[0x2A] * 256)) / 10f;
            RelPressureOffset = ((short)(rdd[0x2B] + rdd[0x2C] * 256)) / 10f;

            WindDirectionOffset = (short)(rdd[0x2D] + rdd[0x2E] * 256);
            WindFactor = rdd[0x2F] / 100f;
            RainfallFactor = rdd[0x30] / 100f;




            return true;
        }

        // todo:  DataDefinitionsToString fehlt noch

        // todo:  SendDataDefinitions to WS980
        public bool WriteParameter()
        {
            WriteDataDefinitions();
            // todo  folgende Funktionen noch implementieren
            //WriteRainIndex();
            //WriteAlarmSettings();
            //WriteTotalMaxMin()
            //WriteBarometricDataLast24h();

            // todo einzelnen ergebnisse zusammenfassen und zurückgeben
            return true;
        }

        private void WriteDataDefinitions()
        {
            ws980.WriteEprom(0, rawDataDefinitions);
        }

        public string GetRawData()
        {
            return Tools.ToString(rawDataDefinitions);
        }

        public override string ToString()
        {
            // todo  zu implementieren
            return "Beep=" + KeyBeep.ToString();
        }
    }

    enum RainDisplayTyp
    {
        rate,rainEvent,day,week,month,year,total
    }

    enum PreasureDisplayTyp
    {
        rel,  abs
    }
}
enum WindDisplayTyp
{
    wind,
    gust,
    windDir
}

enum TemperatureDisplayTyp
{
    windChill,
    drewPoint,
    heatIndex
}

[Flags]
enum AlarmEnableState
{
    light_high_alarm    = 0x0001,
    UVI_high_alarm      = 0x0002,
    in_temp_high_alarm  = 0x0004,
    in_temp_low_alarm   = 0x0008,
    humidity_high_alarm = 0x0010,
    humidity_low_alarm  = 0x0020,
    out_temp_high_alarm = 0x0040,
    out_temp_low_alarm  = 0x0080,
    out_humidity_high_alarm    = 0x0100 ,
    out_humidity_low_alarm     = 0x0200 ,
    ABS_barometric_high_alarm  = 0x0400 ,
    ABS_barometric_low_alarm   = 0x0800 ,
    REL_barometric_high_alarm  = 0x1000 ,
    REL_barometric_low_alarm   = 0x2000 ,
    wind_high_alarm            = 0x4000 ,
    gust_wind_high_alarm       = 0x8000 ,
    wind_chill_low_alarm       = 0x10000 ,
    dew_point_high_alarm       = 0x20000 ,
    dew_point_low_alarm        = 0x40000 ,
    heat_index_high_alarm      = 0x80000 ,
    rain_rate_high_alarm       = 0x100000 ,
    rain_day_high_alarm        = 0x200000 ,
    storm_alarm                = 0x400000 ,
    flash_flood_alarm          = 0x800000

}