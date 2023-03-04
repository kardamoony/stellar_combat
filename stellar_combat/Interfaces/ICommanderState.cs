namespace StellarCombat.Interfaces
{
    public interface ICommanderState
    {
        bool IsQueueAlive { get; }
        void ExecuteNext();
    }
}

