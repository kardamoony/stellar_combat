using StellarCombat.Interfaces;

namespace StellarCombat
{
    public sealed class Commander
    {
        private readonly Queue<ICommand> _commandQueue = new Queue<ICommand>();
        private readonly ICommandExceptionHandler _exceptionHandler;

        public Commander(ICommandExceptionHandler exceptionHandler)
        {
            _exceptionHandler = exceptionHandler;
            _exceptionHandler.Commander = this;
        }

        public void ExecuteNext()
        {
            var cmd = _commandQueue.Dequeue();
                
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
            while (_commandQueue.Count > 0)
            {
                ExecuteNext();
            }
        }

        public void Enqueue(params ICommand[] args)
        {
            foreach (var cmd in args)
            {
                _commandQueue.Enqueue(cmd);
            }
        }

        public bool IsInQueue(ICommand command)
        {
            return _commandQueue.Contains(command);
        }
        
        public bool IsInQueue(Func<ICommand, bool> func)
        {
            return _commandQueue.FirstOrDefault(func.Invoke) != null;
        }
    }
}

