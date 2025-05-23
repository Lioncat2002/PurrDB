using System.Runtime.InteropServices;

namespace GameServer.Schemas
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PlayerData
    {
        public int id;
        public float x;
        public float y;
    }
}
