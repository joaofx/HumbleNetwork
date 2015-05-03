namespace HumbleNetwork
{
    using System;

    public interface IExceptionHandler
    {
        void Execute(IHumbleStream stream, Exception exception);
    }
}