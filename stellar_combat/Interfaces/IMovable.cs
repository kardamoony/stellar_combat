using System.Numerics;

namespace StellarCombat.Interfaces
{
    public interface IMovable
    {
        Vector2 Position { get; set; }
        Vector2 Direction { get; }
        float Velocity { get; }
    }
}

