using System.Runtime.InteropServices;

namespace GameServer.Schemas;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct QueryData<T> where T : struct
{
    public int Function;
    public T Payload;
}