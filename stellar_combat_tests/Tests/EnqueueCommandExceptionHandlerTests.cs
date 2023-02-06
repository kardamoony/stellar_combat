using IoC.Interfaces;
using NUnit.Framework;
using Moq;
using StellarCombat;
using StellarCombat.Interfaces;
using StellarCombat.ExceptionHandling.Handlers;

[TestFixture]
public class EnqueueCommandExceptionHandlerTests
{
    [Test]
    public void Exception_Repeats_Command()
    {
        var testedHandler = new EnqueueCommandExceptionHandler();
        var commander = new Commander(testedHandler);

        var commandMock = new Mock<ICommand>();
        commandMock.Setup(cmd => cmd.Execute()).Throws<Exception>();

        commander.Enqueue(commandMock.Object);
        commander.ExecuteNext();
        commander.ExecuteNext();
        
        Assert.IsTrue(commander.IsInQueue(commandMock.Object));
    }
}