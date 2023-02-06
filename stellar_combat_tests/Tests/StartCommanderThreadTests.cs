using IoC.Interfaces;
using Moq;
using NUnit.Framework;
using StellarCombat;
using StellarCombat.CommandQueues;
using StellarCombat.Commands;
using StellarCombat.Interfaces;

[TestFixture]
public class StartCommanderThreadTests
{
    private ICommander _commander;
    private StartCommanderThread _startCommand;
    
    [SetUp]
    public void BeforeTest()
    {
        _commander = new Commander(Mock.Of<ICommandExceptionHandler>(), new ConcurrentCommandQueue());
        _startCommand = new StartCommanderThread(_commander);
    }
    
    [TestCase(4, 500)]
    public void Commander_SoftStop_LetsQueueToExecute(int commandsCount, int timeoutMs)
    {
        _commander.Enqueue(new SoftStop(_commander));
        
        var commands = new Mock<ICommand>[commandsCount];

        for (var i = 0; i < commandsCount; i++)
        {
            commands[i] = new Mock<ICommand>();
            commands[i].Setup(c => c.Execute()).Verifiable();
            _commander.Enqueue(commands[i].Object);
        }

        _startCommand.Execute();
        _startCommand.WorkingThread.Join(commandsCount * timeoutMs);
        
        foreach (var cmd in commands)
        {
            cmd.Verify();
        }
    }
    
    [TestCase(4, 500)]
    public void Commander_SoftStop_CompletesThread(int commandsCount, int timeoutMs)
    {
        _commander.Enqueue(new SoftStop(_commander));
        
        var commands = new Mock<ICommand>[commandsCount];

        for (var i = 0; i < commandsCount; i++)
        {
            commands[i] = new Mock<ICommand>();
            commands[i].Setup(c => c.Execute()).Verifiable();
            _commander.Enqueue(commands[i].Object);
        }

        _startCommand.Execute();

        Assert.IsTrue(_startCommand.WorkingThread.Join(commandsCount * timeoutMs));
    }
    
    [TestCase(4, 500)]
    public void Commander_HardStop_StopsQueueImmediately(int commandsCount, int timeoutMs)
    {
        _commander.Enqueue(new HardStop(_commander));
        
        var commands = new Mock<ICommand>[commandsCount];

        for (var i = 0; i < commandsCount; i++)
        {
            commands[i] = new Mock<ICommand>();
            commands[i].Setup(c => c.Execute()).Verifiable();
            _commander.Enqueue(commands[i].Object);
        }

        _startCommand.Execute();
        _startCommand.WorkingThread.Join(commandsCount * timeoutMs);

        Assert.IsTrue(_commander.Queue.Count == commandsCount);
    }
    
    [TestCase(4, 500)]
    public void Commander_HardStop_CompletesThread(int commandsCount, int timeoutMs)
    {
        _commander.Enqueue(new HardStop(_commander));
        
        var commands = new Mock<ICommand>[commandsCount];

        for (var i = 0; i < commandsCount; i++)
        {
            commands[i] = new Mock<ICommand>();
            commands[i].Setup(c => c.Execute()).Verifiable();
            _commander.Enqueue(commands[i].Object);
        }

        _startCommand.Execute();
        
        Assert.IsTrue(_startCommand.WorkingThread.Join(commandsCount * timeoutMs));
    }
}