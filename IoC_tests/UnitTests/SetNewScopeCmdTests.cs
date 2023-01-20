using IoC.Commands;
using Moq;
using NUnit.Framework;

namespace UnitTests;

[TestFixture]
public class SetNewScopeCmdTests
{
    [Test]
    public void Execute_DoesNotThrowException()
    {
        var cmd = new SetNewScopeCmd(It.IsAny<string>(), It.IsAny<IDictionary<string, Func<object[], object>>>());
        Assert.DoesNotThrow(() => cmd.Execute());
    }
}