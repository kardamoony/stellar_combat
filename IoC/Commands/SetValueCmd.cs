using IoC.Interfaces;

namespace IoC.Commands
{
    public class SetValueCmd<T>: ICommand
    {
        private readonly Action<T> _setter;
        private readonly T _value;

        public SetValueCmd(Action<T> setter, T value)
        {
            _setter = setter;
            _value = value;
        }

        public void Execute()
        {
            _setter.Invoke(_value);
        }
    }
}