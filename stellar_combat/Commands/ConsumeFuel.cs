using IoC.Interfaces;
using StellarCombat.Interfaces;

namespace StellarCombat.Commands
{
    public class ConsumeFuel : ICommand
    {
        private readonly IFuelConsumer _fuelConsumer;
        
        public ConsumeFuel(IFuelConsumer fuelConsumer)
        {
            _fuelConsumer = fuelConsumer;
        }
        
        public void Execute()
        {
            var fuelLevel = _fuelConsumer.FuelAmount -= _fuelConsumer.ConsumeSpeed;
            _fuelConsumer.FuelAmount = fuelLevel;
        }
    }
}

