namespace StellarCombat.Interfaces
{
    public interface IMessageEndpoint
    {
        void Receive(byte[] data);
    }
}
