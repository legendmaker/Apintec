using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Apintec.Core.APCoreLib
{
    public static class Gadget
    {
        public static Int32 BitArrayToInt32(BitArray bitArray)
        {
            if (bitArray == null)
            {
                return 0;
            }

            if (bitArray.Length > 32)
                throw new ArgumentException("Argument length shall be at most 32 bits.");

            Int32[] array = new Int32[1];
            bitArray.CopyTo(array, 0);
            return array[0];
        }

        public static Int64 BitArrayToInt64(BitArray bitArray)
        {
            Int64 result = 0;
            if (bitArray == null)
            {
                return 0;
            }
            if (bitArray.Length > 64)
                throw new ArgumentException("Argument length shall be at most 64 bits.");

            for (int i = 0; i < bitArray.Length; i++)
            {
                if (bitArray[i])
                    result += Convert.ToInt64(Math.Pow(2, i));
            }
            return result;
        }

        public static BitArray IntToBitArray(object obj, int aLength)
        {
            Int64 value = 0;
            BitArray bitArray = new BitArray(aLength);
            int length = 0;
            try
            {
                value = Convert.ToInt64(obj);
            }
            catch (Exception e)
            {
                throw e;
            }

            while (true)
            {
                if (length >= aLength)
                    break;
                if (value == 0)
                    break;
                if (length != 0)
                    value = value / 2;
                bitArray[length] = ((value & 0x01) == 1) ? true : false;
                length++;
            }
            return bitArray;
        }

        public static T[] ArrayAppend<T>(T[] source, T[] sub)
        {
            T[] c = new T[source.Length + sub.Length];
            source.CopyTo(c, 0);
            sub.CopyTo(c, source.Length);
            return c;
        }

        public static object DeepClone(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj); 
            ms.Position = 0;
            return (bf.Deserialize(ms));
        }
    }
}
