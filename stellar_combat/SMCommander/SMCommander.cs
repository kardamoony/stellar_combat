using IoC.Interfaces;
using StellarCombat.Interfaces;

namespace StellarCombat.SMCommander
{
    public class SMCommander : ICommander
    {
        public ICommanderState? CurrentState { get; private set; }
        
        public List<ICommand> PendingCommands { get; } = new List<ICommand>();
        public ICommandQueue Queue { get; }
        
        public SMCommander(ICommandQueue queue)
        {
            Queue = queue;
        }

        public SMCommander TransitionTo(ICommanderState state)
        {
            CurrentState = state;
            return this;
        }
        
        public void ExecuteNext()
        {
            CurrentState.ExecuteNext();
        }

        public void ExecuteAll()
        {
            while (CurrentState.IsQueueAlive)
            {
                ExecuteNext();
            }
        }

        public void Enqueue(params ICommand[] args)
        {
            foreach (var cmd in args)
            {
                Queue.Enqueue(cmd);
            }
        }

        public void SetQueueLifetimeRule(Func<bool> func){}
    }
}

