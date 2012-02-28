
namespace HumbleServer.Tests.Streams
{
    using Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class FixedLengthStreamTest : StreamTestBase
    {
        [Test]
        public void Should_send_and_receive_message()
        {
            this.StreamTest((sender, receiver) =>
            {
                sender.Send("hello");
                Assert.That(receiver.Receive(), Is.EqualTo("hello"));
            });
        }

        [Test]
        public void Should_send_and_receive_batch_of_messages()
        {
            this.StreamTest((sender, receiver) =>
            {
                sender.Send("hello");
                sender.Send("wonderful");
                sender.Send("world");
                Assert.That(receiver.Receive(), Is.EqualTo("hello"));
                Assert.That(receiver.Receive(), Is.EqualTo("wonderful"));
                Assert.That(receiver.Receive(), Is.EqualTo("world"));
            });
        }

        [Test]
        public void Should_send_and_receive_big_message()
        {
            this.StreamTest((sender, receiver) =>
            {
                var message = StringExtension.GenerateRandomString(1 << 16);
                sender.Send(message);
                Assert.That(receiver.Receive(), Is.EqualTo(message));
            });
        }
    }
}
