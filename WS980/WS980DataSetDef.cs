using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WS980
{
    class WS980DataSetDef
    {
        public SortedList<int, WS980DataItemDef> DataItemList = new SortedList<int, WS980DataItemDef>();
    }

    internal class WS980DataItemDef
    {
        private int length;
        private string name;
        private string unit;

        public WS980DataItemDef(int length, string name, string unit)
        {
            this.length = length;
            this.name = name;
            this.unit = unit;
        }

        public int Length { get => length; set => length = value; }
        public string Name { get => name; set => name = value; }
        public string Unit { get => unit; set => unit = value; }
    }
}
