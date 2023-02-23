namespace StellarCombatAuthorization.Interfaces;

public interface ITokenValidator
{
    bool Validate(string token);
}