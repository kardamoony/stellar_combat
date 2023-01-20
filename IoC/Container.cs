using IoC.Commands;

namespace IoC
{
    public static class Container
    {
        public const string IoC_SetupStrategyKey = "IoC.Strategy.Setup";

        public static Func<string, object[], object> ResolveStrategy { get; internal set; } = GetDefaultStrategy;

        public static T Resolve<T>(string key, params object[] args)
        {
            return (T)ResolveStrategy(key, args);
        }

        public static object GetDefaultStrategy(string key, params object[] args)
        {
            if (key == IoC_SetupStrategyKey)
            {
                return new SetupContainerResolveStrategyCmd((Func<string, object[], object>)args[0]);
            }
            
            throw new ArgumentException($"[{nameof(Container)}] dependency {key} not found.");
        }
    }
}

