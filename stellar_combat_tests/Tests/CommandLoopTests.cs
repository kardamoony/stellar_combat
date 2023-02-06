using IoC.Interfaces;
using Moq;
using NUnit.Framework;
using StellarCombat;
using StellarCombat.Commands;
using StellarCombat.Interfaces;

[TestFixture]
public class CommandLoopTests
{
    [Test]
    public void CommandLoop_ExecuteAll_Queues_NewLoop()
    {
        var handler = Mock.Of<ICommandExceptionHandler>();
        var commander = new Commander(handler);
        var command = Mock.Of<ICommand>();
        
        var loop = new CommandLoop(commander, command);
        
        commander.Enqueue(loop);
        commander.ExecuteAll();

        var isLoopQueued = commander.IsInQueue(cmd => cmd is CommandLoop);

        Assert.IsTrue(isLoopQueued);
    }
}