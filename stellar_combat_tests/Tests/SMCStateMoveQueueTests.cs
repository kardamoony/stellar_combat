using IoC.Interfaces;
using Moq;
using NUnit.Framework;
using StellarCombat.Interfaces;
using StellarCombat.SMCommander;

[TestFixture]
public class SMCStateMoveQueueTests
{
    [Test]
    public void SMCStateMoveQueue_IsQueueAlive_ReturnsTrue_IfQueueNotEmpty()
    {
        var queue = new Mock<ICommandQueue>();

        queue.SetupGet(q => q.Count).Returns(1);

        var commander = new SMCommander(queue.Object);
        var anotherCommander = Mock.Of<ICommander>();
        
        var state = new SMCStateMoveQueue(commander, anotherCommander);

        Assert.IsTrue(state.IsQueueAlive);
    }
    
    [Test]
    public void SMCStateMoveQueue_IsQueueAlive_ReturnsFalse_IfQueueEmpty()
    {
        var queue = new Mock<ICommandQueue>();

        queue.SetupGet(q => q.Count).Returns(0);

        var commander = new SMCommander(queue.Object);
        var anotherCommander = Mock.Of<ICommander>();

        var state = new SMCStateMoveQueue(commander, anotherCommander);

        Assert.IsFalse(state.IsQueueAlive);
    }

    [Test]
    public void SMCStateMoveQueue_ExecuteNext_MovesCommandTo_AnotherCommander()
    {
        var commander = new SMCommander(Mock.Of<ICommandQueue>());
        var anotherCommander = new Mock<ICommander>();
        anotherCommander.Setup(c => c.Enqueue(It.IsAny<ICommand>())).Verifiable();
        
        var state = new SMCStateMoveQueue(commander, anotherCommander.Object);
        state.ExecuteNext();
        
        anotherCommander.Verify();
    }
}