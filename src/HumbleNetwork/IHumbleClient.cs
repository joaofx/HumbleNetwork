namespace HumbleNetwork
{
    using System;

    public interface IHumbleClient : IDisposable
    {
        int ReceiveTimeOut
        {
            get;
            set;
        }

        HumbleClient Send(string data);

        string Receive();

        HumbleClient Connect(string host, int port);
    }
}