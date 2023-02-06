using IoC.Interfaces;

namespace StellarCombat.Interfaces
{
    public interface ICommandQueue
    {
        int Count { get; }
        
        void Enqueue(ICommand command);
        ICommand Dequeue();
        bool Contains(ICommand command);
        bool Contains(Func<ICommand, bool> func);
    }
}

