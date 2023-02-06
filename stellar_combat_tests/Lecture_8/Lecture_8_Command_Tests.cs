using System.Numerics;
using IoC.Interfaces;
using Moq;
using NUnit.Framework;
using StellarCombat;
using StellarCombat.Commands;
using StellarCombat.ExceptionHandling.Exceptions;
using StellarCombat.Interfaces;

[TestFixture]
public class Lecture_8_Command_Tests
{
    private const float DegToRad_90 = 1.5708f;
    
    [Test]
    public void Move_Straight()
    {
        var fuelConsumerMock = Mock.Of<IFuelConsumer>(fc => 
            fc.FuelAmount == 100 && 
            fc.ConsumeSpeed == 1);
        
        var movableMock = Mock.Of<IMovable>(mv =>
            mv.Direction == Vector2.UnitX &&
            mv.Position == Vector2.Zero &&
            mv.Velocity == 1f);

        var moveSequence = new CommandSequence(new ICommand[]
        {
            new CheckFuel(fuelConsumerMock),
            new ConsumeFuel(fuelConsumerMock),
            new Move(movableMock)
        });
        
        moveSequence.Execute();
        
        var expectedPosition = Vector2.UnitX;
        Assert.AreEqual(expectedPosition, movableMock.Position);
    }
    
    [Test]
    public void Move_Straight_Fuel_Consumption()
    {
        var fuelConsumerMock = Mock.Of<IFuelConsumer>(fc => 
            fc.FuelAmount == 100 && 
            fc.ConsumeSpeed == 1);
        
        var movableMock = Mock.Of<IMovable>(mv =>
            mv.Direction == Vector2.UnitX &&
            mv.Position == Vector2.Zero &&
            mv.Velocity == 1f);

        var moveSequence = new CommandSequence(new ICommand[]
        {
            new CheckFuel(fuelConsumerMock),
            new ConsumeFuel(fuelConsumerMock),
            new Move(movableMock)
        });
        
        moveSequence.Execute();

        var expectedFuel = 99;
        Assert.AreEqual(expectedFuel, fuelConsumerMock.FuelAmount);
    }

    [Test]
    public void Move_Straight_Insufficient_Fuel()
    {
        var fuelConsumerMock = Mock.Of<IFuelConsumer>(fc => 
            fc.FuelAmount == 5 && 
            fc.ConsumeSpeed == 10);
        
        var movableMock = Mock.Of<IMovable>(mv =>
            mv.Direction == Vector2.UnitX &&
            mv.Position == Vector2.Zero &&
            mv.Velocity == 1f);

        var moveSequence = new CommandSequence(new ICommand[]
        {
            new CheckFuel(fuelConsumerMock),
            new ConsumeFuel(fuelConsumerMock),
            new Move(movableMock)
        });

        Assert.Throws<CommandException>(moveSequence.Execute);
    }
    
    [Test]
    public void Rotate_And_Move()
    {
        var rotatableMock = new Mock<IRotatable>();

        rotatableMock.SetupProperty(rt => rt.Direction, Vector2.UnitX);
        rotatableMock.SetupGet(rt => rt.AngularVelocity).Returns(DegToRad_90);

        var movableMock = rotatableMock.As<IMovable>();
        movableMock.SetupProperty(mv => mv.Position, Vector2.Zero);
        movableMock.SetupGet(mv => mv.Velocity).Returns(1f);

        //rotate 90 degrees counterclockwise --> move one unit up
        var rotateAndMoveSequence = new CommandSequence(new ICommand[]
        {
            new Rotate(rotatableMock.Object),
            new Move(movableMock.Object)
        });
        
        rotateAndMoveSequence.Execute();
        
        var expectedPosition = Vector2.UnitY;
        Assert.AreEqual(expectedPosition, movableMock.Object.Position);
    }
    
    //When a repeatable command is queued it's expected
    //that some class is going to execute Commander.ExecuteAll()
    //on some fixed interval
    [Test]
    public void Continious_Movement()
    {
        var fuelConsumerMock = Mock.Of<IFuelConsumer>(fc => 
            fc.FuelAmount == 5 && 
            fc.ConsumeSpeed == 10);
        
        var movableMock = Mock.Of<IMovable>(mv =>
            mv.Direction == Vector2.UnitX &&
            mv.Position == Vector2.Zero &&
            mv.Velocity == 1f);
        
        var handler = Mock.Of<ICommandExceptionHandler>();
        var commander = new Commander(handler);
        
        var moveSequence = new CommandSequence(new ICommand[]
        {
            new CheckFuel(fuelConsumerMock),
            new ConsumeFuel(fuelConsumerMock),
            new Move(movableMock)
        });
        
        var macro = new Macro(commander, moveSequence);
        
        commander.Enqueue(macro);
        commander.ExecuteAll();

        var isCommandInQueue = commander.IsInQueue(moveSequence);
        
        Assert.IsTrue(isCommandInQueue);
    }
}