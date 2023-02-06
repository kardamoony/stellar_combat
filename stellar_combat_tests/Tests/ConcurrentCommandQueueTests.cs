using IoC.Interfaces;
using Moq;
using NUnit.Framework;
using StellarCombat.CommandQueues;

[TestFixture]
public class ConcurrentCommandQueueTests : CommandQueueTestsBase
{
    [SetUp]
    public void BeforeTest()
    {
        CommandQueue = new ConcurrentCommandQueue();
    }

    [TestCase(3, 500)]
    public void CommandQueue_Is_ThreadSafe(int commandsCount, int threadTimeoutMs)
    {
        for (var i = 0; i < commandsCount; i++)
        {
            CommandQueue.Enqueue(Mock.Of<ICommand>());
        }
        
        var mre = new ManualResetEvent(false);

        var thread = new Thread(() =>
        {
            while (CommandQueue.Count > 0)
            {
                CommandQueue.Dequeue();
            }

            mre.Set();
        });
        
        thread.Start();
        
        Assert.IsTrue(mre.WaitOne(commandsCount * threadTimeoutMs));
    }
    
}