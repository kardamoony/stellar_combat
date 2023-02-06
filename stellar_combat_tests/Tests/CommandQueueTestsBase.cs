using IoC.Interfaces;
using Moq;
using NUnit.Framework;
using StellarCombat.Interfaces;

public abstract class CommandQueueTestsBase
{
    protected ICommandQueue CommandQueue;
    
    [Test]
    public void CommandQueue_Enqueue_EnqueuesCommand()
    {
        var cmd = Mock.Of<ICommand>();
        CommandQueue.Enqueue(cmd);
        Assert.IsTrue(CommandQueue.Contains(cmd));
    }

    [Test]
    public void CommandQueue_Dequeue_DequeuedCommand()
    {
        var cmd = Mock.Of<ICommand>();
        CommandQueue.Enqueue(cmd);
        var dequeuedCmd = CommandQueue.Dequeue();
        
        Assert.IsTrue(cmd.Equals(dequeuedCmd) && !CommandQueue.Contains(cmd));
    }
}