using System.Runtime.InteropServices;

namespace GameServer.Schemas;

public class Schema
{
    public static T BytesToStruct<T>(byte[] arr) where T : struct
    {
        int size = Marshal.SizeOf(typeof(T));
        if (arr.Length < size)
            throw new ArgumentException($"Byte array too small for type {typeof(T)}. Expected at least {size} bytes, got {arr.Length}.");

        IntPtr ptr = Marshal.AllocHGlobal(size);
        try
        {
            Marshal.Copy(arr, 0, ptr, size);
            return Marshal.PtrToStructure<T>(ptr);
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }
    
    public static (int function, T payload) DeserializeQueryData<T>(byte[] data) where T : struct
    {
        int function = BitConverter.ToInt32(data, 0);
        byte[] payloadBytes = new byte[data.Length - 4];
        Buffer.BlockCopy(data, 4, payloadBytes, 0, payloadBytes.Length);

        T payload = BytesToStruct<T>(payloadBytes);
        return (function, payload);
    }


}