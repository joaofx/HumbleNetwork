namespace HumbleNetwork.Tests.Streams
{
    using System.Net.Sockets;
    using NUnit.Framework;

    [TestFixture]
    public class DelimitedStreamTest : StreamTestBase
    {
        protected override IHumbleStream CreateStream(TcpClient tcpClient)
        {
            return MessageFraming.Create(Framing.Delimitered, tcpClient, MessageFraming.DefaultDelimiter);
        }
    }
}
