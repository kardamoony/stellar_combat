using System.Numerics;
using StellarCombat.ExceptionHandling.Exceptions;
using StellarCombat.Extensions;
using StellarCombat.Interfaces;

namespace StellarCombat.Commands
{
    public class Rotate : ICommand
    {
        private readonly IRotatable _rotatable;
        
        public Rotate(IRotatable rotatable)
        {
            _rotatable = rotatable;
        }
        
        public void Execute()
        {
            if (!ValidateRotatable())
            {
                throw new CommandException(this);
            }
            
            var matrix = Matrix3x2.CreateRotation(_rotatable.AngularVelocity);
            _rotatable.Direction = Vector2.Transform(_rotatable.Direction, matrix);
        }

        private bool ValidateRotatable()
        {
            return _rotatable.Direction.X.IsValidNumber()
                   && _rotatable.Direction.Y.IsValidNumber()
                   && _rotatable.AngularVelocity.IsValidNumber();
        }
    }
}

