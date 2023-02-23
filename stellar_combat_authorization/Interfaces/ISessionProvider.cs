namespace StellarCombatAuthorization.Interfaces
{
    public interface ISessionProvider
    {
        void Create(Guid[] players, out uint sessionId);
        bool PlayerAuthorizedInSession(Guid playerId, uint sessionId);
    }
}

