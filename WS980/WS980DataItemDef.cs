using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WS980_NS
{
    [Flags]
    public enum ValueTypes
    {
        Actual = 0x00,
        Min = 0x01,
        Max = 0x02,
        DayMin = 0x04,
        DayMax = 0x08,
        MinMax = Min | Max,
        All = Min | Max | DayMin | DayMax
    }

    public class WS980DataItemDef
    {
        // List of all ItemDefs
        public static SortedList<int, WS980DataItemDef> dataItemList = new SortedList<int, WS980DataItemDef>();
        private int index;
        private int length;
        private string name;
        private string unit;
        private double scale;

        public WS980DataItemDef(int index,int length, string name, string unit, int nachkommastellen = 0, double minAllowedValue = double.MinValue, double maxAllowedValue = double.MaxValue, ValueTypes usedValueTypes = ValueTypes.Actual)
        {
            this.index = index;
            this.length = length;
            this.name = name;
            this.unit = unit;
            this.scale = (float)Math.Pow(10, -nachkommastellen);
            this.MinAllowedValue = minAllowedValue;
            this.MaxAllowedValue = maxAllowedValue;
            UsedValueTypes = usedValueTypes;
            dataItemList.Add(index, this);
        }

        public int Length { get => length; set => length = value; }
        public string Name { get => name; set => name = value; }
        public string Unit { get => unit; set => unit = value; }
        public double Scale { get => scale; set => scale = value; }
        public int Index { get => index; set => index = value; }
        public double MinAllowedValue { get; set; }
        public double MaxAllowedValue { get; set; }

        public ValueTypes UsedValueTypes { get; set; }
    }
}
