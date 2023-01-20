using IoC.Interfaces;
using IoC.Strategies;

namespace IoC.Commands
{
    public class SetNewScopeCmd : ICommand
    {
        private readonly string _id;
        private readonly IDictionary<string, Func<object[], object>> _dependencies;

        public SetNewScopeCmd(string id, IDictionary<string, Func<object[], object>> dependencies)
        {
            _id = id;
            _dependencies = dependencies;
        }
        
        public void Execute()
        {
            var scope = new Scope(ScopeStrategy.CurrentScope.Value, _id, _dependencies);
            ScopeStrategy.CurrentScope.Value = scope;
        }
    }
}

