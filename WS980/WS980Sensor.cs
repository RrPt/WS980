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
                    break;
                case ValueType.dayMax:
                    dayMaxValue = val;
                    break;
                default:
                    break;
            }
        }

        public override string ToString()
        {
            if (itemDef.Index==23) return String.Format("{0,-12}={1} (Idx={4})  <{2};{3}><{5};{6}>", name, ActualValueStr, MinValueStr, MaxValueStr, GetUvIdx(actualValue), GetUvIdx(minValue), GetUvIdx(maxValue));
            return String.Format("{0,-12}={1}  <{2};{3}>   Day:<{4};{5}>", name, ActualValueStr, MinValueStr, MaxValueStr,DayMinValueStr,DayMaxValueStr);
        }

        private string str(double? val)
        {
            if (!val.HasValue) return "";
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
