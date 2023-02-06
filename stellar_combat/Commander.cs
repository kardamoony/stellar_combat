using IoC.Interfaces;
using StellarCombat.CommandQueues;
using StellarCombat.Interfaces;

namespace StellarCombat
{
    public sealed class Commander : ICommander
    {
        private readonly ICommandExceptionHandler _exceptionHandler;
        private bool _executingQueue;
        
        private Func<bool> _queueLifetimeRule;

        public ICommandQueue Queue { get; }
        public List<ICommand> PendingCommands { get; } = new List<ICommand>();
        
        public Commander(ICommandExceptionHandler exceptionHandler)
        {
            Queue = new CommandQueue();
            _exceptionHandler = exceptionHandler;
            _exceptionHandler.Commander = this;
        }

        public Commander(ICommandExceptionHandler exceptionHandler, ICommandQueue queue)
        {
            Queue = queue;
            _exceptionHandler = exceptionHandler;
            _exceptionHandler.Commander = this;
        }

        public void ExecuteNext()
        {
            var cmd = Queue.Dequeue();
                
            try
            {
                cmd.Execute();
            }
            catch (Exception exception)
            {
                _exceptionHandler.Handle(cmd, exception);
            }
        }

        public void ExecuteAll()
        {
            if (_executingQueue) return;
            _executingQueue = true;

            SetQueueLifetimeRule(DefaultQueueLifetimeRule);
            
            while (_queueLifetimeRule.Invoke())
            {
                ExecuteNext();
            }

            EnqueuePending();
            _executingQueue = false;
        }

        public void Enqueue(params ICommand[] args)
        {
            if (_executingQueue)
            {
                PendingCommands.AddRange(args);
                return;
            }
            
            foreach (var cmd in args)
            {
                Queue.Enqueue(cmd);
            }
        }

        public bool IsInQueue(ICommand command)
        {
            return Queue.Contains(command);
        }
        
        public bool IsInQueue(Func<ICommand, bool> func)
        {
            return Queue.Contains(func);
        }

        public void SetQueueLifetimeRule(Func<bool> func)
        {
            _queueLifetimeRule = func;
        }

        private void EnqueuePending()
        {
            foreach (var cmd in PendingCommands)
            {
                Queue.Enqueue(cmd);
            }
            
            PendingCommands.Clear();
        }

        private bool DefaultQueueLifetimeRule()
        {
            return Queue.Count > 0;
        }
    }
}

