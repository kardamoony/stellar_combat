using Moq;
using StellarCombat.Interfaces;

namespace StaticHelpers;

public static class MockingHelper
{
    public static IMessage GetMockedMessage(uint sessionId, string objectId, string commandId)
    {
        var msgMock = new Mock<IMessage>();
        
        msgMock.SetupGet(m => m.ObjectId).Returns(objectId);
        msgMock.SetupGet(m => m.SessionId).Returns(sessionId);
        msgMock.SetupGet(m => m.OperationId).Returns(commandId);

        return msgMock.Object;
    }
}