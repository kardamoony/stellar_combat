namespace TestStrategy;
using IoC.Interfaces;

public class RegisterTestDependencyCmd : ICommand
{
    private readonly string _key;
    private readonly Func<object[], object> _dependency;
    
    public RegisterTestDependencyCmd(string key, Func<object[], object> dependency)
    {
        _key = key;
        _dependency = dependency;
    }
    
    public void Execute()
    {
        if (!IocTestStrategy.Dependencies.ContainsKey(_key))
        {
            IocTestStrategy.Dependencies.Add(_key, _dependency);
            return;
        }

        IocTestStrategy.Dependencies[_key] = _dependency;
    }
}