using IoC.Interfaces;

namespace IoC
{
    public class Scope : IScope
    {
        private readonly IDictionary<string, Func<object[], object>> _dependencies;
        
        public string Id { get; }
        public IScope? Parent { get; }

        public Scope(string id, IDictionary<string, Func<object[], object>> dependencies)
        {
            Parent = null;
            Id = id;
            _dependencies = dependencies;
        }
        
        public Scope(IScope parent, string id, IDictionary<string, Func<object[], object>> dependencies)
        {
            Parent = parent;
            Id = id;
            _dependencies = dependencies;
        }
        
        public object Resolve(string key, params object[] args)
        {
            if (_dependencies.TryGetValue(key, out var dependency))
            {
                return dependency.Invoke(args);
            }

            if (Parent != null)
            {
                return Parent.Resolve(key, args);
            }

            throw new ArgumentException($"[{GetType().Name}_{Id}] unable to resolve - {key} not found.");
        }

        public bool TryAdd(string key, Func<object[], object> dependency)
        {
            return _dependencies.TryAdd(key, dependency);
        }
    }
}

