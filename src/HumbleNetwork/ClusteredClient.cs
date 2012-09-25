namespace HumbleNetwork
{
    using System.Collections.Generic;

    public class ClusteredClient : HumbleClient
    {
        private readonly IList<Server> servers;

        public ClusteredClient(params string[] serversAddress)
        {
            this.servers = Server.Parse(serversAddress);
        }
        
        public HumbleClient Connect()
        {
            var server = this.servers[0];
            return this.Connect(server.Host, server.Port);
        }
    }
}