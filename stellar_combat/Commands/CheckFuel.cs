using IoC.Interfaces;
using StellarCombat.ExceptionHandling.Exceptions;
using StellarCombat.Interfaces;

namespace StellarCombat.Commands
{
    public class CheckFuel : ICommand
    {
        private readonly IFuelConsumer _fuelConsumer;
        
        public CheckFuel(IFuelConsumer fuelConsumer)
        {
            _fuelConsumer = fuelConsumer;
        }
        
        public void Execute()
        {
            if (_fuelConsumer.ConsumeSpeed > _fuelConsumer.FuelAmount)
            {
                throw new CommandException(this);
            }
        }
    }
}

