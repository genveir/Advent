using System;
using System.Collections.Generic;
using System.Linq;
using Advent2023.Shared;

namespace Advent2023.Advent15;

public class Solution : ISolution
{
    public ParsedInput[] modules;

    public Solution(string input)
    {
        var line = Input.GetInputLines(input).Single();

        var inputParser = new InputParser<ParsedInput[]>("line");

        modules = inputParser.Parse(line);
    }
    public Solution() : this("Input.txt") { }

    public class ParsedInput
    {
        public string Value { get; set; }

        public Instruction Instruction { get; set; }

        [ComplexParserConstructor("value")]
        public ParsedInput(string value)
        {
            Value = value;
            Instruction = Instruction.FromString(value);
        }

        public long CalculateHash(string val = null)
        {
            if (val == null) val = Value;

            var current = 0;
            for (int n = 0; n < val.Length; n++)
            {
                current += val[n];
                current *= 17;
                current = current % 256;
            }
            return current;
        }

        public void Apply(Box[] boxes) => Instruction.Apply(boxes[CalculateHash(Instruction.Label)]);
    }

    public abstract class Instruction
    {
        public string Label { get; set; }

        public static Instruction FromString(string value)
        {
            if (value.Contains('-'))
                return new Dash(value);
            else return new Equality(value);
        }

        public abstract void Apply(Box box);
    }

    public class Dash : Instruction
    {
        public Dash(string value)
        {
            var parts = value.Split('-');
            Label = parts[0];
        }

        public override void Apply(Box box)
        {
            box.RemoveLens(Label);
        }
    }

    public class Equality : Instruction
    {
        public long FocalLength { get; set; }

        public Equality(string value)
        {
            var parts = value.Split('=');
            Label = parts[0];
            FocalLength = long.Parse(parts[1]);
        }

        public override void Apply(Box box)
        {
            box.AddOrUpdateLens(Label, FocalLength);
        }
    }

    public class Box
    {
        public int Index { get; set; }

        public List<Lens> Lenses { get; set; } = new();

        public Box(int index)
        {
            Index = index;
        }

        public void RemoveLens(string label)
        {
            var relevantLens = Lenses.SingleOrDefault(l => l.Label == label);
            if (relevantLens != null)
                Lenses.Remove(relevantLens);
        }

        public void AddOrUpdateLens(string label, long focalLength)
        {
            var relevantLens = Lenses.SingleOrDefault(l => l.Label == label);
            if (relevantLens == null)
            {
                Lenses.Add(new Lens(label, focalLength));
            }
            else
            {
                relevantLens.FocalLength = focalLength;
            }
        }

        public long LensValue()
        {
            long sum = 0;
            var boxVal = Index + 1;
            for (int n = 0; n < Lenses.Count; n++)
            {
                var lens = Lenses[n];

                var slotVal = n + 1;
                var focalLength = lens.FocalLength;

                sum += boxVal * slotVal * focalLength;
            }

            return sum;
        }

        public override string ToString()
        {
            return $"Box {Index}: {string.Join(' ', Lenses)}";
        }
    }

    public class Lens
    {
        public string Label;

        public long FocalLength;

        public Lens(string label, long focalLength)
        {
            Label = label;
            FocalLength = focalLength;
        }

        public override string ToString()
        {
            return $"[{Label} {FocalLength}]";
        }
    }

    public object GetResult1()
    {
        return modules.Sum(m => m.CalculateHash());
    }

    // 4334581 too high
    public object GetResult2()
    {
        var boxes = new Box[256];
        for (int n = 0; n < boxes.Length; n++) boxes[n] = new Box(n);

        foreach (var module in modules)
            module.Apply(boxes);

        return boxes.Sum(b => b.LensValue());
    }
}
