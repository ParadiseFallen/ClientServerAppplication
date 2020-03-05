using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UNP;

namespace UnitTestUNP
{
    [TestClass]
    public class UnitTestUNP
    {
        [TestMethod]
        public void TestSerializationAndBack()
        {
            var packet = TestpacketCreation();
            var byteArr = TestSerializationPacket(packet);
            var packet2 = TestDeserializationPacket(byteArr);
            Assert.AreEqual(packet, packet2);
        }
        [TestMethod]
        public byte[] TestSerializationPacket(NetPacket packet)
        {
            var arr = NetPacket.Serialazie(packet);
            Assert.IsNotNull(arr);
            return arr;
        }
        [TestMethod]
        public NetPacket TestDeserializationPacket(byte[] data)
        {
            var packet = NetPacket.Deserialize(data);
            Assert.IsNotNull(packet);
            Assert.IsInstanceOfType(packet,typeof(NetPacket));
            return packet;
        }

        [TestMethod]
        public NetPacket TestpacketCreation(object Data=null, NetPacket.NetPacketType packetType = NetPacket.NetPacketType.CLEAR,Message msg = null, NetPacket responsePacket = null )
        {
            return new NetPacket() { Data = "Hello!", PacketType = NetPacket.NetPacketType.POST,ResponseOn = responsePacket, Message = msg };
        }
    }
}
