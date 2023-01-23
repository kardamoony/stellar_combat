using IoC.Commands;
using NUnit.Framework;

namespace UnitTests;

[TestFixture]
public class SetValueCmdTests
{
    [TestCase(10)]
    public void SetValueCmd_Execute_SetsIntValue(int value)
    {
        var variable = value - 1;

        Action<int> setter = (val) => variable = val;

        var cmd = new SetValueCmd<int>(setter, value);
        
        cmd.Execute();

        Assert.AreEqual(variable, value);
    }
 
    [TestCase((object)"TestObject")]
    public void SetValueCmd_Execute_SetsObjectValue(object value)
    {
        var variable = (object)1;

        Action<object> setter = (val) => variable = val;

        var cmd = new SetValueCmd<object>(setter, value);
        
        cmd.Execute();

        Assert.AreSame(variable, value);
    }
}