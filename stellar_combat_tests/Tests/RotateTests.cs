using System.Numerics;
using Moq;
using NUnit.Framework;
using StellarCombat.Commands;
using StellarCombat.Interfaces;

[TestFixture]
public class RotateTests
{
    private static readonly Vector2 Vector2_Nan = new Vector2(1f, float.NaN);
    private static readonly Vector2 Vector2_Inf = new Vector2(1f, float.PositiveInfinity);
    private static readonly Vector2 Vector2_NegInf = new Vector2(1f, float.NegativeInfinity);
    
    private const float DegToRad_90 = 1.5708f;
    private const float DegToRad_360 = 6.28319f;
    
    [Test]
    public void Rotate_90Deg_Clockwise()
    {
        var startDirection = Vector2.UnitX;
        var angularVelocity = -DegToRad_90;

        var rotatableMock = Mock.Of<IRotatable>(rt =>
            rt.Direction == startDirection
            && rt.AngularVelocity == angularVelocity);

        var rotateCmd = new Rotate(rotatableMock);
        rotateCmd.Execute();

        var expectedDirection = new Vector2(0f, -1f);
        Assert.AreEqual(expectedDirection, rotatableMock.Direction);
    }
    
    [Test]
    public void Rotate_90Deg_CtClockwise()
    {
        var startDirection = Vector2.UnitX;
        var angularVelocity = DegToRad_90;

        var rotatableMock = Mock.Of<IRotatable>(rt =>
            rt.Direction == startDirection
            && rt.AngularVelocity == angularVelocity);

        var rotateCmd = new Rotate(rotatableMock);
        rotateCmd.Execute();

        var expectedDirection = Vector2.UnitY;
        Assert.AreEqual(expectedDirection, rotatableMock.Direction);
    }

    [Test]
    public void Rotate_0Deg()
    {
        var startDirection = Vector2.UnitX;
        var angularVelocity = 0f;

        var rotatableMock = Mock.Of<IRotatable>(rt =>
            rt.Direction == startDirection
            && rt.AngularVelocity == angularVelocity);

        var rotateCmd = new Rotate(rotatableMock);
        rotateCmd.Execute();

        var expectedDirection = Vector2.UnitX;
        Assert.AreEqual(expectedDirection, rotatableMock.Direction);
    }
    
    [Test]
    public void Rotate_360Deg_Clockwise()
    {
        var startDirection = Vector2.UnitX;
        var angularVelocity = DegToRad_360;

        var rotatableMock = Mock.Of<IRotatable>(rt =>
            rt.Direction == startDirection
            && rt.AngularVelocity == angularVelocity);

        var rotateCmd = new Rotate(rotatableMock);
        rotateCmd.Execute();

        var expectedDirection = Vector2.UnitX;
        Assert.AreEqual(expectedDirection, rotatableMock.Direction);
    }
    
    [Test]
    public void Rotate_360Deg_CtClockwise()
    {
        var startDirection = Vector2.UnitX;
        var angularVelocity = -DegToRad_360;

        var rotatableMock = Mock.Of<IRotatable>(rt =>
            rt.Direction == startDirection
            && rt.AngularVelocity == angularVelocity);

        var rotateCmd = new Rotate(rotatableMock);
        rotateCmd.Execute();

        var expectedDirection = Vector2.UnitX;
        Assert.AreEqual(expectedDirection, rotatableMock.Direction);
    }
    
    [Test]
    public void Rotate_NullObject_ThrowsException()
    {
        var rotateCmd = new Rotate(null);
        Assert.Throws<NullReferenceException>(() => rotateCmd.Execute());
    }

    [Test]
    public void Rotate_Direction_NaN_ThrowsException()
    {
        var angularVelocity = DegToRad_90;

        var rotatableMock = Mock.Of<IRotatable>(rt =>
            rt.Direction == Vector2_Nan
            && rt.AngularVelocity == angularVelocity);

        var rotateCmd = new Rotate(rotatableMock);

        Assert.Throws<ArgumentException>(() => rotateCmd.Execute());
    }
    
    [Test]
    public void Rotate_Direction_Inf_ThrowsException()
    {
        var angularVelocity = DegToRad_90;

        var rotatableMock = Mock.Of<IRotatable>(rt =>
            rt.Direction == Vector2_Inf
            && rt.AngularVelocity == angularVelocity);

        var rotateCmd = new Rotate(rotatableMock);

        Assert.Throws<ArgumentException>(() => rotateCmd.Execute());
    }
    
    [Test]
    public void Rotate_Direction_NegInf_ThrowsException()
    {
        var angularVelocity = DegToRad_90;

        var rotatableMock = Mock.Of<IRotatable>(rt =>
            rt.Direction == Vector2_NegInf
            && rt.AngularVelocity == angularVelocity);

        var rotateCmd = new Rotate(rotatableMock);

        Assert.Throws<ArgumentException>(() => rotateCmd.Execute());
    }
    
    [Test]
    public void Rotate_Velocity_NaN_ThrowsException()
    {
        var direction = Vector2.UnitX;

        var rotatableMock = Mock.Of<IRotatable>(rt =>
            rt.Direction == direction
            && rt.AngularVelocity == float.NaN);

        var rotateCmd = new Rotate(rotatableMock);

        Assert.Throws<ArgumentException>(() => rotateCmd.Execute());
    }
    
    [Test]
    public void Rotate_Velocity_Inf_ThrowsException()
    {
        var direction = Vector2.UnitX;

        var rotatableMock = Mock.Of<IRotatable>(rt =>
            rt.Direction == direction
            && rt.AngularVelocity == float.PositiveInfinity);

        var rotateCmd = new Rotate(rotatableMock);

        Assert.Throws<ArgumentException>(() => rotateCmd.Execute());
    }
    
    [Test]
    public void Rotate_Velocity_NegInf_ThrowsException()
    {
        var direction = Vector2.UnitX;

        var rotatableMock = Mock.Of<IRotatable>(rt =>
            rt.Direction == direction
            && rt.AngularVelocity == float.NegativeInfinity);

        var rotateCmd = new Rotate(rotatableMock);

        Assert.Throws<ArgumentException>(() => rotateCmd.Execute());
    }
}