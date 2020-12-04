using NUnit.Framework;
using MixUp;
using System.Net;

namespace NUnitTestProject1
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void LobbyServiceTest()
        {
            MixUp.Services.LobbyService ls = new MixUp.Services.LobbyService();
            Assert.IsTrue(ls.SaveLobbyCode("123457", "192.168.232.2"));
            Assert.IsTrue(!ls.SaveLobbyCode("123457", "192.168.232.2"));
        }

        [Test]
        public void RoomCodeGeneratorTest()
        {
            string firstIp = "192.168.232.7";
            RoomCodeGenerator rcg = new RoomCodeGenerator();
            IPAddress ipAdd = IPAddress.Parse(firstIp);
            string code = rcg.GenerateAndInsertCode(ipAdd);
            string result = rcg.GetRoomAddress(code).LobbyAddr;
            Assert.AreEqual(firstIp, result);
        }

    }
}