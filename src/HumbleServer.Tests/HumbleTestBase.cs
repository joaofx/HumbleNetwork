using NUnit.Framework;

namespace HumbleServer.Tests
{
    using System;
    using System.Text;

    [TestFixture]
    public class HumbleTestBase
    {
		protected NetworkServer server;
		
		[SetUp]
		public void Setup()
		{
			this.server = new NetworkServer().Start(987);
            this.BeforeTest();
		}
		
        protected virtual void BeforeTest()
        {
        }

		[TearDown]
		public void TearDown()
		{
			this.server.Stop();
		}
    }
}
