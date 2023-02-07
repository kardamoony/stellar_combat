using IoC;
using IoC.Commands;
using IoC.Interfaces;
using Moq;
using StellarCombat.Interfaces;

public abstract class MessagingTests
{
    protected static ICommandExceptionHandler ExceptionHandler;

    protected static List<string> ObjectIds = new List<string> { "obj_0", "obj_1", "obj_2", "obj_3" };
    protected static List<string> CommandIds = new List<string> { "cmd_0", "cmd_1", "cmd_2" };
    
    public virtual void SetUp()
    {
        ExceptionHandler = Mock.Of<ICommandExceptionHandler>();
        SetupContainer();
    }

    private void SetupContainer()
    {
        try
        {
            new InitializeScopeStrategyCmd().Execute();

            foreach (var objectId in ObjectIds)
            {
                Container.Resolve<ICommand>
                    ("IoC.Register", objectId, (Func<object[], object>)((args) => new object())).Execute();
            }

            foreach (var commandId in CommandIds)
            {
                Container.Resolve<ICommand>
                    ("IoC.Register", commandId, (Func<object[], object>)((args) => Mock.Of<ICommand>())).Execute();
            }
        }
        catch (ArgumentException e)
        {
            Console.WriteLine(e);
        }
    }
}