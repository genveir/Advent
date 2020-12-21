using Advent2020.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2020.Advent20
{
    class Tests
    {
        [TestCase(sample, 20899048083289)]
        public void Test1(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase(sample, 273)]
        public void Test2(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult2());
        }

        [Test]
        public void CanGetEdges()
        {
            var pieceStr = @"Tile 123:
...#
#..#
...#
#...";

            var piece = PuzzlePiece.Parse(pieceStr.Split(Environment.NewLine), true);
            var edges = piece.GetEdges();
            
            Assert.AreEqual(123, piece.Id);
            Assert.AreEqual(1, edges[PuzzlePiece.UP]);
            Assert.AreEqual(14, edges[PuzzlePiece.RIGHT]);
            Assert.AreEqual(8, edges[PuzzlePiece.DOWN]);
            Assert.AreEqual(5, edges[PuzzlePiece.LEFT]);
        }

        [TestCase(0, 1, 14, 8, 5)]
        [TestCase(1, 10, 1, 7, 8)]
        [TestCase(2, 1, 10, 8, 7)]
        [TestCase(3, 14, 1, 5, 8)]
        [TestCase(4, 8, 5, 1, 14)]
        [TestCase(5, 7, 8, 10, 1)]
        [TestCase(6, 8, 7, 1, 10)]
        [TestCase(7, 5, 8, 14, 1)]
        [TestCase(8, 1, 14, 8, 5)] // modular
        public void CanRotatePiece(int rotation, int up, int right, int down, int left)
        {
            var pieceStr = @"Tile 123:
...#
#..#
...#
#...";

/* rot1:    rot2:   rot3:   flip:   rot5:   rot6:   rot7:
#.#.        ...#    ###.    #...    .###    #...    .#.#
....        #...    ....    #..#    ....    ...#    ....
....        #..#    ....    #...    ....    #..#    ....
.###        #...    .#.#    ...#    #.#.    ...#    ###.
*/
            var piece = PuzzlePiece.Parse(pieceStr.Split(Environment.NewLine), true);
            piece.Rotation = rotation;
            var edges = piece.GetEdges();

            Assert.AreEqual(up, edges[PuzzlePiece.UP]);
            Assert.AreEqual(right, edges[PuzzlePiece.RIGHT]);
            Assert.AreEqual(down, edges[PuzzlePiece.DOWN]);
            Assert.AreEqual(left, edges[PuzzlePiece.LEFT]);
        }

/*
.......#
.##.....
.##.....
#####...
#####...
##.##.#.
#..##...
#####...

 */

        public const string smallSample = @"Tile 1:
####
##.#
#..#
####

Tile 2:
....
.##.
.##.
####

Tile 3:
#...
#.#.
#...
#...

Tile 4:
...#
....
....
#...";

        public const string expected = @"Tile 24:
##..
##..
#..#
....";

        public const string expected1 = @"Tile 24:
.###
..##
....
.#..";

        [TestCase(sample, s2)]
        [TestCase(smallSample, expected)]
        [TestCase(smallSample, expected1)]
        public void FuseIsCorrect(string input, string output)
        {
            var blocks = input.Split(Environment.NewLine + Environment.NewLine);

            var pieces = blocks.Select(block => PuzzlePiece.Parse(block.Split(Environment.NewLine), true)).ToArray();

            var fullPuzzle = new Puzzle().Lay(pieces);

            var fused = Puzzle.Fuse(fullPuzzle);

            var expected = output.Split(Environment.NewLine).Skip(1).ToArray();

            bool match = false;
            for (int rot = 0; rot < 8; rot++)
            {
                fused.Rotation = rot;
                var data = fused.GetStringData();
                Assert.AreEqual(expected.Length, data.Length);

                match = true;
                for (int n = 0; n < expected.Length; n++)
                {
                    if (expected[n] != data[n])
                    {
                        match = false;
                    }
                }
                if (match) break;
            }

            Assert.That(match);
        }

        [TestCase(singleMonster, 50)]
        [TestCase(s2, 273)]
        [TestCase(sample2, 273)]
        public void CanParseImages(string stringData, int roughness)
        {
            var lines = stringData.Split(Environment.NewLine);

            var piece = PuzzlePiece.Parse(lines, false);

            var solve = new ImageParser().Solve(piece);

            Assert.AreEqual(roughness, solve);
        }

        public const string singleMonster = @"Tile 123:
##### ##### ##### #####
      ##### ##### # 
#    ##    ##    ###
 #  #  #  #  #  #   
##### ##### ##### #####";

        public const string s2 = @"Tile 1235:
.#.#..#.##...#.##..#####
###....#.#....#..#......
##.##.###.#.#..######...
###.#####...#.#####.#..#
##.#....#.##.####...#.##
...########.#....#####.#
....#..#...##..#.#.###..
.####...#..#.....#......
#..#.##..#..###.#.##....
#.####..#.####.#.#.###..
###.#.#...#.######.#..##
#.####....##..########.#
##..##.#...#...#.#.#.#..
...#..#..#.#.##..###.###
.#.#....#.##.#...###.##.
###.#...#..#.##.######..
.#.#.###.##.##.#..#.##..
.####.###.#...###.#..#.#
..#.#..#..#.#.#.####.###
#..####...#.#.#.###.###.
#####..#####...###....##
#.##..#..#...#..####...#
.#.###..##..##..####.##.
...###...##...#...#..###";

        public const string sample2 = @"Tile 1248:
.####...#####..#...###..
#####..#..#.#.####..#.#.
.#.#...#.###...#.##.O#..
#.O.##.OO#.#.OO.##.OOO##
..#O.#O#.O##O..O.#O##.##
...#.#..##.##...#..#..##
#.##.#..#.#..#..##.#.#..
.###.##.....#...###.#...
#.####.#.#....##.#..#.#.
##...#..#....#..#...####
..#.##...###..#.#####..#
....#.##.#.#####....#...
..##.##.###.....#.##..#.
#...#...###..####....##.
.#.##...#.##.#.#.###...#
#.###.#..####...##..#...
#.###...#.##...#.##O###.
.O##.#OO.###OO##..OOO##.
..O#.O..O..O.#O##O##.###
#.#..##.########..#..##.
#.#####..#.#...##..#....
#....##..#.#########..##
#...#.....#..##...###.##
#..###....##.#...##.##.#";

        public const string sample = @"Tile 2311:
..##.#..#.
##..#.....
#...##..#.
####.#...#
##.##.###.
##...#.###
.#.#.#..##
..#....#..
###...#.#.
..###..###

Tile 1951:
#.##...##.
#.####...#
.....#..##
#...######
.##.#....#
.###.#####
###.##.##.
.###....#.
..#.#..#.#
#...##.#..

Tile 1171:
####...##.
#..##.#..#
##.#..#.#.
.###.####.
..###.####
.##....##.
.#...####.
#.##.####.
####..#...
.....##...

Tile 1427:
###.##.#..
.#..#.##..
.#.##.#..#
#.#.#.##.#
....#...##
...##..##.
...#.#####
.#.####.#.
..#..###.#
..##.#..#.

Tile 1489:
##.#.#....
..##...#..
.##..##...
..#...#...
#####...#.
#..#.#.#.#
...#.#.#..
##.#...##.
..##.##.##
###.##.#..

Tile 2473:
#....####.
#..#.##...
#.##..#...
######.#.#
.#...#.#.#
.#########
.###.#..#.
########.#
##...##.#.
..###.#.#.

Tile 2971:
..#.#....#
#...###...
#.#.###...
##.##..#..
.#####..##
.#..####.#
#..#.#..#.
..####.###
..#.#.###.
...#.#.#.#

Tile 2729:
...#.#.#.#
####.#....
..#.#.....
....#..#.#
.##..##.#.
.#.####...
####.#.#..
##.####...
##..#.##..
#.##...##.

Tile 3079:
#.#.#####.
.#..######
..#.......
######....
####.#..#.
.#...#.##.
#.#####.##
..#.###...
..#.......
..#.###...";
    }
}