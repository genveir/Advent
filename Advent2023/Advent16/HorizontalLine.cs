using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Advent2023.Advent16.Solution;

namespace Advent2023.Advent16;
public class HorizontalLine
{
    public static int _indexCounter = 0;
    public int Index;

    public HorizontalLine(Mirror left, Mirror right)
    {
        Index = _indexCounter++;

        Left = left;
        Left.Right = this;

        Right = right;
        Right.Left = this;

        Y = Left.Position.Y;
        MinX = Left.Position.X;
        MaxX = Right.Position.X;
    }

    public long Y { get; set; }

    public long MinX { get; set; }
    public long MaxX { get; set; }

    public Mirror Left { get; set; }
    public Mirror Right { get; set; }

    public bool IsEnergizedLeft { get; set; } = false;
    public bool IsEnergizedRight { get; set; } = false;

    public void EnergizeFromLeft()
    {
        if (IsEnergizedLeft) return;

        IsEnergizedLeft = true;
        Right.OnEnergizeLeft();
    }
    public void EnergizeFromRight()
    {
        if (IsEnergizedRight) return;

        IsEnergizedRight = true;
        Left.OnEnergizeRight();
    }

    public override string ToString() => $"HL {Left.Position}-{Right.Position}{(IsEnergizedLeft || IsEnergizedRight ? "  E" : "")}";
}
