#define SERVER
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp10
{
    class Program
    {
        static void Main(string[] args)
        {

            //NetPacket packet = new NetPacket() { Data = "test", DataType = NetPacket.ObjectType.TEXT };
            //byte[] arr = NetPacket.Serialazie(packet)
            //Console.WriteLine(arr);
            //Console.WriteLine(NetPacket.Deserialize(arr));

#if SERVER
            using (var server = new Server())
            {
                server.Start();
            }
#else
            using (var server = new Client())
            {
                server.Start();
            }
#endif

        }
    }
}
