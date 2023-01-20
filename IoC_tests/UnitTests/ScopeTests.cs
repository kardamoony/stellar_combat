using IoC;
using NUnit.Framework;

namespace UnitTests;

[TestFixture]
public class ScopeTests
{
    [Test]
    public void Scope_Add_UniqueDependency_ReturnsTrue()
    {
        var dependencies = new Dictionary<string, Func<object[], object>>();
        var scope = new Scope("TestScope", dependencies);
        
        var key = "dependency";
        Func<object[], object> dependency = (args) => args[0];
        
        Assert.IsTrue(scope.TryAdd(key, dependency));
    }
    
    [Test]
    public void Scope_Add_DuplicateKeyDependency_ReturnsFalse()
    {
        var dependencies = new Dictionary<string, Func<object[], object>>();
        var scope = new Scope("TestScope", dependencies);
        
        var key = "dependency";
        
        Func<object[], object> dependency0 = (args) => args[0];
        Func<object[], object> dependency1 = (args) => args[0];

        scope.TryAdd(key, dependency0);
        Assert.IsFalse(scope.TryAdd(key, dependency1));
    }
    
    [Test]
    public void Scope_Resolve_ExistingDependency_IsResolved()
    {
        var dependencies = new Dictionary<string, Func<object[], object>>();

        var key = "dependency";
        Func<object[], object> value = (args) => args[0];
        
        dependencies.Add("dependency", value);

        var scope = new Scope("TestScope", dependencies);

        var expectedString = "returnedValue";
        var resolvedValue = scope.Resolve(key, expectedString);
        
        Assert.AreSame(expectedString, resolvedValue);
    }
    
    [Test]
    public void Scope_Resolve_UnknownDependency_ThrowsException()
    {
        var dependencies = new Dictionary<string, Func<object[], object>>();

        var key = "unknown_dependency";
        var scope = new Scope("TestScope", dependencies);

        Assert.Throws<ArgumentException>(() => scope.Resolve(key, "returnedValue"));
    }
    
    [Test]
    public void Scope_Add_Resolve_IsResolved()
    {
        var dependencies = new Dictionary<string, Func<object[], object>>();

        var scope = new Scope("TestScope", dependencies);
        
        var key = "dependency";
        Func<object[], object> value = (args) => args[0];
        
        scope.TryAdd(key, value);

        var expectedString = "returnedValue";
        var resolvedValue = scope.Resolve(key, expectedString);
        
        Assert.AreSame(expectedString, resolvedValue);
    }

    [Test]
    public void Scope_Resolve_FromParentScope_IsResolved()
    {
        var parentDependencies = new Dictionary<string, Func<object[], object>>();
        var parentScope = new Scope("ParentScope", parentDependencies);
        
        var key = "dependency";
        Func<object[], object> value = (args) => args[0];
        
        parentScope.TryAdd(key, value);
        
        var childDependencies = new Dictionary<string, Func<object[], object>>();
        var childScope = new Scope(parentScope, "ParentScope", childDependencies);
        
        var expectedString = "returnedValue";
        var resolvedValue = childScope.Resolve(key, expectedString);
        
        Assert.AreSame(expectedString, resolvedValue);
    }
}