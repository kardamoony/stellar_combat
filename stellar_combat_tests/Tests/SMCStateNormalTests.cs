using IoC.Interfaces;
using Moq;
using NUnit.Framework;
using StellarCombat.Interfaces;
using StellarCombat.SMCommander;

[TestFixture]
public class SMCStateRunTests
{
    [Test]
    public void SMCStateRun_IsQueueAlive_ReturnsTrue_IfQueueNotEmpty()
    {
        var queue = new Mock<ICommandQueue>();

        queue.SetupGet(q => q.Count).Returns(1);

        var commander = new SMCommander(queue.Object);
        var state = new SMCStateRun(commander);

        Assert.IsTrue(state.IsQueueAlive);
    }
    
    [Test]
    public void SMCStateRun_IsQueueAlive_ReturnsFalse_IfQueueEmpty()
    {
        var queue = new Mock<ICommandQueue>();

        queue.SetupGet(q => q.Count).Returns(0);

        var commander = new SMCommander(queue.Object);
        var state = new SMCStateRun(commander);

        Assert.IsFalse(state.IsQueueAlive);
    }

    [Test]
    public void SMCStateRun_ExecuteNext_CallsQueue_DequeueMethod()
    {
        var queue = new Mock<ICommandQueue>();
        queue.Setup(q => q.Dequeue()).Returns(Mock.Of<ICommand>()).Verifiable();

        var commander = new SMCommander(queue.Object);
        var state = new SMCStateRun(commander);

        state.ExecuteNext();
        
        queue.Verify();
    }
    
    [Test]
    public void SMCStateRun_ExecuteNext_ExecutesCommand()
    {
        var queue = new Mock<ICommandQueue>();
        var cmd = new Mock<ICommand>();
        cmd.Setup(c => c.Execute()).Verifiable();

        queue.Setup(q => q.Dequeue()).Returns(cmd.Object);

        var commander = new SMCommander(queue.Object);
        var state = new SMCStateRun(commander);

        state.ExecuteNext();
        
        cmd.Verify();
    }
}