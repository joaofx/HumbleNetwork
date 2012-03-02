
namespace HumbleServer.Tests.Streams
{
    using System.Net.Sockets;
    using Helpers;
    using HumbleServer.Streams;
    using NUnit.Framework;

    [TestFixture]
    public class DelimitedStreamTest : StreamTestBase
    {
        protected override IHumbleStream CreateStream(NetworkStream stream)
        {
            return new DelimitedStream(stream);
        }
    }
}
