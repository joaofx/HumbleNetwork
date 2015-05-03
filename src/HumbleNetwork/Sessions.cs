namespace HumbleNetwork
{
    using System.Collections.Generic;
    using System.Net.Sockets;

    public class Sessions
    {
        private readonly List<Session> _sessions = new List<Session>();
        private readonly object _locker = new object();

        public Session NewSession(HumbleServer humbleServer, TcpClient client, Framing framing, string delimiter)
        {
            var session = new Session(this, humbleServer, client, framing, delimiter);
            
            lock (_locker)
            {
                _sessions.Add(session);
            }

            return session;
        }

        public void Disposed(Session session)
        {
            lock (_locker)
            {
                _sessions.Remove(session);
            }
        }

        public void DisposeAllSessions()
        {
            var sessionsCopy = _sessions.ToArray();

            foreach (var session in sessionsCopy)
            {
                session.Dispose();
            }
        }
    }
}