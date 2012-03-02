using NUnit.Framework;

namespace HumbleServer.Tests
{
    using System.IO;
    using Helpers;

    /// TODO: control connected clients
    [TestFixture]
    public class HumbleServerTest : HumbleTestBase
    {
        private NetworkClient client;

        protected override void BeforeTest()
        {
            this.server.MessageFraming = MessageFraming.LengthPrefixing;
		    this.server.AddCommand("echo", () => new EchoCommand());
            this.server.AddCommand("ping", () => new PingCommand());
            this.server.AddCommand("wait", () => new WaitCommand());

            this.client = new NetworkClient().Connect("localhost", 987);
        }

        [Test]
        public void Should_return_server_port()
        {
            Assert.That(this.server.Port, Is.EqualTo(987));
        }

        [Test]
        public void Should_process_echo_command()
        {
            this.client.Send("ECHO").Send("hello");
            Assert.That(this.client.Receive(), Is.EqualTo("hello"));
        }

        [Test]
        public void Shoud_process_echo_command_many_times()
        {
            this.client.Send("ECHO").Send("hello");
            Assert.That(client.Receive(), Is.EqualTo("hello"));

            client.Send("ECHO").Send("Other hello");
            Assert.That(client.Receive(), Is.EqualTo("Other hello"));

            client.Send("ECHO").Send("Third hello");
            Assert.That(client.Receive(), Is.EqualTo("Third hello"));
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
            Assert.That(client.Receive(), Is.EqualTo("PONG"));
        }

        [Test]
        public void Shoud_process_ping_command_many_times()
        {
            this.client.Send("PING");
            Assert.That(client.Receive(), Is.EqualTo("PONG"));

            this.client.Send("PING");
            Assert.That(client.Receive(), Is.EqualTo("PONG"));

            this.client.Send("PING");
            Assert.That(client.Receive(), Is.EqualTo("PONG"));
        }

        [Test]
        public void Should_treat_unknow_command_with_a_custom_handler()
        {
            this.server.UnknowCommandHandler = () => new CustomUnknowCommandHandler();

            this.client.Send("????");
            Assert.That(client.Receive(), Is.EqualTo("CustomUnknowCommandHandler"));
        }

        [Test]
        public void Should_treat_unknow_command_without_set_a_handler()
        {
            this.client.Send("????");
            Assert.That(client.Receive(), Is.EqualTo("UNKN"));
        }

        [Test]
        public void Should_treat_exception_without_a_handler()
        {
            this.server.AddCommand("EXCE", ()=> new ThrowExceptionCommand());

            this.client.Send("EXCE");
            Assert.That(client.Receive(), Is.EqualTo("InvalidOperationException: An exception was thrown"));
        }

        [Test]
        public void Should_treat_exception_with_a_custom_handler()
        {
            this.server.ExceptionHandler = () => new CustomExceptionHandler();
            this.server.AddCommand("EXCE", () => new ThrowExceptionCommand());

            this.client.Send("EXCE");
            Assert.That(client.Receive(), Is.EqualTo("CustomExceptionHandler: InvalidOperationException"));
        }

        [Test]
        [ExpectedException(typeof(IOException))]
        public void Should_throw_exception_when_read_timeout_was_fired()
        {
            this.client.ReceiveTimeOut = 1000;
            this.client.Send("WAIT");
            this.client.Receive();
        }
    }
}
