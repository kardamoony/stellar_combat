using IoC.Interfaces;
using StellarCombat.Interfaces;

namespace StellarCombat.Commands
{
    public class CheckCollisionRegion : ICommand
    {
        private readonly ICommander _commander;
        private readonly ICollidable _collidable;
        private ICollisionRegion _collisionRegion;

        public CheckCollisionRegion(
            ICollidable collidable,
            ICollisionRegion region,
            ICommander commander)
        {
            _collidable = collidable;
            _collisionRegion = region;
            _commander = commander;
        }
        
        public void Execute()
        {
            if (!_collisionRegion.IsInRegion(_collidable))
            {
                _collisionRegion.Remove(_collidable);
                var nextRegion = _collisionRegion.GetObjectRegion(_collidable.Position);
                nextRegion.Add(_collidable);

                _collisionRegion = nextRegion;
            }
            
            var commandsCount = _collidable.CheckCollisionCmd != null
                ? _collisionRegion.ObjectsCount + 1
                : _collisionRegion.ObjectsCount;

            var checkCollisionCommands = new ICommand[commandsCount];

            var i = 0;
                
            foreach (var collidable in _collisionRegion.Objects)
            {
                checkCollisionCommands[i++] = IoC.Container.Resolve<ICommand>("Commands.CheckCollision", _collidable, collidable);
            }

            if (_collidable.CheckCollisionCmd != null)
            {
                checkCollisionCommands[i] = _collidable.CheckCollisionCmd;
            }

            _collidable.CheckCollisionCmd = new Macro(_commander, checkCollisionCommands);
        }
    }
}

