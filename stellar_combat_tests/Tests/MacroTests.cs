using NUnit.Framework;
using Moq;
using StellarCombat;
using StellarCombat.Interfaces;
using StellarCombat.Commands;

[TestFixture]
internal class MacroTests
{
    private const int ArraySize = 5;
    
    [Test]
    public void Single_Macro_Execute()
    {
        var handlerMock = Mock.Of<ICommandExceptionHandler>();
        var commander = new Commander(handlerMock);
        var commandToQueue = Mock.Of<ICommand>();
        var macro = new Macro(commander, commandToQueue);
        
        macro.Execute();
        
        Assert.IsTrue(commander.IsInQueue(commandToQueue));
    }

    [Test]
    public void Multiple_Macro_Execute()
    {
        var handlerMock = Mock.Of<ICommandExceptionHandler>();
        var commander = new Commander(handlerMock);
        var commandsToQueue = new ICommand[ArraySize];
        
        for (var i = 0; i < ArraySize; i++)
        {
            commandsToQueue[i] = Mock.Of<ICommand>();
        }
        
        var macro = new Macro(commander, commandsToQueue);
        
        macro.Execute();

        var isQueued = true;

        foreach (var cmd in commandsToQueue)
        {
            if (!commander.IsInQueue(cmd))
            {
                isQueued = false;
                break;
            }
        }
        
        Assert.IsTrue(isQueued);
    }

    [Test]
    public void Null_Macro_Initialize_Throws_Exception()
    {
        var handlerMock = Mock.Of<ICommandExceptionHandler>();
        var commander = new Commander(handlerMock);
        
        Assert.Throws<ArgumentNullException>(() => new Macro(commander, null));
    }
    
    [Test]
    public void Empty_Macro_Initialize_Throws_Exception()
    {
        var handlerMock = Mock.Of<ICommandExceptionHandler>();
        var commander = new Commander(handlerMock);
        
        Assert.Throws<ArgumentException>(() => new Macro(commander, Array.Empty<ICommand>()));
    }
    
    [Test]
    public void Macro_Null_Commander_Initialize_Throws_Exception()
    {
        Commander commander = null;
        var commandToQueue = Mock.Of<ICommand>();
        var macro = new Macro(commander, commandToQueue);
        
        Assert.Throws<NullReferenceException>(macro.Execute);
    }
}

