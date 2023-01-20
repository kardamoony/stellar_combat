namespace IoC.Interfaces
{
    public interface IScope
    {
        IScope? Parent { get; }
        string Id { get; }
        object Resolve(string key, params object[] args);
        bool TryAdd(string key, Func<object[], object> dependency);
    }
}

