namespace StellarCombatAuthorization.Interfaces
{
    public interface ITokenProvider
    {
        string Provide(Guid playerId, uint sessionId);
    }
}

