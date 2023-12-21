using Advent2023.Shared;

namespace Advent2023.Advent16;
public abstract class Mirror
{
    public Coordinate Position { get; set; }

    public Mirror(Coordinate position) => Position = position;

    public HorizontalLine Left, Right;
    public VerticalLine Up, Down;

    public abstract void OnEnergizeLeft();
    public abstract void OnEnergizeRight();
    public abstract void OnEnergizeUp();
    public abstract void OnEnergizeDown();
}

public class UprightSplitter : Mirror
{
    public UprightSplitter(Coordinate position) : base(position) { }

    public override void OnEnergizeDown() => Up.EnergizeFromBottom();
    public override void OnEnergizeUp() => Down.EnergizeFromTop();
    public override void OnEnergizeLeft()
    {
        Up.EnergizeFromBottom();
        Down.EnergizeFromTop();
    }
    public override void OnEnergizeRight()
    {
        Up.EnergizeFromBottom();
        Down.EnergizeFromTop();
    }
}

public class HorizontalSplitter : Mirror
{
    public HorizontalSplitter(Coordinate position) : base(position) { }

    public override void OnEnergizeLeft() => Right.EnergizeFromLeft();
    public override void OnEnergizeRight() => Left.EnergizeFromRight();
    public override void OnEnergizeDown()
    {
        Left.EnergizeFromRight();
        Right.EnergizeFromLeft();
    }
    public override void OnEnergizeUp()
    {
        Left.EnergizeFromRight();
        Right.EnergizeFromLeft();
    }
}

public class LeftBouncer : Mirror
{
    public LeftBouncer(Coordinate position) : base(position) { }

    public override void OnEnergizeDown() => Left.EnergizeFromRight();
    public override void OnEnergizeLeft() => Down.EnergizeFromTop();
    public override void OnEnergizeRight() => Up.EnergizeFromBottom();
    public override void OnEnergizeUp() => Right.EnergizeFromLeft();
}

public class RightBouncer : Mirror
{
    public RightBouncer(Coordinate position) : base(position) { }

    public override void OnEnergizeDown() => Right.EnergizeFromLeft();
    public override void OnEnergizeRight() => Down.EnergizeFromTop();
    public override void OnEnergizeLeft() => Up.EnergizeFromBottom();
    public override void OnEnergizeUp() => Left.EnergizeFromRight();
}

public class Accepter : Mirror
{
    public Accepter(Coordinate position) : base(position) { }

    public override void OnEnergizeDown() { }
    public override void OnEnergizeRight() { }
    public override void OnEnergizeLeft() { }
    public override void OnEnergizeUp() { }
}