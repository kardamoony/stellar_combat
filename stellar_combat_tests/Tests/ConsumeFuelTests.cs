using Moq;
using NUnit.Framework;
using StellarCombat.Commands;
using StellarCombat.Interfaces;

[TestFixture]
public class ConsumeFuelTests
{
    [Test]
    public void ConsumeFuel_Execute_Decreases_FuelAmount()
    {
        var fuelConsumerMock = Mock.Of<IFuelConsumer>(fc => 
            fc.FuelAmount == 10 && 
            fc.ConsumeSpeed == 5);

        var cmd = new ConsumeFuel(fuelConsumerMock);
        cmd.Execute();
        var expectedFuel = 5;
        
        Assert.AreEqual(expectedFuel, fuelConsumerMock.FuelAmount);
    }
}