using Moq;
using NUnit.Framework;
using StellarCombat.Commands;
using StellarCombat.Interfaces;

[TestFixture]
public class CommandSequenceTests
{
    [Test]
    public void CommandSequence_Executes_Single_Command()
    {
        var commandMock = new Mock<ICommand>();
        var hasExecuted = false;

        commandMock.Setup(cmd => cmd.Execute()).Callback(() => hasExecuted = true);
        var sequence = new CommandSequence(commandMock.Object);
        sequence.Execute();
        
        Assert.IsTrue(hasExecuted);
    }
    
    [Test]
    public void CommandSequence_Executes_Multiple_Commands()
    {
        var commandsCount = 3;
        var commands = new ICommand[commandsCount];
        bool? hasExecuted = null;

        for (var i = 0; i < commandsCount; i++)
        {
            var commandMock = new Mock<ICommand>();
            commandMock.Setup(cmd => cmd.Execute()).Callback(() =>
            {
                if (hasExecuted.HasValue && !hasExecuted.Value)
                {
                    return;
                }

                hasExecuted = true;
            });

            commands[i] = commandMock.Object;
        }

        var sequence = new CommandSequence(commands);
        sequence.Execute();
        
        Assert.IsTrue(hasExecuted);
    }

    [Test]
    public void CommandSequence_Throws_Exception()
    {
        var commandMock = new Mock<ICommand>();
        commandMock.Setup(cmd => cmd.Execute()).Throws<Exception>();
        var sequence = new CommandSequence(commandMock.Object);

        Assert.Throws<Exception>(sequence.Execute);
    }

    [Test]
    public void CommandSequence_Stops_Execution_On_Exception()
    {
        var commandMock0 = new Mock<ICommand>();
        var commandMock1 = new Mock<ICommand>();
        var commandMock2 = new Mock<ICommand>();

        var hasExecuted0 = false;
        var hasExecuted2 = false;

        commandMock0.Setup(cmd => cmd.Execute()).Callback(() => hasExecuted0 = true);
        commandMock1.Setup(cmd => cmd.Execute()).Throws<Exception>();
        commandMock2.Setup(cmd => cmd.Execute()).Callback(() => hasExecuted2 = true);
        
        var sequence = new CommandSequence(commandMock0.Object, commandMock1.Object, commandMock2.Object);
        
        try
        {
            sequence.Execute();
        }
        // ReSharper disable once EmptyGeneralCatchClause
        catch (Exception _){}
        
        Assert.IsTrue(hasExecuted0 && !hasExecuted2);
    }
}