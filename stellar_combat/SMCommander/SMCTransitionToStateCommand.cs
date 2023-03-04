using IoC.Interfaces;
using StellarCombat.Interfaces;

namespace StellarCombat.SMCommander
{
    public class SMCTransitionToStateCommand : ICommand
    {
        private readonly SMCommander _commander;
        private readonly ICommanderState _state;

        public SMCTransitionToStateCommand(SMCommander commander, ICommanderState state)
        {
            _commander = commander;
            _state = state;
        }
        
        public void Execute()
        {
            _commander.TransitionTo(_state);
        }
    }
}

