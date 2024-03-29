﻿using IoC.Interfaces;
using StellarCombat.Commands;
using StellarCombat.Interfaces;

namespace StellarCombat.ExceptionHandling.Handlers
{
    public class EnqueueCommandExceptionHandler : ICommandExceptionHandler
    {
        public ICommander Commander { get; set; }

        public void Handle(ICommand command, Exception exception)
        {
            Commander.Enqueue(new Macro(Commander, command));
        }
    }
}

