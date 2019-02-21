using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WS980
{
    class Tools
    {
        public static string ToString(byte[] arr,string fmt = "{0:X2} ")
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < arr.Length; i++)  sb.AppendFormat(fmt, arr[i]);
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

        internal static byte LoNibble(byte v)
        {
            return (byte)(v & 0x0F);
        }

        internal static byte HiNibble(byte v)
        {
            return (byte)(v >>4);
        }


        /// <summary>
        /// Prüft, ob ein angegebenes Bit im Byte gesetzt ist.
        /// </summary>
        /// <param name="b">Byte, welches getestet werden soll.</param>
        /// <param name="BitNumber">Das zu prüfende Bit (0 bis 7).</param>
        /// <returns>gesetzt=true, nicht gesetzt=false</returns>
        public static bool CheckBitSet(byte b, int BitNumber)
        {
            //Kleine Fehlerbehandlung
            if (BitNumber < 8 && BitNumber > -1)
            {
                return (b & (1 << BitNumber)) > 0;
            }
            else
            {
                throw new InvalidOperationException(
                "Der Wert für BitNumber " + BitNumber.ToString() + " war nicht im zulässigen Bereich! (BitNumber = (min)0 - (max)7)");
            }

        }

    }
}
