namespace StellarCombat.Interfaces
{
    public interface ICommandExceptionHandler
    {
        Commander Commander { set; }
        void Handle(ICommand command, Exception exception);
    }
}

