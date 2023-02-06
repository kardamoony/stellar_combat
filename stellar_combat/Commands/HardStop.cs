using IoC.Interfaces;
using StellarCombat.Interfaces;

namespace StellarCombat.Commands
{
    public class HardStop : ICommand
    {
        private readonly ICommander _commander;

        public HardStop(ICommander commander)
        {
            _commander = commander;
        }
        
        public void Execute()
        {
            _commander.SetQueueLifetimeRule(() => false);
        }
    }
}

