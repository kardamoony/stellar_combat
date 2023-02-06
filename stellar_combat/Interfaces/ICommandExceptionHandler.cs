using IoC.Interfaces;

namespace StellarCombat.Interfaces
{
    public interface ICommandExceptionHandler
    {
        ICommander Commander { set; }
        void Handle(ICommand command, Exception exception);
    }
}

