using IoC.Commands;
using IoC.Interfaces;
using Moq;
using NUnit.Framework;

namespace UnitTests;

[TestFixture]
public class RegisterScopeDependencyCmdTests
{
    [Test]
    public void Execute_RegistersReturnsTrue_DoesNotThrowException()
    {
        var scopeMock = new Mock<IScope>();
        scopeMock.Setup(s => s.TryAdd(It.IsAny<string>(), It.IsAny<Func<object[], object>>())).Returns(true);

        var cmd = new RegisterScopeDependencyCmd(It.IsAny<string>(), It.IsAny<Func<object[], object>>(), scopeMock.Object);
        
        Assert.DoesNotThrow(cmd.Execute);
    }
    
    [Test]
    public void Execute_RegistersReturnsFalse_ThrowsException()
    {
        var scopeMock = new Mock<IScope>();
        scopeMock.Setup(s => s.TryAdd(It.IsAny<string>(), It.IsAny<Func<object[], object>>())).Returns(false);

        var cmd = new RegisterScopeDependencyCmd(It.IsAny<string>(), It.IsAny<Func<object[], object>>(), scopeMock.Object);
        
        Assert.Throws<ArgumentException>(cmd.Execute);
    }
}