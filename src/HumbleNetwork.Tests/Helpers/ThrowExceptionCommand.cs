namespace HumbleNetwork.Tests.Helpers
{
    using System;
    using HumbleNetwork;
    using HumbleNetwork.Streams;

    public class ThrowExceptionCommand : ICommand
    {
        public void Execute(IHumbleStream stream)
        {
            throw new InvalidOperationException("An exception was thrown");
        }
    }
}