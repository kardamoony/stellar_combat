using System.Numerics;
using IoC.Interfaces;

namespace StellarCombat.Interfaces
{
    public interface ICollidable
    {
        Vector2 Position { get; set; }
        ICommand? CheckCollisionCmd { get; set; }
    }
}

