using NUnit.Framework;
using Moq;
using StellarCombat;
using StellarCombat.Commands;
using StellarCombat.Interfaces;
using StellarCombat.ExceptionHandling.Handlers;

[TestFixture]
public class CommanderExceptionHandlerTests
{
    [Test]
    public void Handler_Saves_Single_Exception_Occurence()
    {
        var testedHandler = new CommanderExceptionHandler(new StringWriter());
        var commander = new Commander(testedHandler);

        var commandMock = new Mock<ICommand>();
        commandMock.Setup(cmd => cmd.Execute()).Throws<Exception>();

        commander.Enqueue(commandMock.Object);
        commander.ExecuteNext();
        
        Assert.IsTrue(testedHandler.HasExceptionOccured(typeof(Exception), out _));
        
        testedHandler.Dispose();
    }
    
    [Test]
    public void Handler_Saves_Multiple_Exception_Occurence()
    {
        var occurences = 5;
        
        var testedHandler = new CommanderExceptionHandler(new StringWriter(), occurences + 1);
        var commander = new Commander(testedHandler);

        var commands = new ICommand[occurences];

        for (var i = 0; i < occurences; i++)
        {
            var commandMock = new Mock<ICommand>();
            commandMock.Setup(cmd => cmd.Execute()).Throws<Exception>();
            commands[i] = commandMock.Object;
        }

        commander.Enqueue(commands);
        
        for (var i = 0; i < occurences; i++)
        {
            commander.ExecuteNext();
        }
        
        Assert.IsTrue(testedHandler.HasExceptionOccured(typeof(Exception), out _));
        
        testedHandler.Dispose();
    }
    
    [Test]
    public void Handler_Single_Exception_Occurence_Returns_1()
    {
        var testedHandler = new CommanderExceptionHandler(new StringWriter());
        var commander = new Commander(testedHandler);

        var commandMock = new Mock<ICommand>();
        commandMock.Setup(cmd => cmd.Execute()).Throws<Exception>();

        commander.Enqueue(commandMock.Object);
        commander.ExecuteNext();

        testedHandler.HasExceptionOccured(typeof(Exception), out var occurrences);

        Assert.AreEqual(1, occurrences);
        
        testedHandler.Dispose();
    }
    
    [Test]
    public void Handler_Multiple_Exception_Returns_Correct_Occurence_Count()
    {
        var expectedOccurences = 5;
        
        var testedHandler = new CommanderExceptionHandler(new StringWriter(), expectedOccurences + 1);
        var commander = new Commander(testedHandler);

        var commands = new ICommand[expectedOccurences];

        for (var i = 0; i < expectedOccurences; i++)
        {
            var commandMock = new Mock<ICommand>();
            commandMock.Setup(cmd => cmd.Execute()).Throws<Exception>();
            commands[i] = commandMock.Object;
        }

        commander.Enqueue(commands);
        
        for (var i = 0; i < expectedOccurences; i++)
        {
            commander.ExecuteNext();
        }

        testedHandler.HasExceptionOccured(typeof(Exception), out var occurrences);
        
        Assert.AreEqual(expectedOccurences, occurrences);
        
        testedHandler.Dispose();
    }

    [Test]
    public void Handling_Single_Exception_Repeats_Command()
    {
        var testedHandler = new CommanderExceptionHandler(new StringWriter());
        var commander = new Commander(testedHandler);
        
        var commandMock = new Mock<ICommand>();
        commandMock.Setup(cmd => cmd.Execute()).Throws<Exception>();
        
        commander.Enqueue(commandMock.Object);
        
        commander.ExecuteNext(); 
        commander.ExecuteNext();

        Assert.IsTrue(commander.IsInQueue(commandMock.Object));

        testedHandler.Dispose();
    }

    [Test]
    public void Handling_Repeated_Exceptions_Queues_Write_To_Log()
    {
        var testedHandler = new CommanderExceptionHandler(new StringWriter(), 1);
        var commander = new Commander(testedHandler);
        
        var commandMock = new Mock<ICommand>();
        commandMock.Setup(cmd => cmd.Execute()).Throws<Exception>();
        commander.Enqueue(commandMock.Object);

        commander.ExecuteNext();
        commander.ExecuteNext();
        commander.ExecuteNext();
        commander.ExecuteNext();

        Assert.IsTrue(commander.IsInQueue(cmd => cmd is WriteLineLog));
        testedHandler.Dispose();
    }
}