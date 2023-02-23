using IoC;
using StellarCombat.Interfaces;

namespace StellarCombat.Messaging.Messages
{
    public class NetworkMessage : IMessage
    {
        public uint SessionId { get; }
        public string ObjectId { get; }
        public string OperationId { get; }
        
        public string Token { get; }
        public object[] Args { get; }

        public bool ValidateToken()
        {
            return Container.Resolve<bool>("TokenValidator.Validate", Token);
        }
    }
}

