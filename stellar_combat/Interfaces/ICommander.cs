using IoC.Interfaces;

namespace StellarCombat.Interfaces
{
    public interface ICommander
    {
        void ExecuteNext();
        void ExecuteAll();
        void Enqueue(params ICommand[] args);
        void SetQueueLifetimeRule(Func<bool> func);
        List<ICommand> PendingCommands { get; }
        ICommandQueue Queue { get; }
    }
}