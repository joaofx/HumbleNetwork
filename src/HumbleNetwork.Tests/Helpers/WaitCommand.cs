namespace HumbleNetwork.Tests.Helpers
{
    using System;
    using System.Threading;
    using HumbleNetwork;

    internal class WaitCommand : ICommand
    {
        public void Execute(IHumbleStream stream)
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));
            stream.Send("1");
        }
    }
}