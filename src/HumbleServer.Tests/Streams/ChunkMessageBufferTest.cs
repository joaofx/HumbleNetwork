
namespace HumbleServer.Tests.Streams
{
    using HumbleServer.Streams;
    using NUnit.Framework;

    [TestFixture]
    public class ChunkMessageBufferTest
    {
        [Test]
        public void Should_return_if_buffer_has_complete_message()
        {
            var buffer = new ChunkMessageBuffer(";");
            Assert.That(buffer.HasCompleteMessage, Is.False);

            buffer.Append("message1");
            Assert.That(buffer.HasCompleteMessage, Is.False);

            buffer.Append(";");
            Assert.That(buffer.HasCompleteMessage, Is.True);
        }

        [Test]
        public void Should_return_messages_while_has_messages_in_buffer()
        {
            var buffer = new ChunkMessageBuffer(";");

            Assert.That(buffer.GetMessage(), Is.Empty);

            buffer.Append("message1;message2;message3;");

            Assert.That(buffer.GetMessage(), Is.EqualTo("message1"));
            Assert.That(buffer.GetMessage(), Is.EqualTo("message2"));
            Assert.That(buffer.GetMessage(), Is.EqualTo("message3"));
            Assert.That(buffer.GetMessage(), Is.Empty);
        }

        [Test]
        public void Should_handle_big_delimiter()
        {
            var buffer = new ChunkMessageBuffer("\r\n");

            Assert.That(buffer.GetMessage(), Is.Empty);

            buffer.Append("message1\r\nmessage2\r\n");

            Assert.That(buffer.GetMessage(), Is.EqualTo("message1"));
            Assert.That(buffer.GetMessage(), Is.EqualTo("message2"));
            Assert.That(buffer.GetMessage(), Is.Empty);
        }

        [Test]
        public void Should_return_messages_while_messages_are_being_appended()
        {
            var buffer = new ChunkMessageBuffer(";");

            Assert.That(buffer.GetMessage(), Is.Empty);

            buffer.Append("message1;mess");
            Assert.That(buffer.GetMessage(), Is.EqualTo("message1"));

            buffer.Append("age2;me");
            Assert.That(buffer.GetMessage(), Is.EqualTo("message2"));

            buffer.Append("ssage3;");
            Assert.That(buffer.GetMessage(), Is.EqualTo("message3"));

            Assert.That(buffer.GetMessage(), Is.Empty);
        }
    }
}
