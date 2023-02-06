using IoC.Interfaces;
using StellarCombat.Interfaces;

namespace StellarCombat.CommandQueues
{
    public class CommandQueue : ICommandQueue
    {
        private readonly Queue<ICommand> _queue = new Queue<ICommand>();

        public int Count => _queue.Count;
        
        public void Enqueue(ICommand command)
        {
            _queue.Enqueue(command);
        }

        public ICommand Dequeue()
        {
            return _queue.Dequeue();
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