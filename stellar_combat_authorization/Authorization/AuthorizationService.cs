using StellarCombatAuthorization.Interfaces;

namespace StellarCombatAuthorization.Authorization
{
    public class AuthorizationService
    {
        private readonly ISessionProvider _sessionProvider;
        private readonly ITokenProvider _tokenProvider;

        public AuthorizationService(ISessionProvider sessionProvider, ITokenProvider tokenProvider)
        {
            _sessionProvider = sessionProvider;
            _tokenProvider = tokenProvider;
        }

        public void CreateSession(Guid[] players, out uint sessionId)
        {
            _sessionProvider.Create(players, out sessionId);
        }

        public void JoinSession(Guid playerId, uint sessionId, out string token)
        {
            token = string.Empty;
            
            if (!_sessionProvider.PlayerAuthorizedInSession(playerId, sessionId))
            {
                return;
            }

            token = _tokenProvider.Provide(playerId, sessionId);
        }
    }
}

