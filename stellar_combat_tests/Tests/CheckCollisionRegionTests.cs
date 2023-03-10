using System.Numerics;
using IoC;
using IoC.Commands;
using IoC.Interfaces;
using Moq;
using NUnit.Framework;
using StellarCombat.Commands;
using StellarCombat.Interfaces;
using TestStrategy;

[TestFixture]
public class CheckCollisionRegionTests
{
    private const int ObjectsCount = 5;
    
    private class CollidableStub : ICollidable
    {
        public Vector2 Position { get; set; }
        public ICommand CheckCollisionCmd { get; set; }
    }

    [SetUp]
    public void BeforeTest()
    {
        new InitializeStrategyCmd(IocTestStrategy.GetStrategy).Execute();
        Container.Resolve<ICommand>("IoC.Register", "Commands.CheckCollision", (Func<object[], object>)((args) => Mock.Of<ICommand>())).Execute();
    }

    [Test]
    public void CheckCollisionRegion_Execute_CollidableInRegion_CreatesMacro()
    {
        var region = SetupRegionMock(ObjectsCount);
        region.Setup(r => r.IsInRegion(It.IsAny<ICollidable>())).Returns(true);

        var collidable = new CollidableStub();
        
        var checkRegionCmd = new CheckCollisionRegion(collidable, region.Object,
            Mock.Of<ICommander>());
        
        checkRegionCmd.Execute();
        
        Assert.IsTrue(collidable.CheckCollisionCmd is Macro);
    }
    
    [Test]
    public void CheckCollisionRegion_Execute_CollidableNotInRegion_CreatesMacro()
    {
        var regionA = SetupRegionMock(ObjectsCount);
        regionA.Setup(r => r.IsInRegion(It.IsAny<ICollidable>())).Returns(false);

        var regionB = SetupRegionMock(ObjectsCount);
        regionA.Setup(r => r.GetObjectRegion(It.IsAny<Vector2>())).Returns(regionB.Object);
        
        var collidable = new CollidableStub();

        var checkRegionCmd = new CheckCollisionRegion(collidable, regionA.Object,
            Mock.Of<ICommander>());
        
        checkRegionCmd.Execute();
        
        Assert.IsTrue(collidable.CheckCollisionCmd is Macro);
    }

    [Test]
    public void CheckCollisionRegion_Execute_CollidableNotInRegion_RemoveMethodCalled()
    {
        var region = SetupRegionMock(ObjectsCount);
        region.Setup(r => r.IsInRegion(It.IsAny<ICollidable>())).Returns(false);
        region.Setup(r => r.Remove(It.IsAny<ICollidable>())).Verifiable();
        region.Setup(r => r.GetObjectRegion(It.IsAny<Vector2>())).Returns(SetupRegionMock(ObjectsCount).Object);

        var checkRegionCmd = new CheckCollisionRegion(Mock.Of<ICollidable>(), region.Object,
            Mock.Of<ICommander>());
        
        checkRegionCmd.Execute();

        region.Verify();
    }
    
    [Test]
    public void CheckCollisionRegion_Execute_CollidableNotInRegion_AddMethodCalled()
    {
        var regionA = SetupRegionMock(ObjectsCount);
        regionA.Setup(r => r.IsInRegion(It.IsAny<ICollidable>())).Returns(false);

        var regionB = SetupRegionMock(ObjectsCount);
        regionB.Setup(r => r.Add(It.IsAny<ICollidable>())).Verifiable();

        regionA.Setup(r => r.GetObjectRegion(It.IsAny<Vector2>())).Returns(regionB.Object);
 
        var checkRegionCmd = new CheckCollisionRegion(Mock.Of<ICollidable>(), regionA.Object,
            Mock.Of<ICommander>());
        
        checkRegionCmd.Execute();

        regionB.Verify();
    }

    //this test does not make much sense
    //apart from showing several collision regions
    [Test]
    public void CheckCollisionRegion_WithMultipleRegions()
    {
        //let this region be the base collision system region
        var region = SetupRegionMock(ObjectsCount);
        region.Setup(r => r.IsInRegion(It.IsAny<ICollidable>())).Returns(true);
        
        //let this region be the collision system region with offset
        var regionWithOffset = SetupRegionMock(ObjectsCount);
        regionWithOffset.Setup(r => r.IsInRegion(It.IsAny<ICollidable>())).Returns(true);

        var collidable = Mock.Of<ICollidable>();
        var commander = Mock.Of<ICommander>();
        
        var checkRegionCmd0 = new CheckCollisionRegion(collidable, region.Object,
            commander);
        
        var checkRegionCmd1 = new CheckCollisionRegion(
            collidable, regionWithOffset.Object,
            commander);
        
        checkRegionCmd0.Execute();
        checkRegionCmd1.Execute();
        
        Assert.IsTrue(collidable.CheckCollisionCmd is Macro);
    }
    
    private Mock<ICollisionRegion> SetupRegionMock(int objectsCount)
    {
        var collidables = new List<ICollidable>();
        
        for (var i = 0; i < objectsCount; i++)
        {
            collidables.Add(Mock.Of<ICollidable>());
        }
        
        var regionMock = new Mock<ICollisionRegion>();
        regionMock.SetupGet(r => r.Objects).Returns(collidables);
        regionMock.SetupGet(r => r.ObjectsCount).Returns(collidables.Count);

        return regionMock;
    }
}