namespace HumbleNetwork
{
    using System.Collections.Generic;
    using System.Net.Sockets;

    public class Sessions
    {
        private readonly List<Session> sessions = new List<Session>();
        private readonly object locker = new object();

        public Session NewSession(HumbleServer humbleServer, TcpClient client, Framing framing, string delimiter)
        {
            var session = new Session(this, humbleServer, client, framing, delimiter);
            
            lock (this.locker)
            {
                this.sessions.Add(session);
            }

            return session;
        }

        public void Disposed(Session session)
        {
            lock (this.locker)
            {
                this.sessions.Remove(session);
            }
        }

        public void DisposeAllSessions()
        {
            var sessionsCopy = this.sessions.ToArray();

            foreach (var session in sessionsCopy)
            {
                session.Dispose();
            }
        }
    }
}