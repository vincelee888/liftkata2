using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using liftkata.Implementation;

namespace liftkata.Tests
{
    static internal class TestHelpers
    {
        public static void AssertThatLiftStopsAtFloor(this ICollection<int> stopsVisited, int floorSummonedFrom)
        {
            Assert.That(stopsVisited.Contains(floorSummonedFrom), Is.True, "Didn't stop at " + floorSummonedFrom);
        }

        public static void AssertThatLiftStopsAtFloor(this ICollection<int> stopsvisited, int[] floorSummonedFrom)
        {
            floorSummonedFrom.ToList().ForEach(stopsvisited.AssertThatLiftStopsAtFloor);
        }

        public static void PassTime(IListenForTimePassing listener, int ticks)
        {
            for (var i = 0; i < ticks; i++)
            {
                listener.Tick();
            }
        }

        public static void PassTime(IListenForTimePassing listener, int floorA, int floorB)
        {
            PassTime(listener, Math.Abs(floorA - floorB));
        }
    }
}