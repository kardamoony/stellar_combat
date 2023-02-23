using Moq;
using NUnit.Framework;
using StellarCombatAuthorization.Authorization;
using StellarCombatAuthorization.Interfaces;

[TestFixture]
public class AutorizationServiceTests
{
    [Test]
    public void CreateSession_Calls_SessionProvider_CreateMethod()
    {
        var tokenProvider = new Mock<ITokenProvider>();
        var tokenValidator = tokenProvider.As<ITokenValidator>();
        tokenProvider.Setup(tp => tp.Provide(It.IsAny<Guid>(), It.IsAny<uint>())).Returns(It.IsAny<string>());
        tokenValidator.Setup(tp => tp.Validate(It.IsAny<string>())).Returns(true);
        
        var sessionProvider = new Mock<ISessionProvider>();
        uint sessionId;
        sessionProvider.Setup(sp => sp.Create(It.IsAny<Guid[]>(), out sessionId)).Verifiable();

        var authService = new AuthorizationService(sessionProvider.Object, tokenProvider.Object);
        authService.CreateSession(It.IsAny<Guid[]>(), out _);
        
        sessionProvider.Verify();
    }

    [Test]
    public void JoinSession_AuthorizedPlayer_Calls_TokenProvider_ProvideMethod()
    {
        var tokenProvider = new Mock<ITokenProvider>();
        var tokenValidator = tokenProvider.As<ITokenValidator>();
        tokenProvider.Setup(tp => tp.Provide(It.IsAny<Guid>(), It.IsAny<uint>())).Returns(It.IsAny<string>()).Verifiable();
        tokenValidator.Setup(tp => tp.Validate(It.IsAny<string>())).Returns(true);
        
        var sessionProvider = new Mock<ISessionProvider>();
        sessionProvider.Setup(sp => sp.PlayerAuthorizedInSession(It.IsAny<Guid>(), It.IsAny<uint>())).Returns(true);
        
        var authService = new AuthorizationService(sessionProvider.Object, tokenProvider.Object);
        authService.JoinSession(It.IsAny<Guid>(), It.IsAny<uint>(), out _);
        
        tokenValidator.Verify();
    }
    
    [Test]
    public void JoinSession_UnauthorizedPlayerCalls_TokenProvider_ProvideMethod()
    {
        var tokenProvider = new Mock<ITokenProvider>();
        var tokenValidator = tokenProvider.As<ITokenValidator>();
        tokenProvider.Setup(tp => tp.Provide(It.IsAny<Guid>(), It.IsAny<uint>())).Returns(It.IsAny<string>()).Verifiable();
        tokenValidator.Setup(tp => tp.Validate(It.IsAny<string>())).Returns(true);
        
        var sessionProvider = new Mock<ISessionProvider>();
        sessionProvider.Setup(sp => sp.PlayerAuthorizedInSession(It.IsAny<Guid>(), It.IsAny<uint>())).Returns(false);
        
        var authService = new AuthorizationService(sessionProvider.Object, tokenProvider.Object);
        authService.JoinSession(It.IsAny<Guid>(), It.IsAny<uint>(), out _);
        
        tokenProvider.Verify(tp => tp.Provide(It.IsAny<Guid>(), It.IsAny<uint>()), Times.Never);
    }
    
    [Test]
    public void JoinSession_Calls_SessionProvider_PlayerAuthorizedInSessionMethod()
    {
        var tokenProvider = Mock.Of<ITokenProvider>();

        var sessionProvider = new Mock<ISessionProvider>();
        sessionProvider.Setup(sp => sp.PlayerAuthorizedInSession(It.IsAny<Guid>(), It.IsAny<uint>())).Returns(false).Verifiable();
        
        var authService = new AuthorizationService(sessionProvider.Object, tokenProvider);
        authService.JoinSession(It.IsAny<Guid>(), It.IsAny<uint>(), out _);
        
        sessionProvider.Verify();
    }
}