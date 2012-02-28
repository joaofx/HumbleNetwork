namespace HumbleServer.Tests.Helpers
{
    public class PingCommand : CommandBase
    {
        public override void Execute()
        {
            this.stream.Send("PONG");
        }
    }
}