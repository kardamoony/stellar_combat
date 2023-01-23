using IoC.Interfaces;

namespace IoC.Commands
{
    public class InitializeStrategyCmd : ICommand
    {
        private readonly Func<string, object[], object> _strategy;

        public InitializeStrategyCmd(Func<string, object[], object> strategy)
        {
            _strategy = strategy;
        }

        public void Execute()
        {
            Container.ResolveStrategy = _strategy;
        }
    }
}