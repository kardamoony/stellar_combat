using StellarCombat.Interfaces;

namespace StellarCombat.SMCommander
{
    public class SMCStateRun : ICommanderState
    {
        private readonly SMCommander _commander;
        public bool IsQueueAlive => _commander.Queue.Count > 0;

        public SMCStateRun(SMCommander commander)
        {
            _commander = commander;
        }
        
        public void ExecuteNext()
        {
            //irl there should be some error handling 
            //but I left it out intentionally
            //in order not to bloat up the amount of code for review
            
            var command = _commander.Queue.Dequeue();
            command.Execute();
        }
    }
}

