namespace HumbleNetwork
{
    using System;
    using System.Collections.Generic;

    public class Server
    {
        public Server(string serverAddress) : this(false, serverAddress)
        {   
        }

        private Server(bool active, string serverAddress)
        {
            var splited = serverAddress.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            
            this.Host = splited[0];
            this.Port = Convert.ToInt32(splited[1]);
            this.Active = active;
        }

        public bool Active
        {
            get;
            private set;
        }

        public int Port
        {
            get;
            private set;
        }

        public string Host
        {
            get;
            private set;
        }

        public static IList<Server> Parse(params string[] serversAddress)
        {
            var servers = new List<Server>();

            foreach (var serverAddress in serversAddress)
            {
                servers.Add(servers.Count == 0 ? new Server(true, serverAddress) : new Server(serverAddress));
            }

            return servers;
        }
    }
}
