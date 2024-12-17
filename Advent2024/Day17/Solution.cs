using System.Text;

namespace Advent2024.Day17;

public class Solution
{
    public Computer Original { get; set; }

    public Solution(string input)
    {
        var lines = Input.GetInputLines(input).ToArray();

        var registerParser = new InputParser<char, long>("\\Register letter: value");
        var registers = registerParser.Parse(lines.Take(3));

        var programParser = new InputParser<long[]>("\\Program: values");
        var program = programParser.Parse(lines[4]).ToList();

        Original = new Computer(registers[0].Item2, registers[1].Item2, registers[2].Item2, program);
    }

    public Solution() : this("Input.txt")
    {
    }

    public class Computer
    {
        public long A { get; set; }
        public long B { get; set; }
        public long C { get; set; }

        public List<long> Program { get; set; }
        public int ProgramCounter { get; set; }

        public List<long> Output { get; set; } = [];

        public Computer(long a, long b, long c, List<long> program)
        {
            A = a;
            B = b;
            C = c;
            Program = program;
            ProgramCounter = 0;
        }

        public string PrintProgram()
        {
            var builder = new StringBuilder();
            for (int n = 0; n < Program.Count; n += 2)
            {
                var opCode = Program[n];
                var operand = Program[n + 1];
                var operation = Operation.Create(opCode, operand);

                builder.AppendLine(operation.ToString());
            }
            return builder.ToString();
        }

        public void Run()
        {
            while (ProgramCounter < Program.Count)
            {
                Step();
            }
        }

        public void RunToOutput()
        {
            var currentOutputs = Output.Count;

            while (ProgramCounter < Program.Count && Output.Count == currentOutputs)
            {
                Step();
            }
        }

        public void RunToError()
        {
            while (ProgramCounter < Program.Count)
            {
                Step();

                for (int n = 0; n < Output.Count; n++)
                {
                    if (Output[n] != Program[n])
                        return;
                }
            }
        }

        public void Step()
        {
            var opCode = Program[ProgramCounter];
            var operand = Program[ProgramCounter + 1];
            var operation = Operation.Create(opCode, operand);

            operation.Operate(this);

            ProgramCounter += 2;
        }

        public Computer Clone()
        {
            return new Computer(A, B, C, Program.ToList());
        }
    }

    public abstract class Operation
    {
        public long OpCode { get; set; }
        public long Operand { get; set; }

        public long ComboOperand(Computer computer)
        {
            return Operand switch
            {
                0 => 0,
                1 => 1,
                2 => 2,
                3 => 3,
                4 => computer.A,
                5 => computer.B,
                6 => computer.C,
                _ => throw new Exception("Invalid operand")
            };
        }

        public string ComboOperandString()
        {
            return (Operand) switch
            {
                0 => "0",
                1 => "1",
                2 => "2",
                3 => "3",
                4 => "A",
                5 => "B",
                6 => "C",
                _ => throw new Exception("Invalid operand")
            };
        }

        public static Operation Create(long opCode, long operand)
        {
            return opCode switch
            {
                0 => new Adv { OpCode = opCode, Operand = operand },
                1 => new Bxl { OpCode = opCode, Operand = operand },
                2 => new Bst { OpCode = opCode, Operand = operand },
                3 => new Jnz { OpCode = opCode, Operand = operand },
                4 => new Bxc { OpCode = opCode, Operand = operand },
                5 => new Out { OpCode = opCode, Operand = operand },
                6 => new Bdv { OpCode = opCode, Operand = operand },
                7 => new Cdv { OpCode = opCode, Operand = operand },
                _ => throw new Exception("Invalid opcode")
            };
        }

        public void Step(Computer computer)
        {
            Operate(computer);

            computer.ProgramCounter += 2;
        }

        public abstract void Operate(Computer computer);
    }

    public class Adv : Operation
    {
        public override void Operate(Computer computer)
        {
            var numerator = computer.A;
            var denominator = Math.Pow(2, ComboOperand(computer));

            computer.A = (long)Math.Floor(numerator / denominator);
        }

        public override string ToString()
        {
            return $"Divide A by 2^{ComboOperandString()}";
        }
    }

