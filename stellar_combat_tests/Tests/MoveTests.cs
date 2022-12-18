using System.Numerics;
using NUnit.Framework;
using Moq;
using StellarCombat.Interfaces;
using StellarCombat.Commands;

[TestFixture]
internal class MoveTests
{
    private static readonly Vector2 Vector2_Nan = new Vector2(1f, float.NaN);
    private static readonly Vector2 Vector2_Inf = new Vector2(1f, float.PositiveInfinity);
    private static readonly Vector2 Vector2_NegInf = new Vector2(1f, float.NegativeInfinity);
    
    //This test is here because it's required by the homework assignment
    //Though it's an overcomplicated design for the Move class in its present implementation 
    [Test]
    public void Move_FromPosition_12_5_VelocityVector_7neg_3_ReturnsPosition_5_8()
    {
        var velocityVector = new Vector2(-7f, 3f);
            
        var startPosition = new Vector2(12f, 5f); 
        var velocity = velocityVector.Length();
        var direction = Vector2.Normalize(velocityVector);
            
        var movableMock = Mock.Of<IMovable>( mv =>  
            mv.Position == startPosition
            && mv.Velocity == velocity
            && mv.Direction == direction);

        var expectedPosition = new Vector2(5f, 8f);
            
        var moveCmd = new Move(movableMock);
            
        moveCmd.Execute();

        var areEqual = MathF.Abs(expectedPosition.X - movableMock.Position.X) < float.Epsilon 
                     && MathF.Abs(expectedPosition.Y - movableMock.Position.Y) < float.Epsilon ;
        
        Assert.IsTrue(areEqual);
    }
    
    [Test]
    public void Move_Negative_Velocity()
    {
        var startPosition = Vector2.Zero;
        var velocity = -9f;
        var direction = Vector2.UnitX;
            
        var movableMock = Mock.Of<IMovable>( mv =>  
            mv.Position == startPosition
            && mv.Velocity == velocity
            && mv.Direction == direction);

        var expectedPosition = new Vector2(-9f, 0f);
            
        var moveCmd = new Move(movableMock);
            
        moveCmd.Execute();
        
        var areEqual = MathF.Abs(expectedPosition.X - movableMock.Position.X) < float.Epsilon 
                             && MathF.Abs(expectedPosition.Y - movableMock.Position.Y) < float.Epsilon;
                
        Assert.IsTrue(areEqual);
    }
    
    [Test]
    public void Move_Velocity_0_Returns_StartPosition()
    {
        var startPosition = Vector2.UnitY;
        var velocity = 0f;
        var direction = Vector2.UnitX;
            
        var movableMock = Mock.Of<IMovable>( mv =>  
            mv.Position == startPosition
            && mv.Velocity == velocity
            && mv.Direction == direction);

        var expectedPosition = Vector2.UnitY;
            
        var moveCmd = new Move(movableMock);
            
        moveCmd.Execute();
        
        var areEqual = MathF.Abs(expectedPosition.X - movableMock.Position.X) < float.Epsilon 
                       && MathF.Abs(expectedPosition.Y - movableMock.Position.Y) < float.Epsilon;
                
        Assert.IsTrue(areEqual);
    }
    
    [Test]
    public void Move_Direction_0_0_Returns_StartPosition()
    {
        var startPosition = new Vector2(1, -7);
        var velocity = 100f;
        var direction = Vector2.Zero;
            
        var movableMock = Mock.Of<IMovable>( mv =>  
            mv.Position == startPosition
            && mv.Velocity == velocity
            && mv.Direction == direction);

        var moveCmd = new Move(movableMock);
            
        moveCmd.Execute();
        
        var areEqual = MathF.Abs(startPosition.X - movableMock.Position.X) < float.Epsilon 
                       && MathF.Abs(startPosition.Y - movableMock.Position.Y) < float.Epsilon;
                
        Assert.IsTrue(areEqual);
    }
    
    [Test]
    public void Move_FromPosition_0_0_Dir_1_0_Velocity_3_Returns_3_0()
    {
        var startPosition = Vector2.Zero;
        var velocity = 3f;
        var direction = Vector2.UnitX;
            
        var movableMock = Mock.Of<IMovable>( mv =>  
            mv.Position == startPosition
            && mv.Velocity == velocity
            && mv.Direction == direction);

        var expectedPosition = new Vector2(3f, 0f);
            
        var moveCmd = new Move(movableMock);
            
        moveCmd.Execute();
        
        var areEqual = MathF.Abs(expectedPosition.X - movableMock.Position.X) < float.Epsilon 
                       && MathF.Abs(expectedPosition.Y - movableMock.Position.Y) < float.Epsilon;
        
        Assert.IsTrue(areEqual);
    }
    
