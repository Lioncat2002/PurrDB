
using System.Runtime.InteropServices;
using System.Text;
using QuicClient;
using QuickNet.Utilities;
using QuicNet;
using QuicNet.Connections;
using QuicNet.Infrastructure;
using QuicNet.Infrastructure.Frames;
using QuicNet.Infrastructure.Packets;
using QuicNet.Streams;


   public class Program
    {
        
        public static byte[] StructToBytes<T>(T strct) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            byte[] arr = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(strct, ptr, true);
                Marshal.Copy(ptr, arr, 0, size);
                arr.Append<byte>(0x00);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
            return arr;
        }
        
        public static byte[] SerializeQueryData<T>(int function, T payload) where T : struct
        {
            byte[] functionBytes = BitConverter.GetBytes(function);
            byte[] payloadBytes = StructToBytes(payload);

            byte[] result = new byte[functionBytes.Length + payloadBytes.Length];
            Buffer.BlockCopy(functionBytes, 0, result, 0, functionBytes.Length);
            Buffer.BlockCopy(payloadBytes, 0, result, functionBytes.Length, payloadBytes.Length);
            
            return result;
        }



        static void Main(string[] args)
        {
            QuicNet.QuicClient client = new QuicNet.QuicClient();
            
            // Connect to peer (Server)
            QuicConnection connection = client.Connect("127.0.0.1", 11000);
            // Create a data stream
            
            var player = new PlayerData
            {
                id = 1,
                x = 1.0f,
                y = 7.0f,
            };
            // Send Data
           
            
            {
                QuicStream stream1 = connection.CreateStream(QuickNet.Utilities.StreamType.ClientBidirectional);
                player.x = 5;
                player.y = 7;
                var queryBytes1 = SerializeQueryData(0x01, player);
                stream1.Send(queryBytes1);
                byte[] data2 = stream1.Receive();
                Console.WriteLine(Encoding.UTF8.GetString(data2)+" "+stream1.IsOpen());
            }

           // for (int i = 0; i < 256; i++)
            //{
                QuicConnection conn = client.Connect("127.0.0.1", 11000);

                var nstream = conn.CreateStream(StreamType.ClientBidirectional);
                Console.WriteLine("Updating player position");
                player.x = 5;
                player.y = 7;
                var resp1 = SerializeQueryData(0x03, player);
                nstream.Send(resp1);
                var ndataw = nstream.Receive();
                
                
                var closePacket1 = conn.PacketCreator.CreateConnectionClosePacket(
                    ErrorCode.NO_ERROR, 0x00, "Closing connection"
                );
                conn.Send(closePacket1);
                conn.TerminateConnection();
            //}
            Thread.Sleep(2000);
            Console.ReadKey();
            // put a thread.sleep so that the server can process the data.
            var stream = connection.CreateStream(StreamType.ServerBidirectional);

            var resp = SerializeQueryData(0x02, player);
            stream.Send(resp);
            var ndata1 = stream.Receive();
                
            Console.WriteLine("Players in range: "+Encoding.UTF8.GetString(ndata1));

            
           Console.ReadKey();
           var closePacket = connection.PacketCreator.CreateConnectionClosePacket(
                ErrorCode.NO_ERROR, 0x00, "Closing connection"
            );
            connection.Send(closePacket);
            connection.TerminateConnection();
        }
    }
