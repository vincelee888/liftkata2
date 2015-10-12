using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace liftkata
{
    [TestFixture]
    public class Class1 : IListenToLifts
    {
        private readonly List<int> _stopsVisited;

        public Class1()
        {
            _stopsVisited = new List<int>();
        }

        [Test]
        public void SummonedFromBelow_OpensAtFloorSummonedFrom()
        {
            var sut = new LiftController(10, 10, this);
            sut.Summon(0);
            Assert.That(_stopsVisited.Contains(0), Is.True);
        }

        public void LiftArrived(int stop)
        {
            _stopsVisited.Add(stop);
        }
    }

    public class LiftController
    {
        private readonly int _totalStops;
        private readonly int _startingFloor;
        private readonly IListenToLifts _listener;

        public LiftController(int totalStops, int startingFloor, IListenToLifts listener)
        {
            _totalStops = totalStops;
            _startingFloor = startingFloor;
            _listener = listener;
        }

        public void Summon(int floorSummonedFrom)
        {
            _listener.LiftArrived(floorSummonedFrom);
        }
    }

    public interface IListenToLifts
    {
        void LiftArrived(int stop);
    }
}
