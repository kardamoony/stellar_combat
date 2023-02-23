using NUnit.Framework;
using StellarCombatAuthorization.Authorization;

[TestFixture]
public class JwtTokenProviderTests
{
    private const string TestSecret = "test_key_983rjkvcxk]lkld^jwq";
    private const string TestIssuer = "TestIssuer";
    private const uint TestSession = 323;
    
    private JwtTokenProvider _jwtTokenProvider;
    
    [SetUp]
    public void BeforeTest()
    {
        _jwtTokenProvider = new JwtTokenProvider(TestSecret, TestIssuer);
    }

    [Test]
    public void Provide_Returns_NotEmptyString()
    {
        var token = _jwtTokenProvider.Provide(new Guid(), TestSession);
        Assert.IsFalse(string.IsNullOrEmpty(token));
    }

    [Test]
    public void Verify_ValidToken_ReturnsTrue()
    {
        var token = _jwtTokenProvider.Provide(new Guid(), TestSession);
        Assert.IsTrue(_jwtTokenProvider.Validate(token));
    }
    
    [Test]
    public void Verify_AppendedToken_ReturnsFalse()
    {
        var token = _jwtTokenProvider.Provide(new Guid(), TestSession) + "_test";
        Assert.IsFalse(_jwtTokenProvider.Validate(token));
    }
    
    [Test]
    public void Verify_InvalidToken_ReturnsFalse()
    {
        var token = _jwtTokenProvider.Provide(new Guid(), TestSession);
        var split = token.Split('.');

        var reconstructed = split[0];

        for (var i = 1; i < split.Length - 1; i++)
        {
            reconstructed += "." + split[i];
        }
        
        Assert.IsFalse(_jwtTokenProvider.Validate(reconstructed));
    }
}