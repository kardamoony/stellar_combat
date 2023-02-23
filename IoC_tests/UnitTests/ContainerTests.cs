using IoC;
using IoC.Commands;
using IoC.Interfaces;
using NUnit.Framework;

namespace UnitTests;

[TestFixture]
public class ContainerTests
{
    [SetUp]
    public void BeforeTest()
    {
        new SetupContainerResolveStrategyCmd(Container.GetDefaultStrategy).Execute();
    }
    
    private static object TestStrategy(string key, params object[] args)
    {
        return "TestStrategy";
    }

    [Test]
    public void Container_Resolve_UnknownDependency_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => Container.Resolve<object>("TestKey"));
    }
}