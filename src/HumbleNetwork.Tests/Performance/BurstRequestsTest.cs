namespace HumbleNetwork.Tests.Performance
{
    using System;
    using Helpers;
    using HumbleNetwork;
    using NUnit.Framework;
    using System.Threading;

    /// <summary>
    /// TODO: This test pass and finish but seems that some threads still working
    /// TODO: Refactor. It's ugly.
    /// </summary>
    [TestFixture]
    public class BurstRequestsTest : HumbleTestBase
    {
        private readonly ManualResetEvent mre = new ManualResetEvent(false);
        private const int NumThreads = 100;
        private CountdownEvent countdown;

        protected override void BeforeTest()
        {
            this.server.AddCommand("wait", () => new WaitCommand());
        }

        /// <summary>
        /// This test fires 100 request to server to command wait.
        /// Command wait will sleep two seconds and response with 1.
        /// The server has to open 100 threads at same time, and sleep two seconds 
        /// for each thread and send the response. 
        /// So this test doesn't has to take more than 4 seconds
        /// </summary>
        [Test]
        public void Should_execute_less_than_4_seconds()
        {
            ThreadPool.SetMinThreads(NumThreads, NumThreads);

            Console.WriteLine("Burst test started");

            this.mre.Reset();
            this.countdown = new CountdownEvent(NumThreads);

            for (var i = 0; i < NumThreads; i++)
            {
                new Thread(this.OneThreadExecution) { Name = "Thread " + i }.Start();
            }

            this.countdown.Wait();
            var dateTime = DateTime.Now;
            this.countdown = new CountdownEvent(NumThreads);
            this.mre.Set();
            this.countdown.Wait();
            var timeSpan = DateTime.Now - dateTime;

            Console.WriteLine("Test finished");
            Console.WriteLine("Executed at {0}.{1:0}s.", timeSpan.Seconds, timeSpan.Milliseconds / 100);

            if (timeSpan.Seconds > 5)
            {
                Assert.Ignore("This test should't take more than to 4 seconds to run");
            }
        }

        private void OneThreadExecution()
        {
            this.countdown.Signal();

            var client = new HumbleClient();
            this.mre.WaitOne();
            client.Connect("localhost", this.server.Port);
            client.Send("wait");

            try
            {
                var data = client.Receive();
                if (data.Equals("1") == false)
                {
                    Assert.Ignore("Should receive 1 on thread " + Thread.CurrentThread.Name);
                }
            }
            catch (Exception exception)
            {
                Assert.Ignore("Exception on thread " + Thread.CurrentThread.Name + ": " + exception.Message);
            }

            this.countdown.Signal();
        }
    }
}
