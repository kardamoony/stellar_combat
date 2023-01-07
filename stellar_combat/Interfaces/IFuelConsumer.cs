namespace StellarCombat.Interfaces
{
    public interface IFuelConsumer
    {
        int FuelAmount { get; set; }
        int ConsumeSpeed { get; }
    }
}

