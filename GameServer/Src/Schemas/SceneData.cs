using System.Runtime.InteropServices;

namespace GameServer.Schemas
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SceneData
    {
        public uint totalPlayers;
    }
}

