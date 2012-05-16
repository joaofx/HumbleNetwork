namespace HumbleNetwork.Tests.Streams
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using Helpers;
    using NUnit.Framework;

    public abstract class StreamTestBase
    {
        [Test]
        public void Should_send_and_receive_message()
        {
            this.StreamTest((sender, receiver) =>
            {
                sender.Send("hello");
                Assert.That(receiver.Receive(), Is.EqualTo("hello"));
            });
        }

        [Test]
        public void Should_send_message_and_receive_a_little_on_each_call()
        {
            this.StreamTest((sender, receiver) =>
            {
                sender.Send("hello world");
                Assert.That(receiver.Receive(5), Is.EqualTo("hello"));
                Assert.That(receiver.Receive(1), Is.EqualTo(" "));
                Assert.That(receiver.Receive(5), Is.EqualTo("world"));
            });
        }

        [Test]
        public void Shoud_handle_send_and_receive_and_internal_buffer()
        {
            this.StreamTest((sender, receiver) =>
            {
                sender.Send("hello world!!!");
                Assert.That(receiver.Receive(5), Is.EqualTo("hello"));
                Assert.That(receiver.Receive(), Is.EqualTo(" world!!!"));
            });
        }

        [Test]
        public void Should_send_and_receive_different_characters()
        {
            this.StreamTest((sender, receiver) =>
            {
                sender.Send("ú");
                Assert.That(receiver.Receive(), Is.EqualTo("ú"));
            });
        }

        [Test]
        public void Should_send_and_receive_batch_of_messages()
        {
            this.StreamTest((sender, receiver) =>
            {
                sender.Send("hello");
                sender.Send("wonderful");
                sender.Send("world");
                Assert.That(receiver.Receive(), Is.EqualTo("hello"));
                Assert.That(receiver.Receive(), Is.EqualTo("wonderful"));
                Assert.That(receiver.Receive(), Is.EqualTo("world"));
            });
        }

        /// <summary>
        /// TODO: Too much slow. Fix it.
        /// </summary>
        [Test]
        public void Should_send_and_receive_big_message()
        {
            this.StreamTest((sender, receiver) =>
            {
                var message = StringExtension.GenerateRandomString(1 << 16);
                sender.Send(message);
                Assert.That(receiver.Receive(), Is.EqualTo(message));
            });
        }

        protected abstract IHumbleStream CreateStream(TcpClient client);

        protected void StreamTest(Action<IHumbleStream, IHumbleStream> action)
        {
            var mre = new ManualResetEvent(false);
            var listener = new TcpListener(IPAddress.Any, 61234);
            var tcpReceiver = new TcpClient();
            Exception asyncException = null;

            listener.Start();

            listener.BeginAcceptTcpClient(result =>
            {
                var tcpSender = listener.EndAcceptTcpClient(result);

                try
                {
                    var sender = this.CreateStream(tcpSender);
                    var receiver = this.CreateStream(tcpReceiver);

                    action(sender, receiver);
                }
                catch (Exception exception)
                {
                    asyncException = exception;
                }
                finally
                {
                    tcpReceiver.Close();
                    tcpSender.Close();
                    listener.Stop();
                    mre.Set();
                }
            }, null);

            tcpReceiver.Connect("localhost", 61234);
            mre.WaitOne();

            if (asyncException != null)
            {
                throw asyncException;
            }
        }
    }
}
