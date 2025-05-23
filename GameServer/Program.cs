using GameServer.Config;
using GameServer.Router;
using QuicNet;

namespace GameServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DB.Instance.Migrate();
            GameRouter _router = new GameRouter();
            QuicListener listener = new QuicListener(11000);
            listener.OnClientConnected += _router.ClientConnected;
            //listener.Close();
            listener.Start();
            Console.ReadKey();
        }
    }
}