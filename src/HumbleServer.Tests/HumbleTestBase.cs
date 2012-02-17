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

        public static string GenerateRandomString(int lenght)
        {
            var randomString = new StringBuilder();
            var randomNumber = new Random();

            for (var i = 0; i < lenght; ++i)
            {
                var appendedChar = Convert.ToChar(Convert.ToInt32(26 * randomNumber.NextDouble()) + 65);
                randomString.Append(appendedChar);
            }

            return randomString.ToString();
        }
    }
}
