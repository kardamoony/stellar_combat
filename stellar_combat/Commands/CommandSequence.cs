using IoC.Interfaces;

namespace StellarCombat.Commands
{
    //Macro command for "Command" assignment (lecture #8)
    public class CommandSequence : ICommand
    {
        private readonly ICommand[] _commands;
        
        public CommandSequence(params ICommand[] args)
        {
            _commands = args;
        }

        public void Execute()
        {
            foreach (var cmd in _commands)
            {
                try
                {
                    cmd.Execute();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}

