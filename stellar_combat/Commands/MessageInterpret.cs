using IoC;
using IoC.Interfaces;
using StellarCombat.Interfaces;

namespace StellarCombat.Commands
{
    public class MessageInterpret : ICommand
    {
        private readonly ICommander _commander;
        private readonly IMessage _message;
        
        public MessageInterpret(ICommander commander, IMessage message)
        {
            _commander = commander;
            _message = message;
        }
        
        public void Execute()
        {
            var target = Container.Resolve<object>(_message.ObjectId);
            var cmd = Container.Resolve<ICommand>(_message.OperationId, target, _message.Args);
            _commander.Enqueue(cmd);
        }
    }
}