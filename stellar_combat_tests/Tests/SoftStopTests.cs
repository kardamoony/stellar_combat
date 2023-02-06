using IoC.Interfaces;
using Moq;
using NUnit.Framework;
using StellarCombat.Commands;

[TestFixture]
public class SoftStopTests : StopCommandTests
{
    [TestCase(3)]
    public void SoftStop_ExecuteFirst_LeavesEmptyQueue(int commandsCount)
    {
        Commander.Enqueue(new SoftStop(Commander));
        
        for (var i = 0; i < commandsCount; i++)
        {
            Commander.Enqueue(Mock.Of<ICommand>());
        }

        Commander.ExecuteAll();
        
        Assert.IsTrue(Commander.Queue.Count < 1);
    }

    [TestCase(3)]
    public void SoftStop_ExecuteLast_LeavesEmptyQueue(int commandsCount)
    {
        for (var i = 0; i < commandsCount; i++)
        {
            Commander.Enqueue(Mock.Of<ICommand>());
        }
        
        Commander.Enqueue(new SoftStop(Commander));
        
        Commander.ExecuteAll();
        
        Assert.IsTrue(Commander.Queue.Count < 1);
    }
    
    [TestCase(3)]
    public void SoftStop_ExecuteFirst_LetsQueueToExecute(int commandsCount)
    {
        Commander.Enqueue(new SoftStop(Commander));

        var commands = new Mock<ICommand>[commandsCount];
        
        for (var i = 0; i < commandsCount; i++)
        {
            commands[i] = new Mock<ICommand>();
            commands[i].Setup(c => c.Execute()).Verifiable();
            Commander.Enqueue(commands[i].Object);
        }

        Commander.ExecuteAll();

        foreach (var cmd in commands)
        {
            cmd.Verify();
        }
    }
}

