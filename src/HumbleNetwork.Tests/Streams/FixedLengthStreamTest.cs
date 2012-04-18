
namespace HumbleNetwork.Tests.Streams
{
    using System.Net.Sockets;
    using NUnit.Framework;

    [TestFixture]
    public class FixedLengthStreamTest : StreamTestBase
    {
        protected override IHumbleStream CreateStream(TcpClient client)
        {
            return MessageFraming.Create(Framing.LengthPrefixed, client, MessageFraming.DefaultDelimiter);
        }
    }
}
