using System.Collections.Generic;
using System.Linq;
using Advent2023.Shared;

namespace Advent2023.Advent03;

public class Solution : ISolution
{
    public List<Number> AllNumbers = new();
    public List<Symbol> AllSymbols = new();

    public Dictionary<Coordinate, Number> AllNumberLocations = new();

    public Solution(string input)
    {
        var grid = Input.GetLetterGrid(input);

        Number parsingNumber = null;
        for (int line = 0; line < grid.Length; line++)
        {
            for (int column = 0; column < grid[line].Length; column++)
            {
                var value = grid[line][column];
                var coord = new Coordinate(column, line);

                switch (grid[line][column])
                {
                    case >= '0' and <= '9':
                        if (parsingNumber == null)
                        {
                            parsingNumber = new Number();
                            AllNumbers.Add(parsingNumber);
                        }
                        parsingNumber.AddDigit(coord, value - '0');
                        AllNumberLocations.Add(coord, parsingNumber);
                        break;
                    case '.':
                        parsingNumber = null;
                        break;
                    default:
                        parsingNumber = null;
                        var symbol = new Symbol(coord, value);
                        AllSymbols.Add(symbol);
                        break;
                }
            }
        }

        foreach (var symbol in AllSymbols)
        {
            var neighbouringCoords = symbol.Coordinate.GetNeighbours();

            foreach (var neighbour in neighbouringCoords)
            {
                if (AllNumberLocations.TryGetValue(neighbour, out var number))
                    symbol.AddNumber(number);
            }
        }
    }
    public Solution() : this("Input.txt") { }

    public class Number
    {
        public List<Coordinate> Coordinates { get; set; } = new();

        public List<Symbol> LinkedSymbols { get; set; } = new();

        public long Value { get; set; }

        public void AddDigit(Coordinate coordinate, int value)
        {
            Coordinates.Add(coordinate);
            Value *= 10;
            Value += value;
        }

        public void AddSymbol(Symbol symbol)
        {
            if (!LinkedSymbols.Contains(symbol))
            {
                LinkedSymbols.Add(symbol);
                symbol.AddNumber(this);
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class Symbol
    {
        public Coordinate Coordinate { get; set; }

        public char Value { get; set; }

        public List<Number> LinkedNumbers { get; set; } = new();

        public Symbol(Coordinate coordinate, char value)
        {
            Coordinate = coordinate;
            Value = value;
        }

        public void AddNumber(Number number)
        {
            if (!LinkedNumbers.Contains(number))
            {
                LinkedNumbers.Add(number);
                number.AddSymbol(this);
            }
        }
    }

    public object GetResult1()
    {
        return AllNumbers.Where(n => n.LinkedSymbols.Count > 0).Sum(n => n.Value);
    }

    public object GetResult2()
    {
        return AllSymbols
            .Where(s => s.Value == '*' && s.LinkedNumbers.Count == 2)
            .Select(s => s.LinkedNumbers.Select(n => n.Value).Aggregate((a, b) => a * b))
            .Sum();
    }
}
