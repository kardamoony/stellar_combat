using IoC.Interfaces;

namespace StellarCombat.Interfaces
{
    public interface ICommanderExceptionHandler
    {
        Commander Commander { get; }
        
        void Handle(ICommand command, Exception exception);
    }
}

