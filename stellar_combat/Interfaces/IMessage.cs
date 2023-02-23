namespace StellarCombat.Interfaces
{
    public interface IMessage
    {
        uint SessionId { get; }
        string ObjectId { get; }
        string OperationId { get; }
        string Token { get; }
        object[] Args { get; }

        bool ValidateToken();
    }
}