namespace StellarCombat.Extensions
{
    public static class FloatExtensions
    {
        public static bool Approximately(this float thisValue, float value)
        {
            return Math.Abs(thisValue - value) < float.Epsilon;
        }
    }
}

