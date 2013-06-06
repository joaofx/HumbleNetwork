namespace HumbleNetwork.Tests
{
    using NUnit.Framework;

    public class HumbleTestBase
    {
        protected HumbleServer server;

        [SetUp]
        public void Setup()
        {
            this.server = new HumbleServer().Start(0);
            this.BeforeTest();  
        }

        [TearDown]
        public void TearDown()
        {
            this.server.Stop();
        }

        protected virtual void BeforeTest()
        {
        }
    }
}
