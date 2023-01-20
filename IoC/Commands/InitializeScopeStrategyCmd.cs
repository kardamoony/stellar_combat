using System.Collections.Concurrent;
using IoC.Interfaces;
using IoC.Strategies;

namespace IoC.Commands
{
    public class InitializeScopeStrategyCmd : ICommand
    {
        public void Execute()
        {
            var dependencies = CreateDependencies();
            var defaultScope = new Scope("DefaultScope", dependencies);

            ScopeStrategy.RootScope = defaultScope;
            new SetupContainerResolveStrategyCmd(ScopeStrategy.GetStrategy).Execute();
        }

        private ConcurrentDictionary<string, Func<object[], object>> CreateDependencies()
        {
            var dependencies = new ConcurrentDictionary<string, Func<object[], object>>();
            
            dependencies.TryAdd("IoC.Register", (args) =>
            {
                return new RegisterScopeDependencyCmd((string)args[0], (Func<object[], object>)args[1], Container.Resolve<IScope>("Scopes.Get.Current"));
            });
            
            dependencies.TryAdd(Container.IoC_SetupStrategyKey, (args) =>
            {
                return new SetupContainerResolveStrategyCmd((Func<string, object[], object>)args[0]);
            });

            dependencies.TryAdd("Scopes.Set.New", (args) =>
            {
                return new SetNewScopeCmd((string)args[0], (IDictionary<string, Func<object[], object>>)args[1]);
            });

            dependencies.TryAdd("Scopes.Get.Current", (_) =>
            {
                return ScopeStrategy.CurrentScope.IsValueCreated
                    ? ScopeStrategy.CurrentScope.Value
                    : ScopeStrategy.RootScope;
            });

            return dependencies;
        }
    }
}

