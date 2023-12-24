using System.Collections.Generic;
using System.Linq;
using Advent2023.Shared;

namespace Advent2023.Advent16;

public class Solution : ISolution
{
    public char[][] grid;
    public Dictionary<long, List<Mirror>> byX = new();
    public Dictionary<long, List<Mirror>> byY = new();

    public long MaxX, MaxY;

    public Solution(string input)
    {
        grid = Input.GetLetterGrid(input);

        MaxX = grid[0].Length;
        MaxY = grid.Length;

        for (long y = 0; y < grid.Length; y++)
        {
            for (long x = 0; x < grid[y].Length; x++)
            {
                var coord = new Coordinate2D(x, y);

                Mirror mirror = grid[y][x] switch
                {
                    '|' => new UprightSplitter(coord),
                    '-' => new HorizontalSplitter(coord),
                    '\\' => new LeftBouncer(coord),
                    '/' => new RightBouncer(coord),
                    _ => null
                };

                if (mirror == null) continue;

                if (byX.ContainsKey(x))
                    byX[x].Add(mirror);
                else byX[x] = new() { mirror };

                if (byY.ContainsKey(y))
                    byY[y].Add(mirror);
                else byY[y] = new() { mirror };
            }
        }
    }
    public Solution() : this("Input.txt") { }

    public (List<VerticalLine>, List<HorizontalLine>) LinkMirrors()
    {
        var verticalLines = new List<VerticalLine>();
        foreach (var column in byX.Keys)
        {
            var topAccepter = new Mirror[] { new Accepter(new(column, 0)) };
            var bottomAccepter = new Mirror[] { new Accepter(new(column, MaxY - 1)) };

            var mirrors = topAccepter.Concat(byX[column].OrderBy(c => c.Position.Y)).Concat(bottomAccepter).ToArray();

            for (int n = 0; n < mirrors.Length - 1; n++)
            {
                // links the line up
                verticalLines.Add(new VerticalLine(mirrors[n], mirrors[n + 1]));
            }
        }

        var horizontalLines = new List<HorizontalLine>();
        foreach (var row in byY.Keys)
        {
            var leftAccepter = new Mirror[] { new Accepter(new(0, row)) };
            var rightAccepter = new Mirror[] { new Accepter(new(MaxX - 1, row)) };

            var mirrors = leftAccepter.Concat(byY[row].OrderBy(c => c.Position.X)).Concat(rightAccepter).ToArray();

            for (int n = 0; n < mirrors.Length - 1; n++)
            {
                // links the line up
                horizontalLines.Add(new HorizontalLine(mirrors[n], mirrors[n + 1]));
            }
        }

        return (verticalLines, horizontalLines);
    }

    public long CalculateEnergizedSquares(List<VerticalLine> verticalLines, List<HorizontalLine> horizontalLines)
    {
        var energizedVertical = verticalLines.Where(v => v.IsEnergizedTop || v.IsEnergizedBottom).ToList();
        var energizedHorizontal = horizontalLines.Where(v => v.IsEnergizedRight || v.IsEnergizedLeft).ToList();

        var combinedVertical = CombineVerticalStretches(energizedVertical);
        var combinedHorizontal = CombineHorizontalStretches(energizedHorizontal);

        long numOverlaps = FindOverlaps(combinedVertical, combinedHorizontal);

        return
            combinedHorizontal.Sum(eh => eh.MaxX - eh.MinX + 1) +
            combinedVertical.Sum(ev => ev.MaxY - ev.MinY + 1) -
            numOverlaps;
    }

    public long FindOverlaps(List<VerticalLine> verticalLines, List<HorizontalLine> horizontalLines)
    {
        long numOverlaps = 0;
        foreach (var verticalLine in verticalLines)
        {
            var x = verticalLine.X;
            var minY = verticalLine.MinY;
            var maxY = verticalLine.MaxY;

            numOverlaps += horizontalLines
                .Where(hl => hl.MinX <= x && hl.MaxX >= x)
                .Where(hl => hl.Y >= minY && hl.Y <= maxY)
                .Count();
        }

        return numOverlaps;
    }

    public List<VerticalLine> CombineVerticalStretches(List<VerticalLine> verticalLines)
    {
        var copy = new List<VerticalLine>(verticalLines);

        bool didReplace;
        do
        {
            didReplace = false;
            for (int n = 0; n < copy.Count; n++)
            {
                didReplace = CombineStretch(copy[n], copy);
                if (didReplace) break;
            }
        } while (didReplace);

        return copy;
    }

