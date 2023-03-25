namespace StellarCombat.Interfaces
{
    public interface IUserContext
    { 
        string UserId { get; }
        string ObjectId { get; }
        string CommandId { get; }
        object[] ObjectArgs { get; }
        object[] CommandArgs { get; }
    }
}