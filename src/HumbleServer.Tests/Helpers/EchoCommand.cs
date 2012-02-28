namespace HumbleServer.Tests.Helpers
{
    public class EchoCommand : CommandBase
    {
        public override void Execute()
        {
            var message = this.stream.Receive();
            this.stream.Send(message);
        }
    }
}