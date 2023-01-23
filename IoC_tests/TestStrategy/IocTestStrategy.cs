namespace TestStrategy;

internal static class IocTestStrategy
{
    internal static Dictionary<string, Func<object[], object>> Dependencies = new Dictionary<string, Func<object[], object>>();

    internal static object GetStrategy(string key, params object[] args)
    {
        if (key == "IoC.Register")
        {
            return new RegisterTestDependencyCmd((string)args[0], (Func<object[], object>)args[1]);
        }

        if (key == "IoC.Clear")
        {
            return new ClearTestDependenciesCmd();
        }
        
        if (Dependencies.TryGetValue(key, out var dependency))
        {
            return dependency.Invoke(args);
        }
        
        throw new ArgumentException($"IoC was unable to resolve dependency - {key} not found.");
    }
}