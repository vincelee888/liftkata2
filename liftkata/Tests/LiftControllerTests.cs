using System.Collections.Generic;
using NUnit.Framework;
using liftkata.Implementation;

namespace liftkata.Tests
{
    [TestFixture]
    public class LiftControllerTests : IListenToLifts
    {
        private List<int> _stopsVisited;

        [SetUp]
        public void Setup()
        {
            _stopsVisited = new List<int>();
        }

        [Test]
        public void SummonedFromBelow_OpensAtFloorSummonedFrom()
        {
            const int startingFloor = 10;
            var sut = new LiftController(startingFloor, this);

            const int floorSummonedFrom = 1;
            sut.Summon(floorSummonedFrom);
            TestHelpers.PassTime(sut, startingFloor, floorSummonedFrom);
            _stopsVisited.AssertThatLiftStopsAtFloor(floorSummonedFrom);
        }

        [Test]
        public void SummonedFromAbove_OpensAtFloorSummonedFrom()
        {
            const int startingFloor = 1;
            var sut = new LiftController(startingFloor, this);

            const int floorSummonedFrom = 10;
            sut.Summon(floorSummonedFrom);
            TestHelpers.PassTime(sut, floorSummonedFrom, startingFloor);
            Assert.That(_stopsVisited.Contains(floorSummonedFrom), Is.True);
        }

        [Test]
        public void SummonedFromAbove_RequestLowerFloor_LiftVisitsBothFloors()
        {
            const int startingFloor = 1;
            var sut = new LiftController(startingFloor, this);

            const int floorSummonedFrom = 10;
            sut.Summon(floorSummonedFrom);

            TestHelpers.PassTime(sut, floorSummonedFrom, startingFloor);

            const int requestedFloor = startingFloor;
            sut.Request(requestedFloor);

            TestHelpers.PassTime(sut, floorSummonedFrom, requestedFloor);

            _stopsVisited.AssertThatLiftStopsAtFloor(new[] { floorSummonedFrom, requestedFloor });
        }

        [Test]
        public void SummonedFromBelow_RequestHigherFloor_LiftVisitsBothFloors()
        {
            const int startingFloor = 10;
            var sut = new LiftController(startingFloor, this);

            const int floorSummonedFrom = 1;
            sut.Summon(floorSummonedFrom);

            TestHelpers.PassTime(sut, floorSummonedFrom, startingFloor);

            const int requestedFloor = startingFloor;
            sut.Request(requestedFloor);

            TestHelpers.PassTime(sut, floorSummonedFrom, requestedFloor);

            _stopsVisited.AssertThatLiftStopsAtFloor(new[] { floorSummonedFrom, requestedFloor });
        }

        void IListenToLifts.LiftArrived(int stop)
        {
            _stopsVisited.Add(stop);
        }

        void IListenToLifts.LiftMovedDownwards()
        {
        }

        void IListenToLifts.LiftMovedUpwards()
        {
        }
    }
}
