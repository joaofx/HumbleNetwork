
namespace HumbleServer.Tests.Streams
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using HumbleServer.Streams;

    public class StreamTestBase
    {
        protected void StreamTest(Action<FixedLengthStream, FixedLengthStream> action)
        {
            var mre = new ManualResetEvent(false);
            var listener = new TcpListener(IPAddress.Any, 61234);
            var tcpReceiver = new TcpClient();
            Exception asyncException = null;

            listener.Start();

            listener.BeginAcceptTcpClient(result =>
            {
                var tcpSender = listener.EndAcceptTcpClient(result);

                try
                {
                    var sender = new FixedLengthStream(tcpSender.GetStream());
                    var receiver = new FixedLengthStream(tcpReceiver.GetStream());

                    action(sender, receiver);
                }
                catch (Exception exception)
                {
                    asyncException = exception;
                }
                finally
                {
                    tcpReceiver.Close();
                    tcpSender.Close();
                    listener.Stop();
                    mre.Set();
                }
            }, null);

            tcpReceiver.Connect("localhost", 61234);
            mre.WaitOne();

            if (asyncException != null)
            {
                throw asyncException;
            }
        }
    }
}
