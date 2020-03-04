using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp10
{
    class Client : IDisposable
    {
        public string Name { get; set; }
        public IPEndPoint ServerPoint;

        public async void  Start()
        {
            Console.WriteLine("Enter name : ");
            Name = Console.ReadLine();
            string adress;
            Console.WriteLine("Enter server adress : ");
            adress = Console.ReadLine();
            int port;
            Console.WriteLine("Enter server port : ");
            port = int.Parse(Console.ReadLine());
            ServerPoint = new IPEndPoint(IPAddress.Parse(adress), port);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //socket.Bind(ServerPoint);
            socket.Connect(ServerPoint);
            socket.Send(await NetPacket.Serialazie(new NetPacket() { PacketType = NetPacket.NetPacketType.CONNECT, Data = Name }));
            bool exit = false;
            Console.WriteLine("Comands: GET: , POST: ,EXIT");
            do
            {
                ParseInput(exit, socket);
            } while (!exit);
            socket.Send(await NetPacket.Serialazie(new NetPacket() { PacketType = NetPacket.NetPacketType.DISCONECT, Data = Name }));
            socket.Shutdown(SocketShutdown.Both);
        }
        private async void ParseInput(bool exit,Socket ConnectionSocket)
        {
            Console.Write("Enter text : ");
            string Text = Console.ReadLine();
            if (Text.Contains("EXIT"))
            {
                ConnectionSocket.Send(await NetPacket.Serialazie(new NetPacket() { PacketType = NetPacket.NetPacketType.DISCONECT }));
                exit = true;
            }

            if (Text.Contains("POST:"))
            {
                ConnectionSocket.Send(await NetPacket.Serialazie(new NetPacket()
                {
                    Data = new Message() { Owner = Name , Time = DateTime.Now, Text = Text.Replace("POST:","")},
                    PacketType = NetPacket.NetPacketType.POST,
                }
                ));
            }

            if (Text.Contains("GET:"))
            {
                ConnectionSocket.Send(await NetPacket.Serialazie(new NetPacket()
                {
                    Data = new Message() { Owner = Text.Replace("GET:",""), Time = DateTime.Now },
                    PacketType = NetPacket.NetPacketType.GET,
                }
                ));
                var response = await NetPacket.Deserialize(ConnectionSocket, 1024);
                if (response.DataType==typeof(List<Message>))
                {
                    (response.Data as List<Message>).ForEach(Console.WriteLine);
                }
            }
        }
        public void Dispose()
        {

        }
    }
}
