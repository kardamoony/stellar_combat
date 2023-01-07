using StellarCombat.Interfaces;

namespace StellarCombat.ExceptionHandling.Exceptions
{
    public class CommandException : Exception
    {
        private readonly string _message;

        public override string Message => _message;
        
        public CommandException(ICommand command)
        {
            _message = $"[{command.GetType().Name}]: execution unsuccessful";
        }
    }
}

