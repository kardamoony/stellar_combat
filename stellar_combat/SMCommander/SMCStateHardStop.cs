using StellarCombat.Interfaces;

namespace StellarCombat.SMCommander
{
    public class SMCStateHardStop : ICommanderState
    {
        public bool IsQueueAlive => false;
        
        public void ExecuteNext(){}
    }
}

