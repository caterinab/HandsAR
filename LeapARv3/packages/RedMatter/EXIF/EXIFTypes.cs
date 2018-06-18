using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedMatter.EXIF
{
    /// <summary>
    /// Methods for convertion of exif types.
    /// </summary>
    public class EXIFTypes
    {

        #region Convertion from EXIF types.

        public static double GetRational(byte[] value)
        {
            long numerator = BitConverter.ToUInt32(new byte[4] { value[0], value[2], value[2], value[3] }, 0);
            long denominator = BitConverter.ToUInt32(new byte[4] { value[4], value[5], value[6], value[7] }, 0);
            return (double)numerator / (double)denominator;
        }

        public static string GetRationalString(byte[] value)
        {
            long numerator = BitConverter.ToUInt32(new byte[4] { value[0], value[2], value[2], value[3] }, 0);
            long denominator = BitConverter.ToUInt32(new byte[4] { value[4], value[5], value[6], value[7] }, 0);
            return numerator.ToString() + "/" + denominator.ToString();

        }

        public static byte GetByte(byte[] value)
        {
            return value[0];
        }

        public static ushort GetShort(byte[] value)
        {
            return BitConverter.ToUInt16(value, 0); 
        }

        public static string GetASCII(byte[] value)
        {
            return Encoding.ASCII.GetString(value, 0, value.Length - 1);
        }

        public static long GetLong(byte[] value)
        {
            return BitConverter.ToUInt32(value, 0);
        }

        public static string GetUndefined(byte[] value)
        {
            return GetASCII(value);
        }

        public static long GetSLong(byte[] value)
        {
            return BitConverter.ToInt32(value, 0);
        }

        public static double GetSRational(byte[] value)
        {
            long numerator = BitConverter.ToInt32(new byte[4] { value[0], value[2], value[2], value[3] }, 0);
            long denominator = BitConverter.ToInt32(new byte[4] { value[4], value[5], value[6], value[7] }, 0);
            return (double)numerator / (double)denominator;
        }

        #endregion

        #region Convertion to EXIF types.

        public static byte[] ToByte(string s)
        {
            return new byte[1] { byte.Parse(s) };
        }

        public static byte[] ToByte(byte b)
        {
            return new byte[1] { b };
        }

        public static byte[] ToASCII(string s)
        {
            return Encoding.ASCII.GetBytes(s);
        }

        public static byte[] ToLong(string s)
        {
            return BitConverter.GetBytes(ulong.Parse(s));
        }

        public static byte[] ToLong(ulong l)
        {
            return BitConverter.GetBytes(l);
        }

        public static byte[] ToRational(double d)
        {
            ulong numerator = (ulong)(d * 10000);
            ulong denominator = 10000;
            byte[] bytes = new byte[8];
            byte[] b1 = BitConverter.GetBytes(numerator);
            byte[] b2 = BitConverter.GetBytes(denominator);
            bytes[0] = b1[0];
            bytes[1] = b1[1];
            bytes[2] = b1[2];
            bytes[3] = b1[3];
            bytes[4] = b2[0];
            bytes[5] = b2[1];
            bytes[6] = b2[2];
            bytes[7] = b2[3];
            return bytes;
        }

        public static byte[] ToRational(string s)
        {
            return ToRational(double.Parse(s));
        }

        public static byte[] ToSLong(string s)
        {
            return BitConverter.GetBytes(long.Parse(s));
        }

        public static byte[] ToSLong(long l)
        {
            return BitConverter.GetBytes(l);
        }

        public static byte[] ToSRational(double d)
        {
            long numerator = (long)(d * 10000);
            long denominator = 10000;
            byte[] bytes = new byte[8];
            byte[] b1 = BitConverter.GetBytes(numerator);
            byte[] b2 = BitConverter.GetBytes(denominator);
            bytes[0] = b1[0];
            bytes[1] = b1[1];
            bytes[2] = b1[2];
            bytes[3] = b1[3];
            bytes[4] = b2[0];
            bytes[5] = b2[1];
            bytes[6] = b2[2];
            bytes[7] = b2[3];
            return bytes;
        }

        public static byte[] ToSRational(string s)
        {
            return ToRational(double.Parse(s));
        }

        public static byte[] ToShort(short s)
        {
            return BitConverter.GetBytes(s);
        }

        public static byte[] ToShort(string s)
        {
            return ToShort(short.Parse(s));
        }

        #endregion
    }
}
