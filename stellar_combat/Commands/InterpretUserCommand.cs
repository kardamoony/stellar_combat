using IoC;
using IoC.Interfaces;
using StellarCombat.Interfaces;

namespace StellarCombat.Commands;

public class InterpretUserCommand : ICommand
{
    private readonly IUserContext _context;

    public InterpretUserCommand(IUserContext context)
    {
        _context = context;
    }

    public void Execute()
    {
        var objectKey = _context.ObjectId + _context.UserId;
        var actionObject = Container.Resolve<object>(objectKey, _context.ObjectArgs);
        
        var commandArgs = new List<object> { actionObject };
        commandArgs.AddRange(_context.CommandArgs);

        Container.Resolve<ICommand>(_context.CommandId, commandArgs.ToArray()).Execute();
    }
}