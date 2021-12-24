using Advent2021.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent23
{
    class Tests
    {
        [TestCase(example, 12521)]
        public void Test1(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase(examplept2, 44169)]
        public void Test2(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase(complete, 0)]
        [TestCase(twoAway, 44)]
        [TestCase(example, 8499)]
        public void TestHeuristicFromStart(string input, long heuristic)
        {
            var sol = new Solution(input);

            Assert.AreEqual(heuristic, sol.startingState.HeuristicDistance);
        }

        [TestCase(twoAway, 42)] // the boys in the rear can move through their friends in target position
        [TestCase(example, 28)]
        public void TestNewStateCountFromStart(string input, int num)
        {
            var sol = new Solution(input);

            var queue = new NetTopologySuite.Utilities.PriorityQueue<State>();
            sol.startingState.GenerateTransitions(queue);

            Assert.AreEqual(num, queue.Size);
        }

        [Test]
        public void HeuristicTest()
        {
            var sol = new Solution(twoAway);

            var queue = new NetTopologySuite.Utilities.PriorityQueue<State>();
            var toMove = sol.startingState.Occupier[12];
            sol.startingState.AddMove(queue, toMove, 5);

            var movedState = queue.Poll();

            Assert.AreEqual(46, movedState.TotalCost);

            sol.startingState.AddMove(queue, toMove, 3);

            movedState = queue.Poll();

            Assert.AreEqual(44, movedState.TotalCost);
        }

        public enum Amphipods { A, a, B, b, C, c, D, d }

        [Test]
        public void BadStep()
        {
            var sol = new Solution("Input.txt");
            

            var state = sol.startingState;
            Assert.AreEqual("...........  D B b A  C c d a", state.Inline());
            state = DoStep(state, Amphipods.b, 5);
            Assert.AreEqual(".....b.....  D B . A  C c d a", state.Inline());
            state = DoStep(state, Amphipods.d, 13);
            Assert.AreEqual(".....b.....  D B d A  C c . a", state.Inline());
            state = DoStep(state, Amphipods.A, 7);
            Assert.AreEqual(".....b.A...  D B d .  C c . a", state.Inline());
            state = DoStep(state, Amphipods.a, 14);
            Assert.AreEqual(".....b.A...  D B d a  C c . .", state.Inline());
            state = DoStep(state, Amphipods.B, 3);
            Assert.AreEqual("...B.b.A...  D . d a  C c . .", state.Inline());
            state = DoStep(state, Amphipods.c, 12);
            Assert.AreEqual("...B.b.A...  D c d a  C . . .", state.Inline());

            var queue = new NetTopologySuite.Utilities.PriorityQueue<State>();
            state.GenerateTransitions(queue, (int)Amphipods.b);

            foreach(var newState in queue)
            {
                // b 5 -> 16 !
                Assert.AreNotEqual("...B...A...  D c d a  C b . .", newState.Inline());
            }
        }

        private State DoStep(State state, Amphipods amphipod, int destination)
        {
            var queue = new NetTopologySuite.Utilities.PriorityQueue<State>();
            state.AddMove(queue, (int)amphipod, destination);

            return queue.Poll();
        }

        public const string complete = @"1
#############
#...........#
###A#B#C#D###
  #A#B#C#D#
  #########";

        public const string twoAway = @"2
#############
#...........#
###B#A#C#D###
  #A#B#C#D#
  #########";

        // move A on 12 to 3
        //#############
        //#...A.......# moved 2, cost = 2, heuristic = 2 + 40, totatl = 44
        //###B#.#C#D###
        //  #A#B#C#D#
        //  #########

        // move A on 12 to 5
        //#############
        //#.....A.....# moved 2, cost = 2, heuristic = 4 + 40, totatl = 46
        //###B#.#C#D###
        //  #A#B#C#D#
        //  #########

        // heuristic is:
        // 9 for A on 18 -> 11
        // 0 for A on 15 -> 15
        // 40 for B on 11 -> 12
        // 50 for B on 13 -> 16
        // 400 for C on 12 -> 13
        // 0 for C on 17 -> 17
        // 0 for D on 14 -> 14
        // 8000 for D on 16 -> 18
        // total = 8000 + 600 + 40 + 50 + 9 = 8499
        public const string example = @"
#############
#...........#
###B#C#B#D###
  #A#D#C#A#
  #########";

        public const string examplept2 = @"
#############
#...........#
###B#C#B#D###
  #D#C#B#A#
  #D#B#A#C#
  #A#D#C#A#
  #########";

        // move B on 11 to 0
        //#############
        //#B..........# moved 11 -> 0, cost = 30
        //###.#C#B#D###
        //  #A#D#C#A#
        //  #########
        // heuristic is:
        // 9 for A on 18 -> 11
        // 0 for A on 15 -> 15
        // 50 for B on 0 -> 12
        // 50 for B on 13 -> 16
        // 400 for C on 12 -> 13
        // 0 for C on 17 -> 17
        // 0 for D on 14 -> 14
        // 8000 for D on 16 -> 18
        // total heuristic = 8000 + 600 + 50 + 50 + 9 = 8519
        // total = 30 + 8519 = 8549
    }
}
