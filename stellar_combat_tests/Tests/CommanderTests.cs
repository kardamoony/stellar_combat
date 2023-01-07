using NUnit.Framework;
using Moq;
using StellarCombat;
using StellarCombat.Interfaces;

[TestFixture]
public class CommanderTests
{
    [Test]
    public void Commander_Enqueue_Enqueues_Command()
    {
        var handler = Mock.Of<ICommandExceptionHandler>();
        var commander = new Commander(handler);

        var command = Mock.Of<ICommand>();
        commander.Enqueue(command);
        
        Assert.IsTrue(commander.IsInQueue(command));
    }

    [Test]
    public void Commander_ExecuteNext_Dequeues_Command()
    {
        var handler = Mock.Of<ICommandExceptionHandler>();
        var commander = new Commander(handler);

        var command = Mock.Of<ICommand>();
        commander.Enqueue(command);
        
        commander.ExecuteNext();
        Assert.IsFalse(commander.IsInQueue(command));
    }

    [Test]
    public void Commander_ExecuteNext_Dequeues_One_Command_Only()
    {
        var handler = Mock.Of<ICommandExceptionHandler>();
        var commander = new Commander(handler);

        var command0 = Mock.Of<ICommand>();
        commander.Enqueue(command0);
        
        var command1 = Mock.Of<ICommand>();
        commander.Enqueue(command1);
        
        commander.ExecuteNext();

        var command0IsInQueue = commander.IsInQueue(command0);
        var command1IsInQueue = commander.IsInQueue(command1);
        
        Assert.IsTrue(!command0IsInQueue && command1IsInQueue);
    }
    
    [Test]
    public void Commander_ExecuteAll_Dequeues_All_Commands()
    {
        var handler = Mock.Of<ICommandExceptionHandler>();
        var commander = new Commander(handler);

        var command0 = Mock.Of<ICommand>();
        commander.Enqueue(command0);
        
        var command1 = Mock.Of<ICommand>();
        commander.Enqueue(command1);
        
        commander.ExecuteAll();

        var command0IsInQueue = commander.IsInQueue(command0);
        var command1IsInQueue = commander.IsInQueue(command1);
        
        Assert.IsFalse(command0IsInQueue && command1IsInQueue);
    }
}