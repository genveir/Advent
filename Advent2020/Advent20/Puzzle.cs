using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020.Advent20
{
    public class Puzzle
    {
        private Dictionary<Coordinate, PuzzlePiece> placed = new Dictionary<Coordinate, PuzzlePiece>();
        private Queue<PuzzlePiece> workQueue = new Queue<PuzzlePiece>();

        public PuzzlePiece[][] Lay(PuzzlePiece[] pieces)
        {
            Place(pieces[0], new Coordinate(0, 0));

            while(workQueue.Count > 0)
            {
                var todo = workQueue.Dequeue();

                TryPlacePiece(todo.Coordinate.ShiftY(-1), pieces);
                TryPlacePiece(todo.Coordinate.ShiftX(1), pieces);
                TryPlacePiece(todo.Coordinate.ShiftY(1), pieces);
                TryPlacePiece(todo.Coordinate.ShiftX(-1), pieces);
            }

            for (int n = 0; n < pieces.Length; n++)
            {
                if (pieces[n].Coordinate == null) throw new NotImplementedException("not this easy"); // it was though
            }

            var minX = pieces.Select(p => p.Coordinate.X).Min();
            var minY = pieces.Select(p => p.Coordinate.Y).Min();
            var maxX = pieces.Select(p => p.Coordinate.X).Max();
            var maxY = pieces.Select(p => p.Coordinate.Y).Max();

            var xSize = maxX - minX + 1;
            var ySize = maxY - minY + 1;

            var result = new PuzzlePiece[ySize][];
            for (int y = 0; y < ySize; y++) result[y] = new PuzzlePiece[xSize];

            for (int n = 0; n < pieces.Length; n++)
            {
                var coord = pieces[n].Coordinate;

                result[coord.Y - minY][coord.X - minX] = pieces[n];
            }

            return result;
        }

        private void TryPlacePiece(Coordinate coord, PuzzlePiece[] pieces)
        {
            if (placed.ContainsKey(coord)) return;

            PuzzlePiece up, right, down, left;
            placed.TryGetValue(coord.ShiftY(-1), out up);
            placed.TryGetValue(coord.ShiftX(1), out right);
            placed.TryGetValue(coord.ShiftY(1), out down);
            placed.TryGetValue(coord.ShiftX(-1), out left);

            long? topEdge = null, rightEdge = null, bottomEdge = null, leftEdge = null;

            if (up != null) topEdge = up.GetEdges()[PuzzlePiece.DOWN];
            if (right != null) rightEdge = right.GetEdges()[PuzzlePiece.LEFT];
            if (down != null) bottomEdge = down.GetEdges()[PuzzlePiece.UP];
            if (left != null) leftEdge = left.GetEdges()[PuzzlePiece.RIGHT];

            for (int n = 0; n < pieces.Length; n++)
            {
                var piece = pieces[n];

                if (piece.Coordinate != null) continue;

                for (var rot = 0; rot < 8; rot++)
                {
                    piece.Rotation = rot;
                    var edges = piece.GetEdges();

                    bool fits = true;
                    if (topEdge != null && topEdge != edges[PuzzlePiece.UP]) fits = false;
                    if (rightEdge != null && rightEdge != edges[PuzzlePiece.RIGHT]) fits = false;
                    if (bottomEdge != null && bottomEdge != edges[PuzzlePiece.DOWN]) fits = false;
                    if (leftEdge != null && leftEdge != edges[PuzzlePiece.LEFT]) fits = false;

                    if (fits)
                    {
                        Place(piece, coord);
                        return;
                    }
                }
            }
        }

        public static PuzzlePiece Fuse(PuzzlePiece[][] laidPuzzle)
        {
            long id = new PuzzlePiece[]
            {
                laidPuzzle[0][0],
                laidPuzzle[0][laidPuzzle[0].Length - 1],
                laidPuzzle[laidPuzzle.Length - 1][0],
                laidPuzzle[laidPuzzle.Length - 1][laidPuzzle[0].Length - 1]
            }.Select(piece => piece.Id)
            .Aggregate((a, b) => a * b);

            List<string> stringData = new List<string>();

            for (int puzzleRow = 0; puzzleRow < laidPuzzle.Length; puzzleRow++)
            {
                string[] accumulators = new string[laidPuzzle[puzzleRow][0].GetStrippedStringData().Length];
                for (int puzzlePiece = 0; puzzlePiece < laidPuzzle[puzzleRow].Length; puzzlePiece++)
                {
                    var strippedData = laidPuzzle[puzzleRow][puzzlePiece].GetStrippedStringData();
                    for (int puzzleLine = 0; puzzleLine < strippedData.Length; puzzleLine++)
                    {
                        accumulators[puzzleLine] += strippedData[puzzleLine];
                    }
                }
                
                for (int n = 0; n < accumulators.Length; n++)
                {
                    stringData.Add(accumulators[n]);
                }   
            }

            var pieceString =
                new string[] { $"Tile {id}:" }
                .Concat(stringData)
                .ToArray();

            return PuzzlePiece.Parse(pieceString, false);
        }

        private void Place(PuzzlePiece piece, Coordinate coord)
        {
            piece.Coordinate = coord;
            workQueue.Enqueue(piece);
            placed[coord] = piece;
        }
    }
}
