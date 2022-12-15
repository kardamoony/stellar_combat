using System.Numerics;

namespace StellarCombat.Extensions
{
    public static class Vector2Extensions
    {
        public static bool Approximately(this Vector2 thisVector2, Vector2 vector2)
        {
            return thisVector2.X.Approximately(vector2.X)
                   && thisVector2.Y.Approximately(vector2.Y);
        }
    }
}

