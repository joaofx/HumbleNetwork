
namespace HumbleNetwork.Tests.Streams
{
    using System.Net.Sockets;
    using HumbleNetwork.Streams;
    using NUnit.Framework;

    [TestFixture]
    public class DelimitedStreamTest : StreamTestBase
    {
        protected override IHumbleStream CreateStream(TcpClient tcpClient)
        {
            return new DelimitedStream(tcpClient);
        }

        [Test]
        public void Should_be_able_to_change_the_delimited_string()
        {
            DelimitedStream.Delimiter = "__";
            this.StreamTest((sender, receiver) =>
            {
                sender.Send("hello 1");
                sender.Send("hello 2");
                Assert.That(receiver.Receive(), Is.EqualTo("hello 1"));
                Assert.That(receiver.Receive(), Is.EqualTo("hello 2"));
            });
        }
    }
}
