namespace HumbleNetwork.Tests
{
    using System.IO;
    using System.Threading;
    using Helpers;
    using HumbleNetwork;
    using NUnit.Framework;

    /// <summary>
    /// TODO: client is not really connected 
    /// </summary>
    [TestFixture]
    public class HumbleNetworkTest : HumbleTestBase
    {
        private HumbleClient client;

        [Test]
        public void Should_return_server_port()
        {
            Assert.That(this.server.Port, Is.Not.EqualTo(0));
        }

        [Test]
        public void Should_process_echo_command()
        {
            this.client.Send("ECHO").Send("hello");
            Assert.That(this.client.Receive(), Is.EqualTo("hello"));
        }

        [Test]
        public void Should_process_echo_command_in_one_request()
        {
            this.client.Send("ECHOhello");
            Assert.That(this.client.Receive(), Is.EqualTo("hello"));
        }

        [Test]
        public void Shoud_process_echo_command_many_times()
        {
            this.client.Send("ECHO").Send("hello");
            Assert.That(this.client.Receive(), Is.EqualTo("hello"));

            this.client.Send("ECHO").Send("Other hello");
            Assert.That(this.client.Receive(), Is.EqualTo("Other hello"));

            this.client.Send("ECHO").Send("Third hello");
            Assert.That(this.client.Receive(), Is.EqualTo("Third hello"));
        }

        [Test]
        public void Should_process_echo_command_with_big_message()
        {
            var message = StringExtension.GenerateRandomString(1 << 16);
            this.client.Send("ECHO").Send(message);
            var received = this.client.Receive();

            Assert.That(received.Length, Is.EqualTo(message.Length));
            Assert.That(received, Is.EqualTo(message));
        }

        [Test]
        public void Shoud_process_ping_command()
        {
            this.client.Send("PING");
            Assert.That(this.client.Receive(), Is.EqualTo("PONG"));
        }

        [Test]
        public void Shoud_process_ping_command_many_times()
        {
            this.client.Send("PING");
            Assert.That(this.client.Receive(), Is.EqualTo("PONG"));

            this.client.Send("PING");
            Assert.That(this.client.Receive(), Is.EqualTo("PONG"));

            this.client.Send("PING");
            Assert.That(this.client.Receive(), Is.EqualTo("PONG"));
        }

        [Test]
        public void Should_treat_unknow_command_with_a_custom_handler()
        {
            this.server.UnknowCommandHandler = () => new CustomUnknowCommandHandler();

            this.client.Send("????");
            Assert.That(this.client.Receive(), Is.EqualTo("CustomUnknowCommandHandler"));
        }

        [Test]
        public void Should_treat_unknow_command_without_set_a_handler()
        {
            this.client.Send("????");
            Assert.That(this.client.Receive(), Is.EqualTo("UNKN"));
        }

        [Test]
        public void Should_treat_exception_without_a_handler()
        {
            this.server.AddCommand("EXCE", () => new ThrowExceptionCommand());

            this.client.Send("EXCE");
            Assert.That(this.client.Receive(), Is.EqualTo("InvalidOperationException: An exception was thrown"));
        }

        [Test]
        public void Should_treat_exception_with_a_custom_handler()
        {
            this.server.ExceptionHandler = () => new CustomExceptionHandler();
            this.server.AddCommand("EXCE", () => new ThrowExceptionCommand());

            this.client.Send("EXCEblablablablablablablabla");
            Assert.That(this.client.Receive(), Is.EqualTo("CustomExceptionHandler: InvalidOperationException"));
        }

        [Test]
        public void Should_accept_differents_delimiters_for_differents_instances()
        {
            var server1 = new HumbleServer(Framing.Delimitered, "[DEL1]");
            var server2 = new HumbleServer(Framing.Delimitered, "[DEL2]");
            
            server1.AddCommand("echo", () => new EchoCommand());
            server2.AddCommand("echo", () => new EchoCommand());

            server1.Start(0);
            server2.Start(0);

            var client1 = new HumbleClient(Framing.Delimitered, "[DEL1]");
            var client2 = new HumbleClient(Framing.Delimitered, "[DEL2]");

            client1.Connect("localhost", server1.Port);
            client2.Connect("localhost", server2.Port);

            Assert.That(client1.Send("echohello1").Receive(), Is.EqualTo("hello1"));
            Assert.That(client2.Send("echohello2").Receive(), Is.EqualTo("hello2"));

            server1.Stop();
            server2.Stop();
        }

        [Test]
        [ExpectedException(typeof(IOException))]
        public void Should_throw_exception_when_read_timeout_was_fired()
        {
            this.client.ReceiveTimeOut = 1000;
            this.client.Send("WAIT");
            this.client.Receive();
        }

        [Test]
        [ExpectedException(typeof(IOException))]
        public void After_server_has_stoped_client_cannot_send_messages()
        {
            this.client.Send("PING");
            Assert.That(this.client.Receive(), Is.EqualTo("PONG"));

            this.server.Stop();
            this.client.Send("PING");
        }

        [Test]
        public void After_server_has_stoped_client_cannot_receive_messages()
        {
            this.client.Send("PING");
            Assert.That(this.client.Receive(), Is.EqualTo("PONG"));

            this.server.Stop();
            Assert.That(this.client.Receive(), Is.Empty);
        }

        protected override void BeforeTest()
        {
            this.server.AddCommand("echo", () => new EchoCommand());
            this.server.AddCommand("ping", () => new PingCommand());
            this.server.AddCommand("wait", () => new WaitCommand());

            this.client = new HumbleClient().Connect("localhost", this.server.Port);
        }
    }
}
