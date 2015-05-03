namespace HumbleNetwork.Tests
{
    using System.IO;
    using Helpers;
    using HumbleNetwork;
    using NBehave.Spec.NUnit;
    using NUnit.Framework;

    [TestFixture]
    public class HumbleNetworkTest : HumbleTestBase
    {
        private HumbleClient _client;

        [Test]
        public void Should_return_server_port()
        {
            Assert.That(Server.Port, Is.Not.EqualTo(0));
        }

        [Test]
        public void Should_process_echo_command()
        {
            _client.Send("ECHO").Send("hello").Receive().ShouldEqual("hello");
        }

        [Test]
        public void Should_process_echo_command_in_one_request()
        {
            _client.Send("ECHOhello").Receive().ShouldEqual("hello");
        }

        [Test]
        public void Shoud_process_echo_command_many_times()
        {
            _client.Send("ECHO").Send("hello").Receive().ShouldEqual("hello");
            _client.Send("ECHO").Send("Other hello").Receive().ShouldEqual("Other hello");
            _client.Send("ECHO").Send("Third hello").Receive().ShouldEqual("Third hello");
        }

        [Test]
        public void Should_process_echo_command_with_big_message()
        {
            var message = StringExtension.GenerateRandomString(1 << 16);

            var received = _client.Send("ECHO")
                .Send(message)
                .Receive();

            received.Length.ShouldEqual(message.Length);
            received.ShouldEqual(message);
        }

        [Test]
        public void Shoud_process_ping_command()
        {
            _client.Send("PING").Receive().ShouldEqual("PONG");
        }

        [Test]
        public void Shoud_process_ping_command_many_times()
        {
            _client.Send("PING").Receive().ShouldEqual("PONG");
            _client.Send("PING").Receive().ShouldEqual("PONG");
            _client.Send("PING").Receive().ShouldEqual("PONG");
        }

        [Test]
        public void Should_treat_unknow_command_with_a_custom_handler()
        {
            Server.UnknowCommandHandler = () => new CustomUnknowCommandHandler();

            _client.Send("????").Receive().ShouldEqual("CustomUnknowCommandHandler");
        }

        [Test]
        public void Should_treat_unknow_command_without_set_a_handler()
        {
            _client.Send("????").Receive().ShouldEqual("UNKN");
        }

        [Test]
        public void Should_treat_exception_without_a_handler()
        {
            Server.AddCommand("EXCE", () => new ThrowExceptionCommand());

            _client
                .Send("EXCE")
                .Receive().ShouldEqual("InvalidOperationException: An exception was thrown");
        }

        [Test]
        public void Should_treat_exception_with_a_custom_handler()
        {
            Server.ExceptionHandler = () => new CustomExceptionHandler();
            Server.AddCommand("EXCE", () => new ThrowExceptionCommand());

            _client
                .Send("EXCEblablablablablablablabla")
                .Receive().ShouldEqual("CustomExceptionHandler: InvalidOperationException");
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

            client1.Send("echohello1").Receive().ShouldEqual("hello1");
            client2.Send("echohello2").Receive().ShouldEqual("hello2");

            server1.Stop();
            server2.Stop();
        }

        [Test]
        [ExpectedException(typeof(IOException))]
        public void Should_throw_exception_when_read_timeout_was_fired()
        {
            new HumbleClient(receiveTimeOut: 1000)
                .Connect("localhost", Server.Port)
                .Send("WAIT")
                .Receive();
        }

        [Test]
        [ExpectedException(typeof(IOException))]
        public void After_server_has_stoped_client_cannot_send_messages()
        {
            _client.Send("PING");
            Assert.That(_client.Receive(), Is.EqualTo("PONG"));

            Server.Stop();
            _client.Send("PING");
        }

        [Test]
        public void After_server_has_stoped_client_cannot_receive_messages()
        {
            _client.Send("PING");
            Assert.That(_client.Receive(), Is.EqualTo("PONG"));

            Server.Stop();
            Assert.That(_client.Receive(), Is.Empty);
        }

        [Test]
        public void Should_assign_write_and_read_timeout_before_connect()
        {
            new HumbleClient(sendTimeOut: 30000, receiveTimeOut: 30000)
                .Connect("localhost", Server.Port)
                .Dispose();
        }

        [Test]
        public void Client_can_connect_even_if_it_is_already_connected()
        {
            _client.Connect("localhost", Server.Port);
            _client.Connect("localhost", Server.Port);
            _client.Send("PING").Receive().ShouldEqual("PONG");
        }

        [Test]
        public void Client_can_reconnect_when_server_stops_for_a_while()
        {
            Server.Stop();

            try
            {
                _client.Send("PING").Receive().ShouldEqual("PONG");
                Assert.Fail("Should thrown IOException");
            }
            catch
            {
            }

            Server.Start(Server.Port);

            _client.Connect("localhost", Server.Port);
            _client.Send("PING").Receive().ShouldEqual("PONG");
        }

        protected override void BeforeTest()
        {
            Server.AddCommand("echo", () => new EchoCommand());
            Server.AddCommand("ping", () => new PingCommand());
            Server.AddCommand("wait", () => new WaitCommand());

            _client = new HumbleClient(receiveTimeOut: 5000, sendTimeOut: 5000)
                .Connect("localhost", this.Server.Port);
        }
    }
}
