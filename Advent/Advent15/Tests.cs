using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Advent.Advent15
{
    class Tests
    {
        [TestCase("Test", 47, 590)]
        [TestCase("Test2", 37, 982)]
        [TestCase("Test3", 46, 859)]
        [TestCase("Test4", 35, 793)]
        [TestCase("Test5", 54, 536)]
        [TestCase("Test6", 20, 937)]
        public void GivenTest(string input, int turn, int hps)
        {
            var solution = new Solution(input, Solution.InputMode.File);

            var result = solution.Run();
            Assert.IsTrue(result.hasResult);
            Assert.AreEqual(turn, result.turn);
            Assert.AreEqual(hps, result.hps);
        }

        [Test]
        public void AnswerIsNotWrong()
        {
            var solution = new Solution();
            var result = solution.Run();

            var score = result.turn * result.hps;

            Assert.AreEqual(0, score);
            Assert.AreNotEqual(203426, score);
        }

        public Solution TakeStepFrom(string input)
        {
            var solution = new Solution(input, Solution.InputMode.String);
            solution.Step();

            return solution;
        }

        public string FlipXY(string input)
        {
            var split = input.Split('\n');
            var splitResult = new char[split[0].Length,split.Length];
            for (int y = 0; y < split.Length; y++)
            {
                for (int x = 0; x < split[y].Length; x++)
                {
                    splitResult[x, y] = split[y][x];
                }
            }
            var sb = new StringBuilder();
            for (int y = 0; y<= splitResult.GetUpperBound(0); y++)
            {
                for (int x = 0; x <= splitResult.GetUpperBound(1); x++)
                {
                    sb.Append(splitResult[y, x]);
                }
                if (y != splitResult.GetUpperBound(0))sb.Append('\n');
            }

            return sb.ToString();
        }

        public string FlipUpsideDown(string input)
        {
            var split = input.Split('\n');

            var sb = new StringBuilder();
            for (int y = split.Length -1; y >= 0; y--)
            {
                sb.Append(split[y]);

                if (y != 0) sb.Append('\n');
            }

            return sb.ToString();
        }

        public string FlipLeftRight(string input)
        {
            var split = input.Split('\n');

            var sb = new StringBuilder();
            for(int y = 0; y < split[0].Length; y++)
            {
                foreach (var c in split[y].Reverse()) sb.Append(c);

                if (y != input.Length - 1) sb.Append('\n');
            }

            return sb.ToString();
        }

        public string Parse(string readable)
        {
            readable = Regex.Replace(readable, "[0-9]", ".");
            return readable
                .Replace("X", "G")
                .Replace("+", ".")
                .Replace("*", ".")
                .Replace(" ", "").Replace("\t", "")
                .Replace("\r", "");
        }

        [Test]
        public void WillWalkAndNotTeleport()
        {
            var solution = TakeStepFrom("e...G");

            Assert.AreEqual("E..G.", solution.ToString());
        }

        [Test]
        public void WillGoLeftBeforeRight()
        {
            var solution = TakeStepFrom("e.G.e");

            Assert.AreEqual("EG..E", solution.ToString());
        }

        [Test]
        public void WillGoUpBeforeDown()
        {
            var input = FlipXY("e.G.e");

            var solution = TakeStepFrom(input);

            Assert.AreEqual(FlipXY("EG..E"), solution.ToString());
        }

        [TestCase("e.G..e", "EG...E", false)]
        [TestCase("e..G.e", "E...GE", false)]
        [TestCase("e.G..e", "EG...E", true)]
        [TestCase("e..G.e", "E...GE", true)]
        public void WillPathTowardsClosestEnemy(string testCase, string expected, bool flip)
        {
            testCase = (flip) ? FlipXY(testCase) : testCase;
            expected = (flip) ? FlipXY(expected) : expected;

            var solution = TakeStepFrom(testCase);

            Assert.AreEqual(expected, solution.ToString());
        }

        [Test]
        public void WillNotWalkThroughWall()
        {
            var solution = TakeStepFrom("G#.e");

            Assert.AreEqual("G#.E", solution.ToString());
        }

        [Test]
        public void WillNotWalkThroughFriend()
        {
            var solution = TakeStepFrom("GG.e");

            Assert.AreEqual("G.GE", solution.ToString());
        }

        [Test]
        public void WillNotPathThroughWall()
        {
            var solution = TakeStepFrom("G.#.e");

            Assert.AreEqual("G.#.E", solution.ToString());
        }

        [Test]
        public void WillNotPathThroughFriend()
        {
            var solution = TakeStepFrom("G.G.e");

            Assert.AreEqual("G..GE", solution.ToString());
        }

        [Test]
        public void WillWalkUpBeforeLeft()
        {
            var testCase = Parse(@" e1
                                    1X");

            var expected = Parse(@" EX
                                    ..");

            var solution = TakeStepFrom(testCase);

            Assert.AreEqual(expected, solution.ToString());
        }

        [Test]
        public void WillWalkUpBeforeRight()
        {
            var testCase = Parse(@" 1e
                                    X1");

            var expected = Parse(@" XE
                                    ..");

            var solution = TakeStepFrom(testCase);

            Assert.AreEqual(expected, solution.ToString());
        }

        [TestCase(true)]
        [TestCase(false)]
        public void WillWalkUpBeforeDown(bool mirror)
        {
            var testCase = Parse(@" 123
                                    X#e
                                    123");

            var expected = Parse(@" X..
                                    .#E
                                    ...");

            if (mirror)
            {
                FlipLeftRight(testCase);
                FlipLeftRight(expected);
            }

            var solution = TakeStepFrom(testCase);

            Assert.AreEqual(expected, solution.ToString());
        }

        [TestCase(true)]
        [TestCase(false)]
        public void WillWalkToLeftBeforeRight(bool mirror)
        {
            var testCase = Parse(@" *X1
                                    *#2
                                    +e+ ");


            var expected = Parse(@" G..
                                    .#.
                                    .E. ");

            if (mirror)
            {
                FlipUpsideDown(testCase);
                FlipUpsideDown(expected);
            }

            var solution = TakeStepFrom(testCase);

            Assert.AreEqual(expected, solution.ToString());
        }

        [Test]
        public void WillWalkLeftBeforeDown()
        {
            var testCase = Parse(@" 1X
                                    e1");

            var expected = Parse(@" X.
                                    E. ");

            var solution = TakeStepFrom(testCase);

            Assert.AreEqual(expected, solution.ToString());
        }

        [Test]
        public void WillWalkRightBeforeDown()
        {
            var testCase = Parse(@" X1
                                    1e");

            var expected = Parse(@" .X
                                    .E ");

            var solution = TakeStepFrom(testCase);

            Assert.AreEqual(expected, solution.ToString());
        }

        [Test]
        public void WillPickTargetPositionOverOwnOrder()
        {
            var testCase = Parse(@" 1G**
                                    212e
                                    e2.. ");

            var expected = Parse(@" ..G. 
                                    ...E
                                    E... ");

            var solution = TakeStepFrom(testCase);

            Assert.AreEqual(expected, solution.ToString());
        }

        [Test]
        public void Wanhoop()
        {
            var testCase = Parse(@" e...e
                                    .....
                                    ..X..
                                    .....
                                    e...e");

            var expected = Parse(@" E...E
                                    ..G..
                                    .....
                                    .....
                                    E...E");

            var solution = TakeStepFrom(testCase);

            Assert.AreEqual(expected, solution.ToString());
        }

        [Test]
        public void BigExampleFromReddit()
        {
            var testCase = Parse(@" .1X**.#.##.#
                                    #212*GGE...#
                                    #323*56.#..#
                                    #434*+.....#
                                    #545+E.....#
                                    #+5+EGE.#..#
                                    .E+........#");

            // + is de "Chosen" target, snelste route met tiebreak op reading order gaat rechts

            Assert.AreEqual('G', testCase.Split('\n')[0][2]);

            var solution = TakeStepFrom(Parse(testCase));

            var gobboNu = solution.ToString().Split('\n')[0][3];
            Assert.AreEqual('G', gobboNu);
        }

        [Test]
        public void AnotherExampleFromReddit()
        {
            var testCase = Parse(@" .e.#...e.
                                    ##..G#### ");

            var expected = Parse(@" .E.#...E.
                                    ##.G.####");

            var solution = TakeStepFrom(testCase);

            Assert.AreEqual(expected, solution.ToString());
        }
        
        [Test]
        public void MoveExample()
        {
            var testCase = Parse(@" #########
                                    #G..G..G#
                                    #.......#
                                    #.......#
                                    #G..E..G#
                                    #.......#
                                    #.......#
                                    #G..G..G#
                                    #########");

            var solution = new Solution(testCase, Solution.InputMode.String);
            solution.Step();

            var s1 = Parse(@"   #########
                                #.G...G.#
                                #...G...#
                                #...E..G#
                                #.G.....#
                                #.......#
                                #G..G..G#
                                #.......#
                                #########");

            Assert.AreEqual(s1, solution.ToString());

            solution.Step();

            var s2 = Parse(@"   #########
                                #..G.G..#
                                #...G...#
                                #.G.E.G.#
                                #.......#
                                #G..G..G#
                                #.......#
                                #.......#
                                #########");

            Assert.AreEqual(s2, solution.ToString());

            solution.Step();

            var s3 = Parse(@"   #########
                                #.......#
                                #..GGG..#
                                #..GEG..#
                                #G..G...#
                                #......G#
                                #.......#
                                #.......#
                                #########");

            Assert.AreEqual(s3, solution.ToString());
        }

        [Test]
        public void SomeOtherGuysInput()
        {
            var input = Parse(@"################################
                                #####################...########
                                ###################....G########
                                ###################....#########
                                #######.##########......########
                                #######G#########........#######
                                #######G#######.G.........######
                                #######.######..G.........######
                                #######.......##.G...G.G..######
                                ########..##..#....G......G#####
                                ############...#.....G.....#####
                                #...#######..........G.#...#####
                                #...#######...#####G......######
                                ##...######..#######G.....#.##.#
                                ###.G.#####.#########G.........#
                                ###G..#####.#########.......#.E#
                                ###..######.#########..........#
                                ###.......#.#########.....E..E.#
                                #####G...#..#########.......#..#
                                ####.G.#.#...#######.....G.....#
                                ########......#####...........##
                                ###########..................###
                                ##########.................#####
                                ##########.................#####
                                ############..E.........E.....##
                                ############.........E........##
                                ###############.#............E##
                                ##################...E..E..##.##
                                ####################.#E..####.##
                                ################.....######...##
                                #################.#..###########
                                ################################");

            var solution = new Solution(input, Solution.InputMode.String);
            var result = solution.Run();

            Assert.AreEqual(201638, result.turn * result.hps);
        }

        [Test]
        public void PrintDoetNietIetsAndersDanDeTests()
        {
            var input = Parse(@"################################
                                #G..#####G.#####################
                                ##.#####...#####################
                                ##.#######..####################
                                #...#####.#.#.G...#.##...###...#
                                ##.######....#...G..#...####..##
                                ##....#....G.........E..####.###
                                #####..#...G........G...##....##
                                ######.....G............#.....##
                                ######....G.............#....###
                                #####..##.......E..##.#......###
                                ########.##...........##.....###
                                ####G.G.......#####..E###...####
                                ##.......G...#######..#####..###
                                #........#..#########.###...####
                                #.G..GG.###.#########.##...#####
                                #...........#########......#####
                                ##..........#########..#.#######
                                ###G.G......#########....#######
                                ##...#.......#######.G...#######
                                ##.......G....#####.E...#.######
                                ###......E..G.E......E.....#####
                                ##.#................E.#...######
                                #....#...................#######
                                #....#E........E.##.#....#######
                                #......###.#..#..##.#....#..####
                                #...########..#..####....#..####
                                #...########.#########......####
                                #...########.###################
                                ############.###################
                                #########....###################
                                ################################");

            var solution = new Solution(input, Solution.InputMode.String);
            var result = solution.Run();

            Assert.AreEqual(203426, result.turn * result.hps);
        }
    }
}
