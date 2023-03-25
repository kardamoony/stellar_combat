using IoC;
using IoC.Commands;
using IoC.Interfaces;
using Moq;
using NUnit.Framework;
using StellarCombat.Commands;
using StellarCombat.Interfaces;

[TestFixture]
public class InterpretUserCommandTests
{
    [Test]
    public void InterpretUserCommand_ForCorrectUser_ExecutesRequiredCommand()
    {
        var userId = "user";
        var commandId = "cmd";
        var objectId = "object";
        var objectArgs = Array.Empty<object>();
        var commandArgs = Array.Empty<object>();

        var contextMock = GetContextMock(userId, objectId, commandId, objectArgs, commandArgs);

        var commandMock = new Mock<ICommand>();
        commandMock.Setup(c => c.Execute()).Verifiable();

        var obj = "string_object";

        new InitializeScopeStrategyCmd().Execute();

        Func<object[], object> commandGetter = (args) => commandMock.Object;
        Func<object[], object> objectGetter = (args) => obj;

        Container.Resolve<ICommand>("IoC.Register", commandId, commandGetter).Execute();

        var userDependencies = new Dictionary<string, Func<object[], object>>
        {
            { objectId + userId, objectGetter }
        };
        
        Container.Resolve<ICommand>("Scopes.Set.New", $"UserScope_{userId}", userDependencies).Execute();

        var interpretCommand = new InterpretUserCommand(contextMock.Object);
        
        interpretCommand.Execute();
        
        commandMock.Verify();
    }
    
        [Test]
        public void InterpretUserCommand_ForWrongUser_ThrowsException()
        {
            var correctUserId = "user_1";
            var incorrectUserId = "user_2";
            
            var commandId = "cmd_1";
            var objectId = "object_1";
            var objectArgs = Array.Empty<object>();
            var commandArgs = Array.Empty<object>();

            var contextMock = GetContextMock(incorrectUserId, objectId, commandId, objectArgs, commandArgs);

            var commandMock = new Mock<ICommand>();
            commandMock.Setup(c => c.Execute()).Verifiable();
    
            var obj = "string_object";
    
            new InitializeScopeStrategyCmd().Execute();
    
            Func<object[], object> commandGetter = (args) => commandMock.Object;
            Func<object[], object> objectGetter = (args) => obj;
    
            Container.Resolve<ICommand>("IoC.Register", commandId, commandGetter).Execute();
    
            var userDependencies = new Dictionary<string, Func<object[], object>>
            {
                { objectId + correctUserId, objectGetter }
            };
            
            Container.Resolve<ICommand>("Scopes.Set.New", $"UserScope_{correctUserId}", userDependencies).Execute();
    
            var interpretCommand = new InterpretUserCommand(contextMock.Object);

            Assert.Throws<ArgumentException>(() => interpretCommand.Execute());
        }

        [Test]
        public void InterpretUserCommand_WithObjectArgs_DoesNotThrowException()
        {
            var userId = "user";
            var commandId = "cmd";
            var objectId = "object";
            var objectArgs = new object [] { "string_arg", 1 };
            var commandArgs = Array.Empty<object>();

            var contextMock = GetContextMock(userId, objectId, commandId, objectArgs, commandArgs);

            new InitializeScopeStrategyCmd().Execute();

            Func<object[], object> commandGetter = (args) => Mock.Of<ICommand>();
            Func<object[], object> objectGetter = (args) => (string)args[0] + (int)args[1];

            Container.Resolve<ICommand>("IoC.Register", commandId, commandGetter).Execute();

            var userDependencies = new Dictionary<string, Func<object[], object>>
            {
                { objectId + userId, objectGetter }
            };
        
            Container.Resolve<ICommand>("Scopes.Set.New", $"UserScope_{userId}", userDependencies).Execute();

            var interpretCommand = new InterpretUserCommand(contextMock.Object);
        
            Assert.DoesNotThrow(() => interpretCommand.Execute());
        }
        
        [Test]
        public void InterpretUserCommand_WithCommandArgs_DoesNotThrowException()
        {
            var userId = "user";
            var commandId = "cmd";
            var objectId = "object";
            var objectArgs = Array.Empty<object>();
            var commandArgs = new object[] { Mock.Of<ICommand>() };

            var contextMock = GetContextMock(userId, objectId, commandId, objectArgs, commandArgs);

            new InitializeScopeStrategyCmd().Execute();

            Func<object[], object> commandGetter = (args) => (ICommand)args[1];
            Func<object[], object> objectGetter = (args) => "string_object";

            Container.Resolve<ICommand>("IoC.Register", commandId, commandGetter).Execute();

            var userDependencies = new Dictionary<string, Func<object[], object>>
            {
                { objectId + userId, objectGetter }
            };
        
            Container.Resolve<ICommand>("Scopes.Set.New", $"UserScope_{userId}", userDependencies).Execute();

            var interpretCommand = new InterpretUserCommand(contextMock.Object);
        
            Assert.DoesNotThrow(() => interpretCommand.Execute());
        }

        private Mock<IUserContext> GetContextMock(string userId, string objectId, string cmdId, object[] objectArgs, object[] commandArgs)
        {
            var contextMock = new Mock<IUserContext>();
            contextMock.SetupGet(c => c.UserId).Returns(userId);
            contextMock.SetupGet(c => c.ObjectId).Returns(objectId);
            contextMock.SetupGet(c => c.CommandId).Returns(cmdId);
            contextMock.SetupGet(c => c.ObjectArgs).Returns(objectArgs);
            contextMock.SetupGet(c => c.CommandArgs).Returns(commandArgs);

            return contextMock;
        }
}