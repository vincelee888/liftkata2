﻿using System.Collections.Generic;
using NUnit.Framework;
using liftkata.Implementation;

namespace liftkata.Tests
{
    [TestFixture]
    public class LiftControllerTests : IListenToLifts
    {
        private List<int> _stopsVisited;
        private int _movesDownward;
        private int _movesUpward;
        private IListenForTimePassing _listener;

        [SetUp]
        public void Setup()
        {
            _stopsVisited = new List<int>();
            _movesUpward = 0;
            _movesDownward = 0;
        }

        [Test]
        public void SummonedFromBelow_OpensAtFloorSummonedFrom()
        {
            const int startingFloor = 10;
            var sut = new LiftController(startingFloor, this);

            _listener = sut;

            const int floorSummonedFrom = 1;
            sut.Summon(floorSummonedFrom);
            PassTime(startingFloor - floorSummonedFrom);
            _stopsVisited.AssertThatLiftStopsAtFloor(floorSummonedFrom);
        }

        [Test]
        public void SummonedFromAbove_OpensAtFloorSummonedFrom()
        {
            const int startingFloor = 1;
            var sut = new LiftController(startingFloor, this);

            _listener = sut;

            const int floorSummonedFrom = 10;
            sut.Summon(floorSummonedFrom);
            PassTime(floorSummonedFrom - startingFloor);
            Assert.That(_stopsVisited.Contains(floorSummonedFrom), Is.True);
        }

        [Test]
        public void SummonedFromAbove_RequestLowerFloor_LiftVisitsBothFloors()
        {
            const int startingFloor = 1;
            var sut = new LiftController(startingFloor, this);

            _listener = sut;

            const int floorSummonedFrom = 10;
            sut.Summon(floorSummonedFrom);

            PassTime(floorSummonedFrom - startingFloor);

            const int requestedFloor = startingFloor;
            sut.Request(requestedFloor);

            PassTime(floorSummonedFrom - requestedFloor);

            _stopsVisited.AssertThatLiftStopsAtFloor(new[] { floorSummonedFrom, requestedFloor});
        }

        void IListenToLifts.LiftArrived(int stop)
        {
            _stopsVisited.Add(stop);
        }

        void IListenToLifts.LiftMovedDownwards()
        {
            _movesDownward++;
        }

        void IListenToLifts.LiftMovedUpwards()
        {
            _movesUpward++;
        }

        private void PassTime(int ticks)
        {
            for(var i = 0; i < ticks; i++)
            {
                _listener.Tick();
            }
        }
    }
}
