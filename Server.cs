//using ServerMSG;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Net.Sockets;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.Text;
//using System.Threading.Tasks;
//using UNP;
//using UNP.Packet;

//namespace ConsoleApp10
//{
//    class Server : IDisposable
//    {
//        public IPEndPoint EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2555);
//        //public IPEndPoint EndPoint =  Dns.
//        public bool Active { get; set; } = true;

//        public void Dispose()
//        {
//        }

//        Dictionary<string, List<ServerMSG.Message>> Messages = new Dictionary<string, List<ServerMSG.Message>>();

//        public void Start()
//        {
//            try
//            {
//                var MainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//                MainSocket.Bind(EndPoint);
//                MainSocket.Listen(100);
//                Console.WriteLine($"Server run on {EndPoint.Address}:{EndPoint.Port}");
//                do
//                {
//                    HandleClient(MainSocket.Accept());
//                } while (Active);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.ToString());
//                Console.WriteLine("Server failure!");
//                Console.ReadKey();
//            }

//        }


//        private void HandleClient(Socket s)
//        {
//            Task.Run(async () =>
//            {
//                bool online = true;
//                UNP.Packet.NetPacket packet;
//                do
//                {
//                    packet = await NetPacket.DeserializeAsync(s, 1024);
//                    switch (packet.PacketType)
//                    {
//                        case NetPacket.NetPacketType.CONNECT:
//                            online = true;
//                            Console.WriteLine("Client connected");
//                            break;
//                        case NetPacket.NetPacketType.DISCONECT:
//                            online = false;
//                            Console.WriteLine("Client disconnected");
//                            break;
//                        case NetPacket.NetPacketType.GET:
//                            GetMessage(s, packet);
//                            break;
//                        case NetPacket.NetPacketType.SERVICE:

//                            break;
//                        case NetPacket.NetPacketType.POST:
//                            PostMessage(packet);
//                            break;
//                        default:
//                            break;
//                    }
//                } while (online);
//            });
//        }
//        private void GetMessage(Socket clientSocket, NetPacket packet)
//        {
//            lock (Messages)
//            {
//                Console.WriteLine("get request");
//                if (packet.Message.DataType == typeof(ServerMSG.Message))
//                {
//                    //получаем сообщение
//                    var msg = (packet.Message.Data as ServerMSG.Message);

//                    List<ServerMSG.Message> t;
//                    if (!Messages.TryGetValue(msg.Owner, out t))
//                    {
//                        clientSocket.Send(NetPacket.SerialazieAsync(new NetPacket()
//                        {
//                            PacketType = NetPacket.NetPacketType.RESPONSE,
//                            ResponseOn = packet,
//                            Data = new ServerMSG.Message() { Owner = "Server", Time = DateTime.Now, Text = "no messages" }
//                        }).Result);
//                    }
//                    else
//                        clientSocket.Send(NetPacket.SerialazieAsync(new NetPacket()
//                        {
//                            PacketType = NetPacket.NetPacketType.RESPONSE,
//                            ResponseOn = packet,
//                            Data = t
//                        }).Result);
//                }
//            }

//        }
//        private void PostMessage(NetPacket packet)
//        {
//            lock (Messages)
//            {
//                if (packet.DataType != typeof(Message))
//                    return;
//                var msg = packet.Data as Message;
//                List<Message> list;
//                if (Messages.TryGetValue(msg.Owner, out list))
//                {
//                    list.Add(msg);
//                }
//                else
//                {
//                    Messages.Add(msg.Owner, new List<Message>() { msg });
//                }
//            }
//        }
//    }
//}
