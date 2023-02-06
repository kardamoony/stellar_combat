using IoC.Interfaces;
using StellarCombat.Interfaces;

namespace StellarCombat.Commands
{
    public class StartCommanderThread : ICommand
    {
        private readonly ICommander _commander;
        public Thread WorkingThread { get; private set; }
        
        public StartCommanderThread(ICommander commander)
        {
            _commander = commander;
        }
        
        public void Execute()
        {
            WorkingThread = new Thread(() => _commander.ExecuteAll());
            WorkingThread.Start();
        }
    }
}

