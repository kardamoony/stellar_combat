using System.Collections.Concurrent;
using IoC.Interfaces;
using StellarCombat.Interfaces;

namespace StellarCombat.CommandQueues
{
    public class ConcurrentCommandQueue : ICommandQueue
    {
        private readonly ConcurrentQueue<ICommand> _queue = new ConcurrentQueue<ICommand>();
        
        public int Count => _queue.Count;
        
        public void Enqueue(ICommand command)
        {
            _queue.Enqueue(command);
        }

        public ICommand Dequeue()
        {
            _queue.TryDequeue(out var command);
            return command;
        }

        public bool Contains(ICommand command)
        {
            return _queue.Contains(command);
        }

        public bool Contains(Func<ICommand, bool> func)
        {
            return _queue.FirstOrDefault(func.Invoke) != null;
        }
    }
}

