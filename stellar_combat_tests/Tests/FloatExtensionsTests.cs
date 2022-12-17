using StellarCombat.Extensions;
using NUnit.Framework;

[TestFixture]
public class FloatExtensionsTests
{
    [Test]
    public void IsValidNumber_ArgIsValid_ReturnsTrue()
    {
        var x = 10.01f;

        var isValid = x.IsValidNumber();
        
        Assert.IsTrue(isValid);
    }
    
    [Test]
    public void IsValidNumber_ArgIsNan_ReturnsFalse()
    {
        var x = float.NaN;

        var isValid = x.IsValidNumber();
        
        Assert.IsFalse(isValid);
    }
    
    [Test]
    public void IsValidNumber_ArgIsInf_ReturnsFalse()
    {
        var x = float.PositiveInfinity;

        var isValid = x.IsValidNumber();
        
        Assert.IsFalse(isValid);
    }
    
    [Test]
    public void IsValidNumber_ArgIsNegInf_ReturnsFalse()
    {
        var x = float.NegativeInfinity;

        var isValid = x.IsValidNumber();
        
        Assert.IsFalse(isValid);
    }
}