    public class Bxl : Operation
    {
        public override void Operate(Computer computer)
        {
            var value = computer.B ^ Operand;

            computer.B = value;
        }

        public override string ToString()
        {
            return $"XOR B with {Operand} and store the result in B";
        }
    }

    public class Bst : Operation
    {
        public override void Operate(Computer computer)
        {
            var value = ComboOperand(computer) % 8;

            computer.B = value;
        }

        public override string ToString()
        {
            return $"Store {ComboOperandString()} % 8 in B";
        }
    }

    public class Jnz : Operation
    {
        public override void Operate(Computer computer)
        {
            if (computer.A != 0)
            {
                computer.ProgramCounter = (int)Operand - 2;
            }
        }

        public override string ToString()
        {
            return $"Jump to {Operand} if A is not 0";
        }
    }

    public class Bxc : Operation
    {
        public override void Operate(Computer computer)
        {
            var value = computer.B ^ computer.C;

            computer.B = value;
        }

        public override string ToString()
        {
            return $"XOR B with C and store the result in B";
        }
    }

    public class Out : Operation
    {
        public override void Operate(Computer computer)
        {
            var value = ComboOperand(computer) % 8;

            computer.Output.Add(value);
        }

        public override string ToString()
        {
            return $"Output {ComboOperandString()} % 8";
        }
    }

    public class Bdv : Operation
    {
        public override void Operate(Computer computer)
        {
            var numerator = computer.A;
            var denominator = Math.Pow(2, ComboOperand(computer));

            computer.B = (long)Math.Floor(numerator / denominator);
        }

        public override string ToString()
        {
            return $"Divide A by 2^{ComboOperandString()} and store the result in B";
        }
    }

    public class Cdv : Operation
    {
        public override void Operate(Computer computer)
        {
            var numerator = computer.A;
            var denominator = Math.Pow(2, ComboOperand(computer));

            computer.C = (long)Math.Floor(numerator / denominator);
        }

        public override string ToString()
        {
            return $"Divide A by 2^{ComboOperandString()} and store the result in C";
        }
    }

    public object GetResult1()
    {
        var p1Comp = Original.Clone();

        p1Comp.Run();

        return string.Join(',', p1Comp.Output);
    }

    // 25373968303429 too low <-- missed one digit
    public object GetResult2()
    {
        return FindA(0, 1);
    }

    public long FindA(long a, int digits)
    {
        if (digits == 17)
            return a;

        var nextDigits = FindNextDigit(a, digits);

        foreach (var option in nextDigits)
        {
            var nextA = FindA(option, digits + 1);
            if (nextA != -1)
                return nextA;
        }

        return -1;
    }

    public List<long> FindNextDigit(long a, int expectedDigits)
    {
        List<long> values = [];
        for (int n = 0; n < 8; n++)
        {
            var p2Comp = Original.Clone();
            p2Comp.A = a * 8 + n;

            p2Comp.Run();

            if (p2Comp.Output.Count != expectedDigits)
                continue;

            bool isMatch = true;
            for (int i = 1; i <= p2Comp.Output.Count; i++)
            {
                if (p2Comp.Output[^i] != Original.Program[^i])
                {
                    isMatch = false;
                    break;
                }
            }

            if (isMatch)
            {
                values.Add(a * 8 + n);
            }
        }

        return values;
    }

    public List<long> GetFirstN(long loopStart, long loopLength, int outputCount)
    {
        List<long> validValues = [];

        long loopValue = loopStart;
        for (int n = 0; n < 1000000; n++)
        {
            var p2Comp = Original.Clone();

            p2Comp.A = loopValue;

            for (int c = 0; c < outputCount; c++)
                p2Comp.RunToOutput();

            if (p2Comp.Output.Count == outputCount)
            {
                bool isMatch = true;
                for (int c = 0; c < outputCount; c++)
                {
                    if (p2Comp.Output[c] != Original.Program[c])
                        isMatch = false;
                }
                if (isMatch)
                    validValues.Add(loopValue);
            }
            loopValue += loopLength;
        }

        return validValues;
    }
}