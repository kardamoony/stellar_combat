using IoC;
using IoC.Commands;
using IoC.Interfaces;
using NUnit.Framework;
using StellarCombat.Messaging.Messages;
using TestStrategy;

[TestFixture]
public class NetworkMessageTests
{
    private void SetupContainer()
    {
        new InitializeStrategyCmd(IocTestStrategy.GetStrategy).Execute();
        Container.Resolve<ICommand>("IoC.Clear").Execute();
    }
    
    [Test]
    public void NetworkMessage_ValidateToken_IsResolvedInIoC()
    {
        SetupContainer();

        var isValidated = false;
        Func<object[], object> validateFunc = _ => isValidated = true;
        
        Container.Resolve<ICommand>("IoC.Register", "TokenValidator.Validate", validateFunc).Execute();
        
        var mssg = new NetworkMessage();

        var validationResult = mssg.ValidateToken();
        Assert.IsTrue(validationResult && isValidated);
    }
}