using System;
using System.Collections.Generic;
using System.Diagnostics;
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


        public static void WriteLine(string format, params object[] args)
        {
            string txt = String.Format(format, args);
            Trace.WriteLine(txt); 
        }

        public static void WriteLine()
        {
            Trace.WriteLine("");
        }
        public static void WriteLine(string txt)
        {
            Trace.WriteLine(txt);
        }
    }
}
