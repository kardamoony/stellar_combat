using StellarCombat.Interfaces;

namespace StellarCombat.Commands
{
    public class WriteLineLog : ICommand
    {
        private readonly string _text;
        private readonly TextWriter _textWriter;

        public WriteLineLog(TextWriter writer, string text)
        {
            _textWriter = writer;
            _text = text;
        }
        
        public void Execute()
        {
            _textWriter.WriteLine(_text);
        }
    }
}

