﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace liftkata
{
    public class LiftController : IListenForTimePassing
    {
        private int _currentFloor;
        private readonly IListenToLifts _listener;
        private int _floorSummonedFrom;
        private int _requestedFloor;
        private readonly List<int> _floorsToVisit;

        public LiftController(int currentFloor, IListenToLifts listener)
        {
            _currentFloor = currentFloor;
            _listener = listener;
            _floorsToVisit = new List<int>();
        }

        public void Summon(int floorSummonedFrom)
        {
            _floorSummonedFrom = floorSummonedFrom;
            _floorsToVisit.Add(floorSummonedFrom);
        }

        public void Request(int requestedFloor)
        {
            _requestedFloor = requestedFloor;
            _floorsToVisit.Add(requestedFloor);
        }

        public void Tick()
        {
            var targetFloor = GetNextFloor();
            Move(targetFloor);
            if (_currentFloor == targetFloor) AtCurrentFloor();
        }

        private int GetNextFloor()
        {
            if (!_floorsToVisit.Any()) return _currentFloor;
            var targetFloor = _floorsToVisit.First();
            return targetFloor;
        }

        private void AtCurrentFloor()
        {
            _listener.LiftArrived(_currentFloor);
            _floorsToVisit.RemoveAt(0);
        }

        private void Move(int targetFloor)
        {
            var increment = _currentFloor < targetFloor ? 1 : -1;
            Action publishedEvent = _listener.LiftMovedDownwards;
            if (_currentFloor < targetFloor) publishedEvent = _listener.LiftMovedUpwards;

            _currentFloor += increment;
            publishedEvent();
        }
    }
}