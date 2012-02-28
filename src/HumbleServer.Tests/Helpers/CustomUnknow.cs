namespace HumbleServer.Tests.Helpers
{
    public class CustomUnknow : CommandBase
    {
        public override void Execute()
        {
            this.stream.Send("CustomUnknow");
        }
    }
}