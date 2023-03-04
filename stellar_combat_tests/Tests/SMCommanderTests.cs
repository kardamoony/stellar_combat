using IoC.Interfaces;
using Moq;
using NUnit.Framework;
using StellarCombat.Interfaces;
using StellarCombat.SMCommander;

[TestFixture]
public class SMCommanderTests
{
    private SMCommander _commander;

    [Test]
    public void SMCommander_TransitionTo_SetsState()
    {
        _commander = new SMCommander(Mock.Of<ICommandQueue>()).TransitionTo(Mock.Of<ICommanderState>());
        
        var state = Mock.Of<ICommanderState>();
        
        _commander.TransitionTo(state);
        
        Assert.AreSame(state, _commander.CurrentState);
    }

    [Test]
    public void SMCommander_ExecuteNext_CallsState_ExecuteNextMethod()
    {
        var state = new Mock<ICommanderState>();
        state.Setup(s => s.ExecuteNext()).Verifiable();

        _commander = new SMCommander(Mock.Of<ICommandQueue>()).TransitionTo(state.Object);
        _commander.ExecuteNext();
        
        state.Verify();
    }

    [Test]
    public void SMCommander_ExecuteAll_CallsState_IsQueueAlive_Property()
    {
        var state = new Mock<ICommanderState>();
        state.SetupGet(q => q.IsQueueAlive).Verifiable();
        
        _commander = new SMCommander(Mock.Of<ICommandQueue>()).TransitionTo(state.Object);
        _commander.ExecuteAll();
        
        state.Verify();
    }
    
    [Test]
    public void SMCommander_Enqueue_CallsQueue_Enqueue_Method()
    {
        var queue = new Mock<ICommandQueue>();
        queue.Setup(q => q.Enqueue(It.IsAny<ICommand>())).Verifiable();

        _commander = new SMCommander(queue.Object).TransitionTo(Mock.Of<ICommanderState>());
        _commander.Enqueue(It.IsAny<ICommand>());
        
        queue.Verify();
    }
}