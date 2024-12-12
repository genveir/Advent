namespace Advent2024.Day12;

public class Solution
{
    public char[][] grid;

    public List<Region> regions;
    public Dictionary<Coordinate2D, Region> regionsByCoordinate;

    public Solution(string input)
    {
        grid = Input.GetLetterGrid(input);

        Reset();
    }

    public void Reset()
    {
        HashSet<Coordinate2D> visited = [];
        regionsByCoordinate = [];

        regions = [];

        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[y].Length; x++)
            {
                if (visited.Contains(new Coordinate2D(x, y)))
                {
                    continue;
                }

                CreateRegion(x, y, visited);
            }
        }
    }

    public void CreateRegion(int x, int y, HashSet<Coordinate2D> visited)
    {
        char letter = grid[y][x];
        Region region = new()
        {
            Letter = letter,
            Coordinates = []
        };

        Queue<Coordinate2D> queue = [];
        queue.Enqueue(new Coordinate2D(x, y));

        while (queue.Count > 0)
        {
            Coordinate2D current = queue.Dequeue();
            if (visited.Contains(current))
            {
                continue;
            }
            visited.Add(current);
            region.Coordinates.Add(current);
            regionsByCoordinate[current] = region;

            foreach (var neighbour in current.GetNeighbours(true))
            {
                if (IsInBounds(grid, neighbour))
                {
                    if (grid[neighbour.Y][neighbour.X] == letter)
                    {
                        queue.Enqueue(neighbour);
                    }
                }
            }
        }
        regions.Add(region);
    }

    public static bool IsInBounds(char[][] grid, Coordinate2D coordinate)
    {
        return coordinate.X >= 0 && coordinate.X < grid[0].Length && coordinate.Y >= 0 && coordinate.Y < grid.Length;
    }

    public Solution() : this("Input.txt")
    {
    }

    public class Region
    {
        public char Letter { get; set; }
        public HashSet<Coordinate2D> Coordinates { get; set; }

        public Dictionary<Coordinate2D, List<int>> NeighbourSides { get; set; }

        public long Area()
        {
            return Coordinates.Count;
        }

        public long Perimeter(Dictionary<Coordinate2D, Region> regionsByCoordinate)
        {
            long sum = 0;

            if (NeighbourSides == null || NeighbourSides.Count > 0)
            {
                NeighbourSides = [];
            }

            foreach (var coordinate in Coordinates)
            {
                var neighbours = coordinate.GetNeighbours(true);

                for (int direction = 0; direction < 4; direction++)
                {
                    var neighbour = direction switch
                    {
                        0 => coordinate.ShiftY(-1),
                        1 => coordinate.ShiftX(1),
                        2 => coordinate.ShiftY(1),
                        3 => coordinate.ShiftX(-1),
                        _ => throw new Exception("Invalid direction")
                    };

                    if (!Coordinates.Contains(neighbour))
                    {
                        sum++;
                    }

                    if (regionsByCoordinate == null)
                    {

                    }
                    else if (regionsByCoordinate.TryGetValue(neighbour, out var region))
                    {
                        if (region != this)
                        {
                            if (!NeighbourSides.TryGetValue(coordinate, out var sides))
                            {
                                sides = [];
                                NeighbourSides[coordinate] = sides;
                            }
                            sides.Add(direction);
                        }
                    }
                }
            }

            return sum;
        }

        public long ImportSides = 0;
        public long OwnSides;

        public void CalculateSides()
        {
            if (Area() == 1)
            {
                OwnSides = 4;
            }

            var topLeftest = Coordinates.OrderBy(c => c.X).ThenBy(c => c.Y).First();

            long sides = 0;
            // outside perimeter
            sides += WalkOutside(topLeftest, 0);

            while (NeighbourSides.Count > 0)
            {
                var kvp = NeighbourSides.First();

                sides += WalkInside(kvp.Key, kvp.Value.First());
            }

            OwnSides = sides;
        }

        public long WalkOutside(Coordinate2D start, int sideFacing)
        {
            long sides = 0;
            var current = start;

            do
            {
                if (!Coordinates.Contains(current))
                {
                    throw new InvalidOperationException("Buiten de region");
                }

                if (NeighbourSides.TryGetValue(current, out var directions))
                {
                    directions.Remove(sideFacing);
                    if (directions.Count == 0)
                    {
                        NeighbourSides.Remove(current);
                    }
                }

                var outward = sideFacing switch
                {
                    0 => current.ShiftY(-1),
                    1 => current.ShiftX(1),
                    2 => current.ShiftY(1),
                    3 => current.ShiftX(-1),
                    _ => throw new Exception("Invalid side")
                };

                var inward = sideFacing switch
                {
                    0 => current.ShiftY(1),
                    1 => current.ShiftX(-1),
                    2 => current.ShiftY(-1),
                    3 => current.ShiftX(1),
                    _ => throw new Exception("Invalid side")
                };

                var forward = sideFacing switch
                {
                    0 => current.ShiftX(1),
                    1 => current.ShiftY(1),
                    2 => current.ShiftX(-1),
                    3 => current.ShiftY(-1),
                    _ => throw new Exception("Invalid side")
                };

                if (Coordinates.Contains(outward))
                {
                    sides++;
                    current = outward;
                    sideFacing = (sideFacing + 3) % 4;
                }
                else if (Coordinates.Contains(inward) && !Coordinates.Contains(forward))
                {
                    sides++;
                    sideFacing = (sideFacing + 1) % 4;
                }
                else if (!Coordinates.Contains(forward))
                {
                    sides++;
                    sideFacing = (sideFacing + 1) % 4;
                }
                else
                {
                    current = forward;
                }
            }
            while (!(current == start && sideFacing == 0));

            return sides;
        }

        public long WalkInside(Coordinate2D start, int startFacing)
        {
            long sides = 0;
            int sideFacing = startFacing;
            var current = start;

            do
            {
                if (!Coordinates.Contains(current))
                {
                    throw new InvalidOperationException("Buiten de region");
                }

                if (NeighbourSides.TryGetValue(current, out var directions))
                {
                    directions.Remove(sideFacing);
                    if (directions.Count == 0)
                    {
                        NeighbourSides.Remove(current);
                    }
                }

                var inward = sideFacing switch
                {
                    0 => current.ShiftY(-1),
                    1 => current.ShiftX(1),
                    2 => current.ShiftY(1),
                    3 => current.ShiftX(-1),
                    _ => throw new Exception("Invalid side")
                };

                var outward = sideFacing switch
                {
                    0 => current.ShiftY(1),
                    1 => current.ShiftX(-1),
                    2 => current.ShiftY(-1),
                    3 => current.ShiftX(1),
                    _ => throw new Exception("Invalid side")
                };

                var forward = sideFacing switch
                {
                    0 => current.ShiftX(-1),
                    1 => current.ShiftY(-1),
                    2 => current.ShiftX(1),
                    3 => current.ShiftY(1),
                    _ => throw new Exception("Invalid side")
                };

                if (Coordinates.Contains(inward))
                {
                    sides++;
                    sideFacing = (sideFacing + 1) % 4;
                    current = inward;
                }
                else if (Coordinates.Contains(outward) && !Coordinates.Contains(forward))
                {
                    sides++;
                    sideFacing = (sideFacing + 3) % 4;
                }
                else if (Coordinates.Contains(forward) && !Coordinates.Contains(inward))
                {
                    current = forward;
                }
                else
                {
                    sides++;
                    sideFacing = (sideFacing + 1) % 4;
                    current = inward;
                }
            }
            while (!(current == start && sideFacing == startFacing));

            return sides;
        }

        public long FencePrice => Perimeter(null) * Area();

        public long FencePrice2 => (ImportSides + OwnSides) * Area();
    }

    public object GetResult1()
    {
        return regions.Sum(r => r.FencePrice);
    }

    // not 886164
    // not 919224
    public object GetResult2()
    {
        foreach (var region in regions)
        {
            region.Perimeter(regionsByCoordinate);
        }
        foreach (var region in regions)
        {
            region.CalculateSides();
        }

        return regions.Sum(r => r.FencePrice2);
    }
}