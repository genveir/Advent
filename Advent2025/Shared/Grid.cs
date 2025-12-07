namespace Advent2025.Shared;
public class Grid<TCellType>
{
    public TCellType[][] Cells { get; set; }

    public TCellType[] this[long index] => Cells[index];

    public TCellType this[int x, int y]
    {
        get => Cells[y][x];
        set => Cells[y][x] = value;
    }

    public TCellType this[Coordinate2D coord]
    {
        get => Cells[coord.Y][coord.X];
        set => Cells[coord.Y][coord.X] = value;
    }

    public Grid(TCellType[][] cells)
    {
        Cells = cells;
    }

    public Coordinate2D[] Coordinates =>
        Enumerable.Range(0, Cells.Length)
            .SelectMany(y => Enumerable.Range(0, Cells[0].Length)
                .Select(x => new Coordinate2D(x, y)))
            .ToArray();

    public bool IsInBounds(int x, int y) => new Coordinate2D(x, y).IsInBounds(Cells);

    public bool IsInBounds(Coordinate2D coord) => coord.IsInBounds(Cells);

    public List<TCellType> GetNeighbours(bool orthogonalOnly, int x, int y) =>
        GetNeighbours(orthogonalOnly, new Coordinate2D(x, y));

    public List<TCellType> GetNeighbours(bool orthogonalOnly, Coordinate2D coord) =>
        coord.GetNeighbours(orthogonalOnly)
            .Where(c => c.IsInBounds(Cells))
            .Select(c => Cells[c.Y][c.X])
            .ToList();
}
