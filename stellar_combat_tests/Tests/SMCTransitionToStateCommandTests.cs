using Moq;
using NUnit.Framework;
using StellarCombat.Interfaces;
using StellarCombat.SMCommander;

[TestFixture]
public class SMCTransitionToStateCommandTests
{
    [Test]
    public void SMCMoveQueueCommand_Execute_SetsCommanderState()
    {
        var commander = new SMCommander(Mock.Of<ICommandQueue>());
        var state = Mock.Of<ICommanderState>();
        var cmd = new SMCTransitionToStateCommand(commander, state);
        cmd.Execute();
        
        Assert.AreSame(state, commander.CurrentState);
    }
}