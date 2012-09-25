namespace HumbleNetwork.Tests
{
    using System.Collections.Generic;
    using System.Threading;
    using Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class ActivePassiveClusterTest
    {
        private ClusteredServer active;
        private ClusteredServer passive;
        private ClusteredClient client1;
        private ClusteredClient client2;

        [SetUp]
        public void Setup()
        {
            var queue = new Queue<int>();

            for (int i = 1; i <= 10; i++)
            {
                queue.Enqueue(i);
            }

            var servers = new[] { "localhost:9520", "localhost:9521" };

            this.active = this.CreateServer(servers, queue);
            this.passive = this.CreateServer(servers, queue);

            this.active.Start(9520);
            this.passive.Start(9521);

            this.client1 = new ClusteredClient(servers);
            this.client2 = new ClusteredClient(servers);

            this.client1.Connect();
            this.client2.Connect();
        }

        [Test]
        public void Should_return_port_from_the_server_which_client_has_connected()
        {
            Assert.That(this.client1.Send("port").Receive(), Is.EqualTo("9520"));
            this.active.Stop();
            Thread.Sleep(3000);
            Assert.That(this.client1.Send("port").Receive(), Is.EqualTo("9521"));
        }

        ////[Test]
        ////public void Should_connect_to_actual_active_server()
        ////{
        ////    Assert.That(this.client1.Send("dequ").Receive(), Is.EqualTo(1));
        ////    Assert.That(this.client2.Send("dequ").Receive(), Is.EqualTo(2));
        ////    Assert.That(this.client1.Send("dequ").Receive(), Is.EqualTo(3));

        ////    this.active.Stop();

        ////    Assert.That(this.client2.Send("dequ").Receive(), Is.EqualTo(4));
        ////    Assert.That(this.client1.Send("dequ").Receive(), Is.EqualTo(5));

        ////    this.active.Start(9520);

        ////    Assert.That(this.client2.Send("dequ").Receive(), Is.EqualTo(6));

        ////    this.passive.Stop();

        ////    Assert.That(this.client2.Send("dequ").Receive(), Is.EqualTo(7));
        ////    Assert.That(this.client1.Send("dequ").Receive(), Is.EqualTo(8));

        ////    this.passive.Start(9521);

        ////    Assert.That(this.client1.Send("dequ").Receive(), Is.EqualTo(9));
        ////    Assert.That(this.client2.Send("dequ").Receive(), Is.EqualTo(10));
        ////}

        private ClusteredServer CreateServer(string[] servers, Queue<int> queue)
        {
            var server = new ClusteredServer(servers);
            server.AddCommand("dequ", () => new DequeueCommand(queue));
            server.AddCommand("port", () => new SendPortCommand());
            return server;
        }
    }
}
