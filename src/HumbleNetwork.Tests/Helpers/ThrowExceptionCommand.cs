namespace HumbleNetwork.Tests.Helpers
{
    using System;
    using HumbleNetwork;
    using HumbleNetwork.Streams;

    internal class ThrowExceptionCommand : ICommand
    {
        public void Execute(IHumbleStream stream)
        {
            throw new InvalidOperationException("An exception was thrown");
        }
    }
}