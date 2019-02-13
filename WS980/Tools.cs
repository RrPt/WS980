using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WS980
{
    class Tools
    {
        public static string ToString(byte[] arr)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < arr.Length; i++)  sb.AppendFormat("{0:X2} ", arr[i]);
            return sb.ToString();
        }

        internal static byte calcChecksum(IEnumerable<byte> arr)
        {
            byte sum = 0;
            for (int i = 0; i < arr.Count<byte>(); i++)
            {
                sum += arr.ElementAt(i);
            }
            return sum;
        }

        /// <summary>
        /// Erzeugt Befehlsarray ohne Parameter
        /// </summary>
        /// <param name="bef">4 bis 9</param>
        /// <returns></returns>
        public static byte[] GetBefArray(byte bef)
        {
            //                               LenHi LenLo Bef  crc1  crc2
            byte[] arr = { 0xff, 0xff, 0x0b, 0x00, 0x06, 0x00, 0x00, 0x00 };
            //arr[4] = (byte)(arr.Length-2);
            arr[5] = bef;
            arr[arr.Length - 2] = Tools.calcChecksum(arr.Skip(5).Take(arr.Length - 7));
            arr[arr.Length - 1] = Tools.calcChecksum(arr.Skip(2).Take(arr.Length - 3));
            return arr;
        }

        public static byte[] GetReadEpromArray(ushort adr, byte size)
        {
            //                               LenHi LenLo Bef   ardLo adrHi size  crc1  crc2
            byte[] arr = { 0xff, 0xff, 0x0b, 0x00, 0x09, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00 };
            //arr[4] = (byte)(arr.Length - 2);
            arr[6] = (byte)(adr & 0xFF);
            arr[7] = (byte)(adr>>8);
            arr[8] = size;
            arr[arr.Length - 2] = Tools.calcChecksum(arr.Skip(5).Take(arr.Length - 7));
            arr[arr.Length - 1] = Tools.calcChecksum(arr.Skip(2).Take(arr.Length - 3));
            return arr;
        }
    }
}
