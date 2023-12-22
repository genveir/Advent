using Advent2023.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2023.Advent19;
public class Part
{
    public long X { get; set; }
    public long M { get; set; }
    public long A { get; set; }
    public long S { get; set; }

    [ComplexParserConstructor("{values}")]
    public Part(Rating[] ratings)
    {
        X = ratings.Single(r => r.Name == "x").Value;
        M = ratings.Single(r => r.Name == "m").Value;
        A = ratings.Single(r => r.Name == "a").Value;
        S = ratings.Single(r => r.Name == "s").Value;

        Value = X + M + A + S;
    }

    public long Value { get; set; }
}
public class Rating
{
    public string Name { get; set; }
    public long Value { get; set; }

    [ComplexParserConstructor("name=value")]
    public Rating(string name, long value)
    {
        Name = name;
        Value = value;
    }
}