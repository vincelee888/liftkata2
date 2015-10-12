﻿using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace liftkata
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
            var sut = new LiftController(10, this);
            
            _listener = sut;

            sut.Summon(1);
            PassTime(9);
            Assert.That(_stopsVisited.Contains(1), Is.True);
        }

        [Test]
        public void SummonedFromBelow_LiftMovesExpectedNumberOfFloors()
        {
            const int startingFloor = 10;
            var sut = new LiftController(startingFloor, this);

            _listener = sut;

            const int floorSummonedFrom = 1;
            sut.Summon(floorSummonedFrom);
            PassTime(startingFloor - floorSummonedFrom);
            Assert.That(_movesDownward, Is.EqualTo(startingFloor - floorSummonedFrom));
        }

        [Test]
        public void SummonedFromAbove_LiftMovesExpectedNumberOfFloors()
        {
            const int startingFloor = 1;
            var sut = new LiftController(startingFloor, this);

            _listener = sut;

            const int floorSummonedFrom = 10;
            sut.Summon(floorSummonedFrom);
            PassTime(floorSummonedFrom - startingFloor);
            Assert.That(_movesUpward, Is.EqualTo(floorSummonedFrom - startingFloor));
        }

        [Test]
        public void SummonedFromAbove_RequestLowerFloor_LiftMovesExpectedNumberOfFloors()
        {
            const int startingFloor = 1;
            var sut = new LiftController(startingFloor, this);

            _listener = sut;

            const int floorSummonedFrom = 10;
            sut.Summon(floorSummonedFrom);

            PassTime(floorSummonedFrom - startingFloor);

            sut.Request(startingFloor);

            PassTime(floorSummonedFrom - startingFloor);

            Assert.That(_movesUpward, Is.EqualTo((floorSummonedFrom - startingFloor)));
            Assert.That(_movesDownward, Is.EqualTo((floorSummonedFrom - startingFloor)));
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