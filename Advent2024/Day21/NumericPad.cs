using System.Text;

namespace Advent2024.Day21;


public class NumericPad
{
    public Dictionary<char, Coordinate2D> buttons = new()
    {
        {'7', new(0,0) },
        {'8', new(1,0) },
        {'9', new(2,0) },
        {'4', new(0,1) },
        {'5', new(1,1) },
        {'6', new(2,1) },
        {'1', new(0,2) },
        {'2', new(1,2) },
        {'3', new(2,2) },
        {'0', new(1,3) },
        {'A', new(2,3) }
    };

    public Dictionary<(char, char), string> moves = [];

    public NumericPad()
    {
        for (int n = 0; n < buttons.Count; n++)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                var from = buttons.ElementAt(n).Key;
                var to = buttons.ElementAt(i).Key;

                var fromCoord = buttons[from];
                var toCoord = buttons[to];

                moves.Add((from, to), GetRoute(fromCoord, toCoord));
            }
        }
    }

    public string GetRoute(Coordinate2D from, Coordinate2D to)
    {
        if (from == to)
            return "A";

        if (from == new Coordinate2D(1, 3) && to.X < from.X)
        {
            return "^" + GetRoute(from.ShiftY(-1), to);
        }

        if (from == new Coordinate2D(0, 2) && to.Y > from.Y)
        {
            return ">" + GetRoute(from.ShiftX(1), to);
        }

        if (from.X > to.X)
        {
            return "<" + GetRoute(from.ShiftX(-1), to);
        }
        if (from.Y > to.Y)
        {
            return "^" + GetRoute(from.ShiftY(-1), to);
        }
        if (from.Y < to.Y)
        {
            return "v" + GetRoute(from.ShiftY(1), to);
        }
        if (from.X < to.X)
        {
            return ">" + GetRoute(from.ShiftX(1), to);
        }
        throw new NotSupportedException("X and Y are the same for from and to, but from and to are not the same");
    }

    public string GetShortestRouteForCode(string code)
    {
        code = 'A' + code;

        var oneShortestBuilder = new StringBuilder();
        for (int n = 0; n < code.Length - 1; n++)
        {
            var pair = code.Substring(n, 2);
            var routes = moves[(pair[0], pair[1])];
            oneShortestBuilder.Append(routes);
        }

        return oneShortestBuilder.ToString();
    }


}