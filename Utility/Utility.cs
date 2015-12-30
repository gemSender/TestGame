using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace TaskTest.Utility
{
    class Utility
    {
        public static T ByteToStruct<T>(byte[] buffer, int offset) where T : struct
        {
            return (T)ByteToStruct(typeof(T), buffer, offset);
        }
        public static object ByteToStruct(Type type, byte[] buffer, int offset)
        {
            int size = 0;
            Type targetType;
            if (type.IsEnum)
            {
                size = sizeof(int);
                targetType = typeof(int);
            }
            else
            {
                size = Marshal.SizeOf(type);
                targetType = type;
            }
            if (size > buffer.Length - offset)
            {
                return null;
            }
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            Marshal.Copy(buffer, offset, structPtr, size);
            object obj = Marshal.PtrToStructure(structPtr, targetType);
            Marshal.FreeHGlobal(structPtr);
            return obj;
        }

        public static byte[] StructToBytes(ValueType structObj)
        {
            int size = Marshal.SizeOf(structObj.GetType());
            byte[] bytes = new byte[size];
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(structObj, structPtr, false);
            Marshal.Copy(structPtr, bytes, 0, size);
            Marshal.FreeHGlobal(structPtr);
            return bytes;
        }

        public static int WriteStructToBuffer(ValueType structObj, byte[] buffer, int startIndex)
        {
            var type = structObj.GetType();
            int size = 0;
            ValueType finalObj;
            if(type.IsEnum)
            {
                size = sizeof(int);
                finalObj = (int)structObj;
            }
            else {
                size = Marshal.SizeOf(type);
                finalObj = structObj;
            }
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(finalObj, structPtr, false);
            Marshal.Copy(structPtr, buffer, startIndex, size);
            Marshal.FreeHGlobal(structPtr);
            return size;
        }

        public static int WriteStringToBuffer(string src, byte[] buffer, int startIndex)
        {
            var bytes = Encoding.UTF8.GetBytes(src);
            Buffer.BlockCopy(bytes, 0, buffer, startIndex, bytes.Length);
            return bytes.Length;
        }
        public static byte[] GetDataBuffer(int len, Func<int, byte> getMethod, int startIndex = 0)
        {
            byte[] ret = new byte[len];
            for (int i = startIndex; i < len + startIndex; i++)
            {
                ret[i - startIndex] = getMethod(i);
            }
            return ret;
        }
    }
}
