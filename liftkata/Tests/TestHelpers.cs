using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

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
    }
}