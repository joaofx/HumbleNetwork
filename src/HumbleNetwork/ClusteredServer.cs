namespace HumbleNetwork
{
    using System.Collections.Generic;

    public class ClusteredServer : HumbleServer
    {
        private readonly IList<Server> servers;
        private bool active;

        public ClusteredServer(params string[] servers)
        {
            this.servers = Server.Parse(servers);
        }

        public override HumbleServer Start(int port)
        {
            this.active = this.PortShouldBeActive(port);
            return base.Start(port);
        }

        private bool PortShouldBeActive(int port)
        {
            return this.servers[0].Port == port;
        }
    }
}