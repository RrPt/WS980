using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WS980
{
    class WS980Sensor
    {
        // List of all Sensors
        public static SortedList<int, WS980Sensor> dataItemList = new SortedList<int, WS980Sensor>();

        const double noValue = -999.9;
        private double actualValue = noValue;
        private double maxValue = noValue;
        private double minValue = noValue;
        private string name = "?";
        private string unit = "";
        private DateTime time = new DateTime(2019,1,1);
        private WS980DataItemDef itemDef;
        private double scale=1.0;

        public WS980Sensor(WS980DataItemDef itemDef)
        {
            this.itemDef = itemDef;
            name = itemDef.Name;
            unit = itemDef.Unit;
            scale = itemDef.Scale;
            dataItemList.Add(itemDef.Index, this);
        }

        public double ActualValue { get => actualValue; set => actualValue = value; }
        public string ActualValueStr { get => str(actualValue); }
        public double MaxValue { get => maxValue; set => maxValue = value; }
        public string MaxValueStr { get => str(maxValue);  }
        public double MinValue { get => minValue; set => minValue = value; }
        public string MinValueStr { get => str(minValue); }
        public string Name { get => name; set => name = value; }
        public string Unit { get => unit; set => unit = value; }
        public DateTime Time { get => time; set => time = value; }
        public double Scale { get => scale; set => scale = value; }
        internal WS980DataItemDef ItemDef { get => itemDef; set => itemDef = value; }

        /// <summary>
        /// Gets the Sensor for a specific itemDef
        /// </summary>
        /// <param name="itemDef"></param>
        /// <returns></returns>
        internal static WS980Sensor GetSensor(WS980DataItemDef itemDef)
        {
            int idx = itemDef.Index;
            if (dataItemList.ContainsKey(idx)) return dataItemList[idx];    // existiert
            // neu anlegen
            return new WS980Sensor(itemDef);
        }

        /// <summary>
        /// Gets the Sensordate out of the Bytestream
        /// </summary>
        /// <param name="idx"></param> actual position in Bytestream
        /// <param name="receiveBytes"></param> Bytestream
        /// <param name="valueType"></param>
        internal void GetValue(ref int idx, byte[] receiveBytes, ValueType valueType)
        {
            idx++;
            double val = 0.0;
            if (itemDef.Length == 1)
            {
                val = receiveBytes[idx] * itemDef.Scale;
            }
            else if (itemDef.Length == 2)
            {
                val = (256 * (int)receiveBytes[idx] + receiveBytes[idx + 1]) * itemDef.Scale;
            }

            else if (itemDef.Length == 4)
            {
                val = 0;
                for (int i = 0; i < 4; i++)
                {
                    val = val * 256 + receiveBytes[idx + i];
                }
                val = val * itemDef.Scale;
            }
            idx += itemDef.Length;
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
                default:
                    break;
            }
        }

        public override string ToString()
        {
            return String.Format("{0,-12}={1}  <{2},{3}>", name, ActualValueStr, MinValueStr, MaxValueStr);
        }

        private string str(double val)
        {
            if (val == noValue) return "";
            int noOfDigAfterDecimalPoint = (int)(0.5 - Math.Log10(scale));
            if (noOfDigAfterDecimalPoint < 0) noOfDigAfterDecimalPoint = 0;
            if (noOfDigAfterDecimalPoint > 6) noOfDigAfterDecimalPoint = 6;
            string fmt =  "{0:0.".PadRight(5 + noOfDigAfterDecimalPoint, '0')+"}"+unit;
            return String.Format(fmt,val); ;
        }
    }
}
