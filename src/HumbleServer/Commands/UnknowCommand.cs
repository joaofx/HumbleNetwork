namespace HumbleServer.Commands
{
    internal class UnknowCommand : CommandBase
    {
        public override void Execute()
        {
            this.stream.Send("UNKN");
        }
    }
}