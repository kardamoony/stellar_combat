using Moq;
using NUnit.Framework;
using StellarCombat.Commands;
using StellarCombat.ExceptionHandling.Exceptions;
using StellarCombat.Interfaces;

[TestFixture]
public class CheckFuelTests
{
    [Test]
    public void CheckFuel_SufficientFuel_Execute_Throws_No_Exception()
    {
        var fuelConsumerMock = Mock.Of<IFuelConsumer>(fc => 
            fc.FuelAmount == 100 && 
            fc.ConsumeSpeed == 1);

        var cmd = new CheckFuel(fuelConsumerMock);
        Assert.That(() => cmd.Execute(), Throws.Nothing);
    }
    
    [Test]
    public void CheckFuel_InsufficientFuel_Execute_Throws_Exception()
    {
        var fuelConsumerMock = Mock.Of<IFuelConsumer>(fc => 
            fc.FuelAmount == 5 && 
            fc.ConsumeSpeed == 10);

        var cmd = new CheckFuel(fuelConsumerMock);
        Assert.Throws<CommandException>(cmd.Execute);
    }
}