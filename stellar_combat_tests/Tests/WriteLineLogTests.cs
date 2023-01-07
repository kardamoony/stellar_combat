using NUnit.Framework;
using StellarCombat.Commands;

[TestFixture]
public class WriteLineLogTests
{
    [Test]
    public void Write_NotEmpty_String_Execute()
    {
        using var writer = new StringWriter();
        var text = "absckkhAAAhh1123../.\\h_";
        var cmd = new WriteLineLog(writer, text);
        cmd.Execute();

        var expected = text + Environment.NewLine;
            
        Assert.AreEqual(expected, writer.ToString());
    }
    
    [Test]
    public void Write_Empty_String_Execute()
    {
        using var writer = new StringWriter();
        var text = string.Empty;
        var cmd = new WriteLineLog(writer, text);
        cmd.Execute();

        var expected = text + Environment.NewLine;
            
        Assert.AreEqual(expected, writer.ToString());
    }
    
    [Test]
    public void Write_Null_String_Execute()
    {
        using var writer = new StringWriter();
        string text = null;
        var cmd = new WriteLineLog(writer, text);
        cmd.Execute();

        var expected = text + Environment.NewLine;
            
        Assert.AreEqual(expected, writer.ToString());
    }
    
    [Test]
    public void Initialize_Null_Writer_Throws_Exception()
    {
        TextWriter writer = null;
        var text = string.Empty;
        var cmd = new WriteLineLog(writer, text);

        Assert.Throws<NullReferenceException>(cmd.Execute);
    }
}