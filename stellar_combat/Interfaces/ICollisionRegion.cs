using System.Numerics;

namespace StellarCombat.Interfaces
{
    public interface ICollisionRegion
    {
        IEnumerable<ICollidable> Objects { get; }
        int ObjectsCount { get; }

        void Add(ICollidable collidable);
        void Remove(ICollidable collidable);

        bool IsInRegion(ICollidable collidable);
        ICollisionRegion GetObjectRegion(Vector2 position);
    }
}