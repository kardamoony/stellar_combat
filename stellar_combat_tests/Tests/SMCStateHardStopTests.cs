using NUnit.Framework;
using StellarCombat.SMCommander;

[TestFixture]
public class SMCStateHardStopTests
{
    [Test]
    public void SMCStateHardStop_IsQueueAlive_ReturnsFalse()
    {
        var state = new SMCStateHardStop();
        Assert.IsFalse(state.IsQueueAlive);
    }

    [Test]
    public void SMCStateHardStop_ExecuteNext_DoesNotThrow_AnyException()
    {
        var state = new SMCStateHardStop();
        Assert.DoesNotThrow(() => state.ExecuteNext());
    }
}