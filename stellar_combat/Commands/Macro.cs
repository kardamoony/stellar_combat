﻿using IoC.Interfaces;
using StellarCombat.Interfaces;

namespace StellarCombat.Commands
{
    //Macro command for "SOLID and Exception Hanlding" assignment (lecture #7)
    public class Macro : ICommand
    {
        private readonly ICommander _commander;
        private readonly IEnumerable<ICommand> _commands;

        public Macro(ICommander commander, params ICommand[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(paramName: nameof(args));
            }

            if (args.Length < 1)
            {
                throw new ArgumentException("Arguments can't be empty", nameof(args));
            }
            
            _commander = commander;
            _commands = args;
        }

        public void Execute()
        {
            foreach (var cmd in _commands)
            {
                _commander.Enqueue(cmd);
            }
        }
    }
}

