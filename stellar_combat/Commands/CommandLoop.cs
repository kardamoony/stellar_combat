using IoC.Interfaces;

namespace StellarCombat.Commands
{
    //Repeat command for "Command" assignment (lecture #8)
    public class CommandLoop : ICommand
    {
        private readonly Commander _commander;
        private readonly ICommand[] _commands;

        public CommandLoop(Commander commander, params ICommand[] args)
        {
            _commander = commander;
            _commands = args;
        }
        
        public void Execute()
        {
            var commands = new ICommand[_commands.Length + 1];

            for (var i = 0; i < _commands.Length; i++)
            {
                commands[i] = _commands[i];
            }

            commands[_commands.Length] = this;
            
            _commander.Enqueue(commands);
        }
    }
}

