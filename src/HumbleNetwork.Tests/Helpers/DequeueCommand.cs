namespace HumbleNetwork.Tests.Helpers
{
    using System.Collections.Generic;

    public class DequeueCommand : ICommand
    {
        private readonly Queue<int> queue;

        public DequeueCommand(Queue<int> queue)
        {
            this.queue = queue;
        }

        public void Execute(IHumbleStream stream)
        {
            stream.Send(this.queue.Dequeue().ToString());
        }
    }
}