using System.Numerics;

namespace StellarCombat.Interfaces
{
    public interface IRotatable
    {
        Vector2 Direction { get; set; }
        float AngularVelocity { get; }
    }
}

