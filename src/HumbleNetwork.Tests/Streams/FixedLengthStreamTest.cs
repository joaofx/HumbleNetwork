
namespace HumbleNetwork.Tests.Streams
{
    using System.Net.Sockets;
    using HumbleNetwork.Streams;
    using NUnit.Framework;

    [TestFixture]
    public class FixedLengthStreamTest : StreamTestBase
    {
        protected override IHumbleStream CreateStream(TcpClient client)
        {
            return new FixedLengthStream(client);
        }
    }
}
