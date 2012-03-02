﻿
namespace HumbleServer.Tests.Streams
{
    using System.Net.Sockets;
    using HumbleServer.Streams;
    using NUnit.Framework;

    [TestFixture]
    public class FixedLengthStreamTest : StreamTestBase
    {
        protected override IHumbleStream CreateStream(NetworkStream stream)
        {
            return new FixedLengthStream(stream);
        }
    }
}