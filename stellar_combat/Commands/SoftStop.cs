using IoC.Interfaces;
using StellarCombat.Interfaces;

namespace StellarCombat.Commands
{
    public class SoftStop : ICommand
    {
        private readonly ICommander _commander;
        
        public SoftStop(ICommander commander)
        {
            _commander = commander;
        }
        
        public void Execute()
        {
            _commander.SetQueueLifetimeRule(() =>
            {
                _commander.PendingCommands.Clear();
                return _commander.Queue.Count > 0;
            });
        }
    }
}