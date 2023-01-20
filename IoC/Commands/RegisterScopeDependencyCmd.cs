using IoC.Interfaces;

namespace IoC.Commands
{
    public class RegisterScopeDependencyCmd : ICommand
    {
        private readonly IScope _scope;
        private readonly string _key;
        private readonly Func<object[], object> _dependency;
        
        public RegisterScopeDependencyCmd(string key, Func<object[], object> dependency, IScope scope)
        {
            _key = key;
            _dependency = dependency;
            _scope = scope;
        }
        
        public void Execute()
        {
            if (!_scope.TryAdd(_key, _dependency))
            {
                throw new ArgumentException($"[{GetType().Name}] tried to register duplicate dependency with key={_key} in scope {_scope.Id}");
            }
        }
    }
}

