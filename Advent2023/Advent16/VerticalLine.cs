namespace Advent2023.Advent16;
public class VerticalLine
{
    public static int _indexCounter = 0;
    public int Index;

    public VerticalLine(Mirror top, Mirror bottom)
    {
        Index = _indexCounter++;

        Top = top;
        Top.Down = this;

        Bottom = bottom;
        Bottom.Up = this;

        X = Top.Position.X;

        MinY = Top.Position.Y;
        MaxY = Bottom.Position.Y;
    }

    public long X { get; set; }

    public long MinY { get; set; }
    public long MaxY { get; set; }

    public Mirror Top { get; set; }
    public Mirror Bottom { get; set; }

    public bool IsEnergizedTop { get; set; } = false;
    public bool IsEnergizedBottom { get; set; } = false;

    public void EnergizeFromTop()
    {
        if (IsEnergizedTop) return;

        IsEnergizedTop = true;
        Bottom.OnEnergizeUp();
    }
    public void EnergizeFromBottom()
    {
        if (IsEnergizedBottom) return;

        IsEnergizedBottom = true;
        Top.OnEnergizeDown();
    }

    public override string ToString() => $"VL {Top.Position}-{Bottom.Position}{(IsEnergizedTop || IsEnergizedBottom ? "  E" : "")}";
}