    [Test]
    public void Move_NullObject_ThrowsException()
    {
        var moveCommand = new Move(null);
        Assert.Throws<NullReferenceException>(() => moveCommand.Execute());
    }

    [Test]
    public void Move_FromPosition_NaN_ThrowsException()
    {
        var velocity = 1f;
        var direction = Vector2.UnitX;
            
        var movableMock = Mock.Of<IMovable>( mv =>  
            mv.Position == Vector2_Nan
            && mv.Velocity == velocity
            && mv.Direction == direction);
        
        var moveCmd = new Move(movableMock);

        Assert.Throws<ArgumentException>(() => moveCmd.Execute());
    }
    
    [Test]
    public void Move_FromPosition_Inf_ThrowsException()
    {
        var velocity = 1f;
        var direction = Vector2.UnitX;
            
        var movableMock = Mock.Of<IMovable>( mv =>  
            mv.Position == Vector2_Inf
            && mv.Velocity == velocity
            && mv.Direction == direction);
        
        var moveCmd = new Move(movableMock);

        Assert.Throws<ArgumentException>(() => moveCmd.Execute());
    }
    
    [Test]
    public void Move_FromPosition_NegInf_ThrowsException()
    {
        var velocity = 1f;
        var direction = Vector2.UnitX;
            
        var movableMock = Mock.Of<IMovable>( mv =>  
            mv.Position == Vector2_NegInf
            && mv.Velocity == velocity
            && mv.Direction == direction);
        
        var moveCmd = new Move(movableMock);

        Assert.Throws<ArgumentException>(() => moveCmd.Execute());
    }
    
    [Test]
    public void Move_Direction_Nan_ThrowsException()
    {
        var startPosition = Vector2.Zero;
        var velocity = 1f;
        
        var movableMock = Mock.Of<IMovable>( mv =>  
            mv.Position == startPosition
            && mv.Velocity == velocity
            && mv.Direction == Vector2_Nan);
        
        var moveCmd = new Move(movableMock);

        Assert.Throws<ArgumentException>(() => moveCmd.Execute());
    }
    
    [Test]
    public void Move_Direction_Inf_ThrowsException()
    {
        var startPosition = Vector2.Zero;
        var velocity = 1f;
        
        var movableMock = Mock.Of<IMovable>( mv =>  
            mv.Position == startPosition
            && mv.Velocity == velocity
            && mv.Direction == Vector2_Inf);
        
        var moveCmd = new Move(movableMock);

        Assert.Throws<ArgumentException>(() => moveCmd.Execute());
    }
    
    [Test]
    public void Move_Direction_NegInf_ThrowsException()
    {
        var startPosition = Vector2.Zero;
        var velocity = 1f;
        
        var movableMock = Mock.Of<IMovable>( mv =>  
            mv.Position == startPosition
            && mv.Velocity == velocity
            && mv.Direction == Vector2_NegInf);
        
        var moveCmd = new Move(movableMock);

        Assert.Throws<ArgumentException>(() => moveCmd.Execute());
    }
    
    [Test]
    public void Move_Velocity_Nan_ThrowsException()
    {
        var startPosition = Vector2.Zero;
        var direction = Vector2.UnitX;

        var movableMock = Mock.Of<IMovable>( mv =>  
            mv.Position == startPosition
            && mv.Velocity == float.NaN
            && mv.Direction == direction);
        
        var moveCmd = new Move(movableMock);

        Assert.Throws<ArgumentException>(() => moveCmd.Execute());
    }
    
    [Test]
    public void Move_Velocity_Inf_ThrowsException()
    {
        var startPosition = Vector2.Zero;
        var direction = Vector2.UnitX;

        var movableMock = Mock.Of<IMovable>( mv =>  
            mv.Position == startPosition
            && mv.Velocity == float.PositiveInfinity
            && mv.Direction == direction);
        
        var moveCmd = new Move(movableMock);

        Assert.Throws<ArgumentException>(() => moveCmd.Execute());
    }
    
    [Test]
    public void Move_Velocity_NegInf_ThrowsException()
    {
        var startPosition = Vector2.Zero;
        var direction = Vector2.UnitX;

        var movableMock = Mock.Of<IMovable>( mv =>  
            mv.Position == startPosition
            && mv.Velocity == float.NegativeInfinity
            && mv.Direction == direction);
        
        var moveCmd = new Move(movableMock);

        Assert.Throws<ArgumentException>(() => moveCmd.Execute());
    }
}

