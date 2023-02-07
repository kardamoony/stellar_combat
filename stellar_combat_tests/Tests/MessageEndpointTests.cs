using NUnit.Framework;
using StaticHelpers;
using StellarCombat;
using StellarCombat.Commands;
using StellarCombat.Interfaces;
using StellarCombat.Messaging;

[TestFixture]
public class MessageEndpointTests : MessagingTests
{
    private MessageEndpoint _endpoint;
    private static List<uint> _sessionIds = new List<uint>{ 0u, 1u, 2u, 3u };
    private static Dictionary<uint, ICommander> _sessions;
    
    [OneTimeSetUp]
    public override void SetUp()
    {
        base.SetUp();
        
        _sessions = new Dictionary<uint, ICommander>();

        foreach (var id in _sessionIds)
        {
            _sessions.Add(id, new Commander(ExceptionHandler));
        }
    }
    
    [SetUp]
    public void BeforeTest()
    {
        _endpoint = new MessageEndpoint(_sessions);
    }

    [TestCase(0U, 0, 0)]
    [TestCase(1U, 3, 2)]
    public void Endpoint_ProcessMessage_QueuesInterpretCommand(uint sessionId, int objectIndex, int cmdIndex)
    {
        var objectId = ObjectIds[objectIndex];
        var commandId = CommandIds[cmdIndex];

        var msg = MockingHelper.GetMockedMessage(sessionId, objectId, commandId);

        _endpoint.ProcessMessage(msg);

        var commander = _endpoint.GetSession(sessionId);
        var messageIsQueued = commander.Queue.Contains(cmd => cmd is MessageInterpret);
        
        Assert.IsTrue(messageIsQueued);
    }

    [TestCase(0, 0)]
    public void Endpoint_ProcessMessage_WrongSessionId_ThrowsException(int objectIndex, int cmdIndex)
    {
        var objectId = ObjectIds[objectIndex];
        var commandId = CommandIds[cmdIndex];
        var sessionId = uint.MaxValue;

        var msg = MockingHelper.GetMockedMessage(sessionId, objectId, commandId);

        Assert.Throws<Exception>(() => _endpoint.ProcessMessage(msg));
    }
}