using StellarCombat.Commands;
using StellarCombat.Interfaces;

namespace StellarCombat.ExceptionHandling.Handlers
{
    public class CommanderExceptionHandler : ICommandExceptionHandler, IDisposable
    {
        private readonly int _repeatsOnException;
        private readonly Dictionary<Type, int> _exceptionsOccurrence = new Dictionary<Type, int>();
        private readonly ICommandExceptionHandler _handler;

        private Commander _commander;
        
        public TextWriter TextWriter { get; }

        public Commander Commander
        {
            get => _commander;
            set => SetCommander(value);
        }
        
        public CommanderExceptionHandler(TextWriter writer, int repeatsOnException = 1)
        {
            TextWriter = writer;
            _repeatsOnException = repeatsOnException;
            _handler = new EnqueueCommandExceptionHandler();
        }

        public void Handle(ICommand command, Exception exception)
        {
            var exceptionType = exception.GetType();

            var hasOccured = HasExceptionOccured(exceptionType, out var occurrenceCount);

            if (!hasOccured)
            {
                _handler.Handle(command, exception);
                _exceptionsOccurrence.Add(exceptionType, 1);
                return;
            }

            if (occurrenceCount < _repeatsOnException)
            {
                _handler.Handle(command, exception);
                _exceptionsOccurrence[exceptionType] += 1;
                return;
            }

            _handler.Handle(new WriteLineLog(TextWriter, exception.ToString()), exception);
            _exceptionsOccurrence.Remove(exceptionType);
        }

        public bool HasExceptionOccured(Type exceptionType, out int occurrences)
        {
            return _exceptionsOccurrence.TryGetValue(exceptionType, out occurrences);
        }
        
        public void Dispose()
        {
            TextWriter.Dispose();
        }
        
        private void SetCommander(Commander commander)
        {
            _commander = commander;
            _handler.Commander = commander;
        }
    }
}

