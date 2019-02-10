using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WS980
{
    class WS980Sensor
    {
        private double? actualValue = null;
        private double? maxValue = null;
        private double? minValue = null;
        private double? dayMaxValue = null;
        private double? dayMinValue = null;
        private DateTime? dayMaxTime = null;
        private DateTime? dayMinTime = null;
        private string name = "?";
        private string unit = "";
        private DateTime time = new DateTime(2019, 1, 1);
        private WS980DataItemDef itemDef;
        private double scale = 1.0;

        public WS980Sensor(WS980DataItemDef itemDef)
        {
            this.itemDef = itemDef;
            name = itemDef.Name;
            unit = itemDef.Unit;
            scale = itemDef.Scale;
        }

        public double? ActualValue { get => actualValue; set => actualValue = value; }
        public string ActualValueStr { get => str(actualValue); }
        public double? MaxValue { get => maxValue; set => maxValue = value; }
        public string MaxValueStr { get => str(maxValue); }
        public double? MinValue { get => minValue; set => minValue = value; }
        public string MinValueStr { get => str(minValue); }
        public double? DayMaxValue { get => dayMaxValue; set => dayMaxValue = value; }
        public string DayMaxValueStr { get => str(dayMaxValue); }
        public double? DayMinValue { get => dayMinValue; set => dayMinValue = value; }
        public string DayMinValueStr { get => str(dayMinValue); }
        public string Name { get => name; set => name = value; }
        public string Unit { get => unit; set => unit = value; }
        public DateTime Time { get => time; set => time = value; }
        public double Scale { get => scale; set => scale = value; }
        internal WS980DataItemDef ItemDef { get => itemDef; set => itemDef = value; }
        public DateTime? DayMinTime { get => dayMinTime; set => dayMinTime = value; }
        public DateTime? DayMaxTime { get => dayMaxTime; set => dayMaxTime = value; }

        /// <summary>
        /// Gets the Sensordates out of the Byte
        /// </summary>
        /// <param name="dataBytes"></param>
        /// <param name="valueType"></param>
        internal void UpdateValue(IEnumerable<byte> dataBytes, ValueType valueType)
        {
            double val = 0.0;
            if (itemDef.Length == 1)
            {
                val = dataBytes.ElementAt(0) * itemDef.Scale;
            }
            else if (itemDef.Length == 2)
            {
                val = ((short)(256 * (short)dataBytes.ElementAt(0) + (short)dataBytes.ElementAt(1))) * itemDef.Scale;
            }

            else if (itemDef.Length == 4)
            {
                val = 0;
                for (int i = 0; i < 4; i++)
                {
                    val = val * 256 + dataBytes.ElementAt(i);
                }
                val = val * itemDef.Scale;
            }

            switch (valueType)
            {
                case ValueType.actual:
                    actualValue = val;
                    break;
                case ValueType.min:
                    minValue = val;
                    break;
                case ValueType.max:
                    maxValue = val;
                    break;
                case ValueType.dayMin:
                    dayMinValue = val;
                    dayMinTime = GetTime(dataBytes.Skip(dataBytes.Count()-2));
                    break;
                case ValueType.dayMax:
                    dayMaxValue = val;
                    dayMaxTime = GetTime(dataBytes.Skip(dataBytes.Count() - 2));
                    break;
                default:
                    break;
            }
        }

        private DateTime? GetTime(IEnumerable<byte> timeBytes)
        {
            DateTime time = DateTime.Now.Date;
            time = time.AddHours(timeBytes.ElementAt(0)).AddMinutes(timeBytes.ElementAt(1));
            return time;
        }

        public override string ToString()
        {
            string erg = String.Format("{0,-13}={1}", name, ActualValueStr).PadRight(25);
            erg += String.Format("<{0};{1}>", MinValueStr, MaxValueStr).PadRight(25);
            erg += String.Format("Day:<{0}({2:HH:mm});{1}({3:HH:mm})>", DayMinValueStr, DayMaxValueStr, DayMinTime, dayMaxTime);
            if (itemDef.Index == 23) erg += String.Format("   Idx={0}\t<{1} ; {2}>", GetUvIdx(actualValue), GetUvIdx(minValue), GetUvIdx(maxValue));
            return erg;
        }

        private string str(double? val)
        {
            if (!val.HasValue) return "   ";
            int noOfDigAfterDecimalPoint = (int)(0.5 - Math.Log10(scale));
            if (noOfDigAfterDecimalPoint < 0) noOfDigAfterDecimalPoint = 0;
            if (noOfDigAfterDecimalPoint > 6) noOfDigAfterDecimalPoint = 6;
            string fmt = "{0:0.".PadRight(5 + noOfDigAfterDecimalPoint, '0') + "}" + unit;
            return String.Format(fmt, val); ;
        }


        byte GetUvIdx(double? value)
        {
            if (!value.HasValue) return 0;
            int[] UvIdxBorders = new int[] { 0, 99, 540, 1000, 1400, 1843, 2292, 2734, 3138, 3648, 4196, 4707, 5209, 5735, 6276, 6778 };
            for (byte i = 0; i < UvIdxBorders.Length; i++)
            {
                if (value < UvIdxBorders[i])  return (byte)(i - 1);
            }
            return 15;
        }
    }
}
