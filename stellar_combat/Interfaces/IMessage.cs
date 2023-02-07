namespace StellarCombat.Interfaces
{
    public interface IMessage
    {
        uint SessionId { get; }
        string ObjectId { get; }
        string OperationId { get; }
        object[] Args { get; }
    }
}