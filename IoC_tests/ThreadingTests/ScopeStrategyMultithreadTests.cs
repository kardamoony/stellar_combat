using IoC;
using IoC.Commands;
using IoC.Interfaces;
using NUnit.Framework;

namespace ThreadingTests;

[TestFixture]
public class ScopeStrategyThreadingTests
{
    private const int ThreadsCount = 4;
    private const int ThreadTimeoutMs = 5000;
    
    [Test]
    public void Container_CanBeUsed_FromMultipleThreads()
    {
        new InitializeScopeStrategyCmd().Execute();
        var threads = new Thread[ThreadsCount];

        for (var i = 0; i < ThreadsCount; i++)
        {
            threads[i] = new Thread(() =>
            {
                Container.Resolve<IScope>("Scopes.Get.Current");
            });
        }

        foreach (var thread in threads)
        {
            thread.Start();
        }

        foreach (var thread in threads)
        {
            if (thread.Join(ThreadTimeoutMs))
            {
                continue;
            }
            
            Assert.Fail();
            return;
        }
        
        Assert.Pass();
    }
}