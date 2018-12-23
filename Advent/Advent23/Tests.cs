using NUnit.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.Advent23
{
    class Tests
    {
        [Test]
        public void IdeaWorks()
        {
            var bot = new NanoBot()
            {
                X = 158723987,
                Y = 0,
                Z = 0,
                SignalStrength = 158723987
            };
            var bot2 = new NanoBot()
            {
                X = -32409809,
                Y = 0,
                Z = 0,
                SignalStrength = 32409809
            };
            var bot3 = new NanoBot()
            {
                X = 0,
                Y = 34987392,
                Z = 0,
                SignalStrength = 34987392
            };

            var me = new NanoBot()
            {
                X = 0,
                Y = 0,
                Z = 0,
                SignalStrength = 0
            };

            Assert.That(me.IsInRangeOf(bot));
            Assert.That(me.IsInRangeOf(bot2));
            Assert.That(me.IsInRangeOf(bot3));

            for (int n = 1; n < 10000000; n *= 10)
            {
                var smallerbot = bot.ReduceByFactor(n);
                var smallerbot2 = bot2.ReduceByFactor(n);
                var smallerbot3 = bot3.ReduceByFactor(n);

                Assert.That(me.IsInRangeOf(smallerbot));
                Assert.That(me.IsInRangeOf(smallerbot2));
                Assert.That(me.IsInRangeOf(smallerbot3));
            }
        }

        [Test]
        public void Example()
        {
            var input = @"pos=<10,12,12>, r=2
pos=<12,14,12>, r=2
pos=<16,12,12>, r=4
pos=<14,14,14>, r=6
pos=<50,50,50>, r=200
pos=<10,10,10>, r=5";

            var solution = new Solution(input, Solution.InputMode.String);

            Assert.AreEqual(36, solution.GetPointInRangeOfMost());
        }

        [Test]
        public void InflateWorks()
        {
            var voxel = new Voxel() { X = 1, Y = 1, Z = 1 };
            var inflated = voxel.Inflate(10);
            Assert.AreEqual(1000, inflated.Count());

            Assert.That(inflated.Any(v => v.X == 10 && v.Y == 10 && v.Z == 10));
            Assert.That(inflated.Any(v => v.X == 12 && v.Y == 12 && v.Z == 12));
            Assert.That(inflated.Any(v => v.X == 19 && v.Y == 19 && v.Z == 19));
            Assert.That(inflated.Any(v => v.X == 10 && v.Y == 19 && v.Z == 10));
            Assert.That(!inflated.Any(v => v.X == 9 && v.Y == 10 && v.Z == 10));
            Assert.That(!inflated.Any(v => v.X == 20 && v.Y == 10 && v.Z == 10));
        }

        [Test]
        public void WhichBotIsTheProblem()
        {
            var voxel = new NanoBot()
            {
                X = 3004710,
                Y = 4599975,
                Z = 4256995
            };

            var solution = new Solution();
            var botsInRange = solution.bots
                .Where(b =>
                {
                    var smallBot = b.ReduceByFactor(100);
                    return voxel.ReduceByFactor(10).IsInRangeOf(smallBot);
                });

            var botsInNextRange = solution.bots
                .Where(b =>
                {
                    var smallBot = b.ReduceByFactor(10);
                    return voxel.IsInRangeOf(smallBot);
                });

            foreach( var bot in botsInNextRange)
            {
                if (!botsInRange.Contains(bot))
                {
                    Assert.Fail("bot " + bot + " is not in the previous result");
                    //bot (17030390, 60459440, 75325221){60231668} 
                }
            }
        }

        [Test]
        public void EdgeShiftsOutward()
        {
            var bot = new NanoBot() { X = 17030390, SignalStrength = 601668 };
            var minimum = bot.X - bot.SignalStrength;
            var maximum = bot.X + bot.SignalStrength;

            var reduced = bot.ReduceByFactor(10);

            var newMinimum = reduced.X - reduced.SignalStrength;
            var newMaximum = reduced.X + reduced.SignalStrength;

            Assert.LessOrEqual(newMinimum, minimum / 10);
            Assert.GreaterOrEqual(newMaximum, maximum / 10);
        }

        [Test]
        public void BotShouldBeInRangeInBoth()
        {
            var voxel = new NanoBot()
            {
                X = 3004710,
                Y = 4599975,
                Z = 4256995
            };

            var bot = new NanoBot()
            {
                X = 17030390,
                Y = 60459440,
                Z = 75325221,
                SignalStrength = 60231668
            };

            var f100 = bot.ReduceByFactor(100);
            var f10 = bot.ReduceByFactor(10);

            Assert.AreEqual(170303, f100.X);
            Assert.AreEqual(604594, f100.Y);
            Assert.AreEqual(753252, f100.Z);
            //Assert.AreEqual(602317, f100.SignalStrength);

            Assert.AreEqual(1703039, f10.X);
            Assert.AreEqual(6045944, f10.Y);
            Assert.AreEqual(7532522, f10.Z);
            //Assert.AreEqual(6023167, f10.SignalStrength);

            var range10 = voxel.DistanceTo(f10);
            var range100 = voxel.ReduceByFactor(10).DistanceTo(f100);

            var isInRange10 = voxel.IsInRangeOf(f10);
            var isInRange100 = voxel.IsInRangeOf(f100);

            Assert.That(isInRange100);
            Assert.That(isInRange10);
        }

        [Test]
        public void IsTheBotInRange()
        {
            var voxel = new NanoBot()
            {
                X = 3004710,
                Y = 4599975,
                Z = 4256995
            };

            var inflated = voxel.Inflate(10);

            var bot = new NanoBot()
            {
                X = 17030390,
                Y = 60459440,
                Z = 75325221,
                SignalStrength = 60231668
            };

            var inRange = inflated.Where(i => i.IsInRangeOf(bot));
            var count = inRange.Count();
            Assert.Greater(count, 0);
        }

        [Test]
        public void NogEenDing()
        {
            var sol = new Solution(@"pos=<17030390,60459440,75325221>, r=60231668
pos=<30047102,45999757,42569950>, r=1", Solution.InputMode.String);

            var inflateFactor = 1000000;
            var inflateShift = 10;
            var voxelsAt = new ConcurrentStack<Voxel>[1001];

            sol.InitVoxels(voxelsAt, inflateFactor);
            var highest = sol.GetHighest(voxelsAt);

            Voxel target = new Voxel() { X = 30, Y = 45, Z = 42 };
            Assert.Less(highest.Count, 8);
            Assert.That(highest.Any(h => h.X == target.X && h.Y == target.Y && h.Z == target.Z));
            Assert.That(highest.All(h => h.DistanceTo(target) < 2));

            var shifted = inflateFactor / inflateShift; // 100000
            sol.RunRound(voxelsAt, shifted, inflateShift);
            highest = sol.GetHighest(voxelsAt);

            target = new Voxel() { X = 300, Y = 459, Z = 425 };
            Assert.Less(highest.Count, 8);
            Assert.That(highest.Any(h => h.X == target.X && h.Y == target.Y && h.Z == target.Z));
            Assert.That(highest.All(h => h.DistanceTo(target) < 2));

            shifted = shifted / inflateShift; // 10000
            sol.RunRound(voxelsAt, shifted, inflateShift);
            highest = sol.GetHighest(voxelsAt);

            target = new Voxel() { X = 3004, Y = 4599, Z = 4256 };
            Assert.Less(highest.Count, 8);
            Assert.That(highest.Any(h => h.X == target.X && h.Y == target.Y && h.Z == target.Z));
            Assert.That(highest.All(h => h.DistanceTo(target) < 2));

            shifted = shifted / inflateShift; // 1000
            sol.RunRound(voxelsAt, shifted, inflateShift);
            highest = sol.GetHighest(voxelsAt);

            target = new Voxel() { X = 30047, Y = 45999, Z = 42569 }; // 1000, hier gaat ie fout
            Assert.Less(highest.Count, 8);
            Assert.That(highest.Any(h => h.X == target.X && h.Y == target.Y && h.Z == target.Z));
            Assert.That(highest.All(h => h.DistanceTo(target) < 2));
        }
    }
}
