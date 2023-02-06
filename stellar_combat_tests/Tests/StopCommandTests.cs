using Moq;
using NUnit.Framework;
using StellarCombat;
using StellarCombat.Interfaces;

public abstract class StopCommandTests
{
    protected Commander Commander;

    [SetUp]
    public void BeforeTest()
    {
        var handlerMock = Mock.Of<ICommandExceptionHandler>();
        Commander = new Commander(handlerMock);
    }
}