using NUnit.Framework;

namespace HumbleServer.Tests
{
    using System;
    using Commands;

    /// TODO: grande quantidade de bytes [stream test]
    /// TODO: mensagem com tamanho pré-fixada (padrao)
    /// TODO: mensagem com delimitador
    /// TODO: tratar mensagens quebradas
    /// TODO: timeout
    /// TODO: tratamento de erro
    /// TODO: retornar porta
    /// TODO: tratar comando desconhecido
    [TestFixture]
    public class HumbleServerTest : HumbleTestBase
    {
        private NetworkClient client;

        protected override void BeforeTest()
        {
		    this.server.AddCommand("echo", () => new Echo());
            this.server.AddCommand("ping", () => new Ping());

            this.client = new NetworkClient().Connect("localhost", 987);
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
            this.server.UnknowCommand = () => new CustomUnknow();

            this.client.Send("????");
            Assert.That(client.Receive(), Is.EqualTo("CustomUnknow"));
        }

        [Test]
        public void Should_treat_unknow_command_without_set_a_handler()
        {
            this.client.Send("????");
            Assert.That(client.Receive(), Is.EqualTo("UNKN"));
        }

        [Test]
        public void Should_treat_exception_on_server_side()
        {
            this.server.UnknowCommand = () => new ThrowException();

            this.client.Send("EXCE");
            Assert.That(client.Receive(), Is.EqualTo("InvalidOperationException: An exception was thrown"));
        }
    }

    public class ThrowException : CommandBase
    {
        public override void Execute()
        {
            throw new InvalidOperationException("An exception was thrown");
        }
    }
}
