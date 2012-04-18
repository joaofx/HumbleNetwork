using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HumbleNetwork
{
    using System.Net.Sockets;

    public static class TcpClientExtensions
    {
        /// <summary>
        /// There's no method to really know if the client is connected on the server.
        /// So I'm using this workaround that I found in some sites on internet
        /// </summary>
        /// <returns></returns>
        public static bool IsItReallyConnected(this TcpClient tcpClient)
        {
            if (tcpClient.Connected == false)
            {
                return false;
            }

            if (tcpClient.Available > 0)
            {
                return false;
            }

            var blockingState = tcpClient.Client.Blocking;
            try
            {
                var tmp = new byte[1];

                tcpClient.Client.Blocking = false;
                var received = tcpClient.Client.Receive(tmp, 0, 0);
                return received > 0;
            }
            catch (SocketException e)
            {
                if (e.NativeErrorCode.Equals(10035))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            finally
            {
                tcpClient.Client.Blocking = blockingState;
            }
        }
    }
}
