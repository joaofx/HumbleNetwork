namespace HumbleNetwork.Tests
{
    using NUnit.Framework;

    public class HumbleTestBase
    {
        protected HumbleServer Server;

        [SetUp]
        public void Setup()
        {
            Server = new HumbleServer().Start(0);
            BeforeTest();  
        }

        [TearDown]
        public void TearDown()
        {
            Server.Stop();
        }

        protected virtual void BeforeTest()
        {
        }
    }
}
