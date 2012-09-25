namespace HumbleNetwork.Tests.Helpers
{
    using System.Net;

    internal class SendPortCommand : ICommand
    {
        public void Execute(IHumbleStream stream)
        {
            stream.Send(((IPEndPoint) stream.TcpClient.Client.LocalEndPoint).Port.ToString());
        }
    }
}