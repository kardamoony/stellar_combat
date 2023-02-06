using IoC.Interfaces;
using Moq;
using NUnit.Framework;
using StellarCombat.Commands;

[TestFixture]
public class HardStopTests : StopCommandTests
{
    [TestCase(3)]
    public void HardStop_ExecuteFirst_StopsQueueImmediately(int commandsCount)
    {
        Commander.Enqueue(new HardStop(Commander));
        
        for (var i = 0; i < commandsCount; i++)
        {
            Commander.Enqueue(Mock.Of<ICommand>());
        }

        Commander.ExecuteAll();
        
        Assert.IsTrue(Commander.Queue.Count == commandsCount);
    }
    
    [TestCase(2, 3)]
    public void HardStop_Execute_StopsQueueImmediately(int commandsBefore, int commandsAfter)
    {
        for (var i = 0; i < commandsBefore; i++)
        {
            Commander.Enqueue(Mock.Of<ICommand>());
        }
        
        Commander.Enqueue(new HardStop(Commander));
        
        for (var i = 0; i < commandsAfter; i++)
        {
            Commander.Enqueue(Mock.Of<ICommand>());
        }

        Commander.ExecuteAll();
        
        Assert.IsTrue(Commander.Queue.Count == commandsAfter);
    }
}