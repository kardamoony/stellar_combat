using System.Numerics;
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
            _movable.Position = GetNextPosition();
        }

        private Vector2 GetNextPosition()
        {
            return _movable.Position + _movable.Direction * _movable.Velocity;
        }
    }
}

