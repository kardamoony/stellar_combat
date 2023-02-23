using NUnit.Framework;
using StaticHelpers;
using StellarCombat.Extensions;

[TestFixture]
public class NetworkingExtensionsTests
{
    private class SimpleSerializable
    {
        public string StringValue;
        public int IntValue;
    }
    
    private class ComplexSerializable
    {
        public SimpleSerializable NestedObject = new SimpleSerializable 
            { StringValue = "Name", IntValue = 42 };
    }
    
    [Test]
    public void Deserialize_SimpleObject_ReturnsObject()
    {
        var testObject = new SimpleSerializable { StringValue = "Name", IntValue = 42};
        var bytes = NetworkingHelper.ObjectToBytes(testObject);

        var deserialized = bytes.Deserialize<SimpleSerializable>();
        
        Assert.IsTrue(deserialized.IntValue == 42 && deserialized.StringValue == "Name");
    }
    
    [Test]
    public void Deserialize_ComplexObject_ReturnsObject()
    {
        var testObject = new ComplexSerializable();
        var bytes = NetworkingHelper.ObjectToBytes(testObject);

        var deserialized = bytes.Deserialize<ComplexSerializable>();
        
        Assert.IsTrue(deserialized.NestedObject.IntValue == 42 && deserialized.NestedObject.StringValue == "Name");
    }

    [Test]
    public void Deserialize_Null_ReturnsNull()
    {
        SimpleSerializable testObject = null;
        var bytes = NetworkingHelper.ObjectToBytes(testObject);

        var deserialized = bytes.Deserialize<SimpleSerializable>();
        
        Assert.IsTrue(deserialized == null);
    }

    [Test]
    public void Deserialize_WrongType_ReturnsNull()
    {
        SimpleSerializable testObject = null;
        var bytes = NetworkingHelper.ObjectToBytes(testObject);

        var deserialized = bytes.Deserialize<ComplexSerializable>();
        
        Assert.IsTrue(deserialized == null);
    }
}