using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2020.Advent20
{
    public class Solution : ISolution
    {
        public PuzzlePiece[][] puzzle;
        public PuzzlePiece fusedPuzzle;

        public Solution(string input)
        {
            var blocks = Input.GetBlockLines(input);

            var pieces = blocks.Select(block => PuzzlePiece.Parse(block, true)).ToArray();

            this.puzzle = new Puzzle().Lay(pieces);
            this.fusedPuzzle = Puzzle.Fuse(puzzle);
        }
        public Solution() : this("Input.txt") { }

        public object GetResult1()
        {
            return fusedPuzzle.Id;
        }

        public object GetResult2()
        {
            return new ImageParser().Solve(fusedPuzzle);
        }
    }
}
