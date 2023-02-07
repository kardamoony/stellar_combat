using StellarCombat.Commands;
using StellarCombat.Extensions;
using StellarCombat.Interfaces;

namespace StellarCombat.Messaging
{
    public class MessageEndpoint : IMessageEndpoint
    {
        private readonly Dictionary<uint, ICommander> _sessions;

        public MessageEndpoint(Dictionary<uint, ICommander> sessions)
        {
            _sessions = sessions;
        }

        public void Receive(byte[] data)
        {
            ProcessMessage(data.Deserialize<IMessage>());
        }

        public void ProcessMessage(IMessage message)
        {
            var commander = GetSession(message.SessionId);
            commander.Enqueue(new MessageInterpret(commander, message));
        }

        public ICommander GetSession(uint id)
        {
            if (_sessions.TryGetValue(id, out var commander))
            {
                return commander;
            }

            throw new Exception($"Game with id={id} not found");
        }
    }
}

