using IoC.Interfaces;

namespace IoC.Commands
{
    public class SetupContainerResolveStrategyCmd : ICommand
    {
        private readonly Func<string, object[], object> _strategy;
        
        public SetupContainerResolveStrategyCmd(Func<string, object[], object> strategy)
        {
            _strategy = strategy;
        }
        
        public void Execute()
        {
            Container.ResolveStrategy = _strategy;
        }
    }
}

