namespace HumbleServer.Tests.Helpers
{
    using System;
    using System.Threading;

    public class WaitCommand : CommandBase
    {
        public override void Execute()
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));
            this.stream.Send("1");
        }
    }
}