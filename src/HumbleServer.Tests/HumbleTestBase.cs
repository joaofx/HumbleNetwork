namespace HumbleNetwork.Tests
{
    using NUnit.Framework;

    public class HumbleTestBase
    {
		protected HumbleServer server;
		
		[SetUp]
		public void Setup()
		{
			this.server = new HumbleServer().Start(987);
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
