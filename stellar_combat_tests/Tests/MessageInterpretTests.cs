using NUnit.Framework;
using StaticHelpers;
using StellarCombat;
using StellarCombat.Commands;
using StellarCombat.Interfaces;

[TestFixture]
public class MessageInterpretTests : MessagingTests
{
    private ICommander _commander;

    [OneTimeSetUp]
    public override void SetUp()
    {
        base.SetUp();
    }

    [SetUp]
    public void BeforeTest()
    {
        _commander = new Commander(ExceptionHandler);
    }

    [TestCase(0, 0)]
    public void MessageInterpret_Execute_QueuesCommand(int objectIndex, int cmdIndex)
    {
        var objectId = ObjectIds[objectIndex];
        var commandId = CommandIds[cmdIndex];

        var message = MockingHelper.GetMockedMessage(0u, objectId, commandId);
        var messageInterpret = new MessageInterpret(_commander, message);
        
        messageInterpret.Execute();
        
        Assert.IsTrue(_commander.Queue.Count == 1);
    }
}