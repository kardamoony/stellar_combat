using IoC.Interfaces;

namespace TestStrategy;

public class ClearTestDependenciesCmd : ICommand
{
    public void Execute()
    {
        IocTestStrategy.Dependencies.Clear();
    }
}