    public bool CombineStretch(VerticalLine verticalLine, List<VerticalLine> verticalLines)
    {
        var sameX = verticalLines.Where(vl => vl.X == verticalLine.X).ToArray();

        var matchY = verticalLine.MinY;
        var newTop = verticalLine.Top;

        List<VerticalLine> toCombine = new();
        VerticalLine topMatch = verticalLine;
        while (topMatch != null)
        {
            toCombine.Add(topMatch);

            topMatch = sameX.Where(vl => vl.MaxY == verticalLine.MinY && !toCombine.Any(tc => tc.Index == vl.Index)).SingleOrDefault();
            if (topMatch != null)
            {
                matchY = topMatch.MinY;
                newTop = topMatch.Top;
            }
        }

        if (toCombine.Count > 1)
        {
            foreach (var vl in toCombine)
            {
                verticalLines.Remove(vl);
            }

            verticalLines.Add(new VerticalLine(newTop, verticalLine.Bottom));
            return true;
        }
        return false;
    }

    public List<HorizontalLine> CombineHorizontalStretches(List<HorizontalLine> horizontalLines)
    {
        var copy = new List<HorizontalLine>(horizontalLines);

        bool didReplace;
        do
        {
            didReplace = false;
            for (int n = 0; n < copy.Count; n++)
            {
                didReplace = CombineStretch(copy[n], copy);
                if (didReplace) break;
            }
        } while (didReplace);

        return copy;
    }

    public bool CombineStretch(HorizontalLine horizontalLine, List<HorizontalLine> horizontalLines)
    {
        var sameY = horizontalLines.Where(hl => hl.Y == horizontalLine.Y).ToArray();

        var matchX = horizontalLine.MinX;
        var newLeft = horizontalLine.Left;

        List<HorizontalLine> toCombine = new();
        HorizontalLine leftMatch = horizontalLine;
        while (leftMatch != null)
        {
            toCombine.Add(leftMatch);

            leftMatch = sameY.Where(hl => hl.MaxX == horizontalLine.MinX && !toCombine.Any(tc => tc.Index == hl.Index)).SingleOrDefault();
            if (leftMatch != null)
            {
                matchX = leftMatch.MinX;
                newLeft = leftMatch.Left;
            }
        }

        if (toCombine.Count > 1)
        {
            foreach (var vl in toCombine)
            {
                horizontalLines.Remove(vl);
            }

            horizontalLines.Add(new HorizontalLine(newLeft, horizontalLine.Right));
            return true;
        }
        return false;
    }

    // 7758 too low
    public object GetResult1()
    {
        var (verticalLines, horizontalLines) = LinkMirrors();

        var firstMirrorX = byY[0].Min(m => m.Position.X);
        var firstMirror = byY[0].Single(m => m.Position.X == firstMirrorX);

        firstMirror.Left.EnergizeFromLeft();

        return CalculateEnergizedSquares(verticalLines, horizontalLines);
    }

    // 7973 too low
    public object GetResult2()
    {
        var (baseVert, baseHor) = LinkMirrors();
        var fromLeft = baseHor.Where(hl => hl.Left is Accepter).Select(hl => hl.Right).ToArray();
        var fromRight = baseHor.Where(hl => hl.Right is Accepter).Select(hl => hl.Left).ToArray();
        var fromTop = baseVert.Where(vl => vl.Top is Accepter).Select(vl => vl.Bottom).ToArray();
        var fromBottom = baseVert.Where(vl => vl.Bottom is Accepter).Select(vl => vl.Top).ToArray();

        long max = 0;

        foreach (var hl in fromLeft)
        {
            var (verticalLines, horizontalLines) = LinkMirrors();

            hl.Left.EnergizeFromLeft();

            var value = CalculateEnergizedSquares(verticalLines, horizontalLines);
            if (value > max) max = value;
        }

        foreach (var hl in fromRight)
        {
            var (verticalLines, horizontalLines) = LinkMirrors();

            hl.Right.EnergizeFromRight();

            var value = CalculateEnergizedSquares(verticalLines, horizontalLines);
            if (value > max) max = value;
        }

        foreach (var vl in fromTop)
        {
            var (verticalLines, horizontalLines) = LinkMirrors();

            vl.Up.EnergizeFromTop();

            var value = CalculateEnergizedSquares(verticalLines, horizontalLines);
            if (value > max) max = value;
        }

        foreach (var vl in fromBottom)
        {
            var (verticalLines, horizontalLines) = LinkMirrors();

            vl.Down.EnergizeFromBottom();

            var value = CalculateEnergizedSquares(verticalLines, horizontalLines);
            if (value > max) max = value;
        }

        return max;
    }
}
