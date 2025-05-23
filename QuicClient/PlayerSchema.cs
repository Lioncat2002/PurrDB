using System.Runtime.InteropServices;

namespace QuicClient;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct QueryData<T> where T : struct
{
    public int Function;
    public T Payload;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct PlayerData
{
    public int id;
    public float x;
    public float y;
}

