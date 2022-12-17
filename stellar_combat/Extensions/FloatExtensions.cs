namespace StellarCombat.Extensions
{
    public static class FloatExtensions
    {
        public static bool IsValidNumber(this float thisValue)
        {
            return !float.IsInfinity(thisValue) && !float.IsNaN(thisValue);
        }
    }
}

