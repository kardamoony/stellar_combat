using IoC;
using IoC.CodeGeneration;
using IoC.Commands;
using NUnit.Framework;
using TestStrategy;
using ICommand = IoC.Interfaces.ICommand;

namespace UnitTests;

[TestFixture]
public class AdapterGenerationTests
{
    public interface ITestInterface
    {
        int IntProperty { get; set; }
        void TestMethod();
        void TestMethodWithArgs(object arg);
    }
    
    public class TestAdaptee
    {
        public int IntProperty { get; set; }
    }

    [SetUp]
    public void BeforeTest()
    {
        new InitializeStrategyCmd(IocTestStrategy.GetStrategy).Execute();
        Container.Resolve<ICommand>("IoC.Clear").Execute();
    }

    [Test]
    public void GeneratedAdapter_ImplementsInterface()
    {
        var adapterType = AdapterGenerator.GenerateAdapterType<ITestInterface>();
        var implementedInterface = adapterType.GetInterface(nameof(ITestInterface));
        Assert.IsTrue(implementedInterface == typeof(ITestInterface));
    }

    [Test]
    public void GeneratedAdapter_CanBeInstantiated()
    {
        var adapterType = AdapterGenerator.GenerateAdapterType<ITestInterface>();
        var adapter = Activator.CreateInstance(adapterType, (object)"Adaptee");
        Assert.IsTrue(adapter != null);
    }

    [TestCase(10)]
    public void GeneratedAdapter_PropertyGetter_ReturnsValue(int value)
    {
        var adaptee = new TestAdaptee{ IntProperty = value };
    
        Func<object[], object> getter = (args) => ((TestAdaptee)args[0]).IntProperty;
        Container.Resolve<ICommand>("IoC.Register", "ITestInterface.IntProperty.Get", getter).Execute();
    
        var adapterType = AdapterGenerator.GenerateAdapterType<ITestInterface>();
        var adapter = Activator.CreateInstance(adapterType, adaptee) as ITestInterface;
        Assert.AreEqual(value, adapter.IntProperty);
    }

    [TestCase(11)]
    public void GeneratedAdapter_PropertySetter_SetsValue(int value)
    {
        var adaptee = new TestAdaptee{IntProperty = value - 1};
        
        Func<object[], object> setCmd = (args) => new SetValueCmd<int>((v) => ((TestAdaptee)args[0]).IntProperty = v, (int)args[1]);

        Container.Resolve<ICommand>("IoC.Register", "ITestInterface.IntProperty.Set", setCmd).Execute();

        var adapterType = AdapterGenerator.GenerateAdapterType<ITestInterface>();
        var adapter = Activator.CreateInstance(adapterType, adaptee) as ITestInterface;

        adapter.IntProperty = value;
        Assert.AreEqual(value, adaptee.IntProperty);
    }

    [Test]
    public void GeneratedAdapter_MethodInvoke_ThrowsException()
    {
        var adaptee = new TestAdaptee();
        var adapterType = AdapterGenerator.GenerateAdapterType<ITestInterface>();
        var adapter = Activator.CreateInstance(adapterType, adaptee) as ITestInterface;

        Assert.Throws<NotImplementedException>(() => adapter.TestMethod());
    }
    
    [Test]
    public void GeneratedAdapter_MethodInvokeWithArgs_ThrowsException()
    {
        var adaptee = new TestAdaptee();
        var adapterType = AdapterGenerator.GenerateAdapterType<ITestInterface>();
        var adapter = Activator.CreateInstance(adapterType, adaptee) as ITestInterface;

        Assert.Throws<NotImplementedException>(() => adapter.TestMethodWithArgs(null));
    }

    [Test]
    public void Generate_NotAnInterface_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => AdapterGenerator.GenerateAdapterType<int>());
    }
}