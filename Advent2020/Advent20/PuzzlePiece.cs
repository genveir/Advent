using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020.Advent20
{
    public class PuzzlePiece
    {
        public long Id { get; private set; }
        public Coordinate Coordinate { get; set; }

        private int _rotation;
        public int Rotation
        {
            get { return _rotation; }
            set 
            {
                _rotation = value % 8;
                _rotation += 8;
                _rotation = value % 8;
            }
        }

        private PieceData[] pieceData;

        public long[] GetEdges()
        {
            return pieceData[Rotation].Edges;
        }

        public string[] GetStrippedStringData()
        {
            return pieceData[Rotation].Stripped;
        }

        public string[] GetStringData()
        {
            return pieceData[Rotation].StringData;
        }

        public const int UP = 0;
        public const int RIGHT = 1;
        public const int DOWN = 2;
        public const int LEFT = 3;
        public const int FLIPPED = 4;

        private class PieceData
        {
            public string[] StringData;
            public string[] Stripped { get; }
            public long[] Edges { get; }

            public PieceData(string[] stringData, bool withEdges)
            {
                var topEdge = new string(stringData[0].Select(c => c == '.' ? '0' : '1').ToArray());
                var bottomEdge = new string(stringData[stringData.Length - 1].Select(c => c == '.' ? '0' : '1').ToArray());
                var leftEdge = new string(stringData.Select(l => l[0] == '.' ? '0' : '1').ToArray());
                var rightEdge = new string(stringData.Select(l => l[l.Length - 1] == '.' ? '0' : '1').ToArray());

                this.StringData = stringData;

                if (withEdges)
                {
                    var stripped = stringData.Skip(1);
                    stripped = stripped.Take(stripped.Count() - 1);
                    stripped = stripped.Select(s => s.Substring(1, s.Length - 2));

                    this.Stripped = stripped.ToArray();

                    this.Edges = new long[]
                    {
                        Convert.ToInt64(topEdge, 2),
                        Convert.ToInt64(rightEdge, 2),
                        Convert.ToInt64(bottomEdge, 2),
                        Convert.ToInt64(leftEdge, 2)
                    };
                }
            }

            public override string ToString()
            {
                return string.Join(Environment.NewLine, StringData);
            }
        }

        public static PuzzlePiece Parse(string[] lines, bool withEdges)
        {
            var id = long.Parse(lines[0].Split(new char[] { ' ', ':' })[1]);

            var data = lines.Skip(1).ToArray();
            
            var flipped = data.Select(l => l.StrReverse()).ToArray();
            var upsideDown = flipped.ArrReverse();
            var flippedUpsideDown = data.ArrReverse();

            string[] rotRight = new string[data.Length];
            string[] flippedRight = new string[data.Length];
            string[] rotLeft = new string[data.Length];
            string[] flippedLeft = new string[data.Length];
            for (int n = 0; n < data.Length; n++)
            {
                rotRight[n] = new string(data.Select(d => d[n]).ToArray()).StrReverse();
                flippedRight[n] = new string(flipped.Select(d => d[n]).ToArray()).StrReverse();
                rotLeft[data.Length - 1 - n] = new string(data.Select(d => d[n]).ToArray());
                flippedLeft[data.Length - 1 - n] = new string(flipped.Select(d => d[n]).ToArray());
            }

            return new PuzzlePiece()
            {
                Id = id,
                pieceData = new PieceData[]
                {
                    new PieceData(data, withEdges),
                    new PieceData(rotRight, withEdges),
                    new PieceData(upsideDown, withEdges),
                    new PieceData(rotLeft, withEdges),
                    new PieceData(flipped, withEdges),
                    new PieceData(flippedRight, withEdges),
                    new PieceData(flippedUpsideDown, withEdges),
                    new PieceData(flippedLeft, withEdges)
                }
            };
        }
    }
}
