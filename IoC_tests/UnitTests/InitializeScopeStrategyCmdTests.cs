using IoC;
using IoC.Commands;
using IoC.Interfaces;
using Moq;
using NUnit.Framework;

namespace UnitTests;

[TestFixture]
public class InitializeScopeStrategyCmdTests
{
    [SetUp]
    public void BeforeTest()
    {
        new InitializeScopeStrategyCmd().Execute();
    }
    
    public class InitializeScopeStrategyCmdTest_IocRegister : InitializeScopeStrategyCmdTests
    {
        [Test]
        public void Execute_Registers_IoCRegister()
        {
            var dependency = Container.Resolve<ICommand>("IoC.Register", It.IsAny<string>(), It.IsAny<Func<object[], object>>());

            Assert.IsInstanceOf<RegisterScopeDependencyCmd>(dependency);
        }
    }
    
    public class InitializeScopeStrategyCmdTest_IocStrategySetup : InitializeScopeStrategyCmdTests
    {
        [Test]
        public void Execute_Registers_IoCStrategySetup()
        {
            var dependency = Container.Resolve<ICommand>("IoC.Strategy.Setup", It.IsAny<Func<string, object[], object>>());

            Assert.IsInstanceOf<SetupContainerResolveStrategyCmd>(dependency);
        }
    }
    
    public class InitializeScopeStrategyCmdTest_ScopesSetNew : InitializeScopeStrategyCmdTests
    {
        [Test]
        public void Execute_Registers_IoCStrategySetup()
        {
            var dependency = Container.Resolve<ICommand>("Scopes.Set.New", It.IsAny<string>(), It.IsAny<IDictionary<string, Func<object[], object>>>());

            Assert.IsInstanceOf<SetNewScopeCmd>(dependency);
        }
    }

    public class InitializeScopeStrategyCmdTest_ScopesGetCurrent : InitializeScopeStrategyCmdTests
    {
        [Test]
        public void Execute_Registers_ScopesGetCurrent()
        {
            new InitializeScopeStrategyCmd().Execute();
        
            var dependency = Container.Resolve<IScope>("Scopes.Get.Current");

            Assert.IsInstanceOf<IScope>(dependency);
        }
    }
}