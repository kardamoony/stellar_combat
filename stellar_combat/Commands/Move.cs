using System.Numerics;
using StellarCombat.Extensions;
using StellarCombat.Interfaces;

namespace StellarCombat.Commands
{
    public class Move : ICommand
    {
        private readonly IMovable _movable;
        
        public Move(IMovable movable)
        {
            _movable = movable;
        }
        
        public void Execute()
        {
            if (!ValidateMovable())
            {
                throw new ArgumentException();
            }
            
            _movable.Position = GetNextPosition();
        }

        private Vector2 GetNextPosition()
        {
            return _movable.Position + _movable.Direction * _movable.Velocity;
        }

        private bool ValidateMovable()
        {
            return _movable.Position.X.IsValidNumber()
                   && _movable.Position.Y.IsValidNumber()
                   && _movable.Direction.X.IsValidNumber()
                   && _movable.Direction.Y.IsValidNumber()
                   && _movable.Velocity.IsValidNumber();
        }
    }
}

