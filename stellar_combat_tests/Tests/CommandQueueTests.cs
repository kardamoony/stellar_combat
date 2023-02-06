using NUnit.Framework;
using StellarCombat.CommandQueues;

[TestFixture]
public class CommandQueueTests : CommandQueueTestsBase
{
    [SetUp]
    public void BeforeTest()
    {
        CommandQueue = new CommandQueue();
    }
}