using IoC.Interfaces;

namespace IoC.Strategies
{
    internal static class ScopeStrategy
    {
        internal static IScope RootScope;
        internal static ThreadLocal<IScope> CurrentScope = new ThreadLocal<IScope>();

        internal static object GetStrategy(string key, params object[] args)
        {
            if (CurrentScope.Value == null)
            {
                CurrentScope.Value = RootScope;
            }
            
            return CurrentScope.Value.Resolve(key, args);
        }
    }
}

