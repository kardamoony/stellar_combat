using StellarCombat.Interfaces;

namespace StellarCombat.SMCommander
{
    public class SMCStateMoveQueue : ICommanderState
    {
        private readonly SMCommander _commander;
        private readonly ICommander _another;

        public bool IsQueueAlive => _commander.Queue.Count > 0;

        public SMCStateMoveQueue(SMCommander commander, ICommander another)
        {
            _commander = commander;
            _another = another;
        }
        
        public void ExecuteNext()
        {
            var command = _commander.Queue.Dequeue();
            _another.Enqueue(command);
        }
    }
}

