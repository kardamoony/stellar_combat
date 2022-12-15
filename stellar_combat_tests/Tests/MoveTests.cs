using System.Numerics;
using NUnit.Framework;
using Moq;
using StellarCombat.Interfaces;
using StellarCombat.Commands;
using StellarCombat.Extensions;

[TestFixture]
internal class MoveTests
{
    [Test]
    public void Move_FromPosition_12_5_Velocity_7neg_3_ReturnsPosition_5_8()
    {
        var startPosition = new Vector2(12f, 5f); 
        var velocityVector = new Vector2(-7f, 3f);
            
        var velocity = velocityVector.Length();
        var direction = Vector2.Normalize(velocityVector);
            
        var movableMock = Mock.Of<IMovable>( mv =>  
            mv.Position == startPosition
            && mv.Velocity == velocity
            && mv.Direction == direction);

        var expectedPosition = new Vector2(5f, 8f);
            
        var moveCommand = new Move(movableMock);
            
        moveCommand.Execute();
            
        Assert.IsTrue(expectedPosition.Approximately(movableMock.Position));
    }

    [Test]
    public void Move_NullObject_ThrowsException()
    {
        var moveCommand = new Move(null);
        Assert.Throws<NullReferenceException>(() => moveCommand.Execute());
    }
}

