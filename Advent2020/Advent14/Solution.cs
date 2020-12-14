using Advent2020.Advent14.Tree;
using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2020.Advent14
{
    public class Solution : ISolution
    {
        public List<IInstruction> instructions = new List<IInstruction>();

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            for (int n = 0; n < lines.Length; n++)
            {
                var line = lines[n];

                if (line.StartsWith("mask = ")) instructions.Add(new SetMask(line.Substring("mask = ".Length).Trim()));
                else
                {
                    var split = line.Split(new char[] { '[', ']', ' ', '=' }, StringSplitOptions.RemoveEmptyEntries);
                    instructions.Add(new SetValue(long.Parse(split[1]), int.Parse(split[2])));
                }
            }
        }
        public Solution() : this("Input.txt") { }

        public interface IInstruction
        {
            void Execute(Memory memory);
        }

        public class SetMask : IInstruction
        {
            public BitMask mask;

            public SetMask(string mask)
            {
                this.mask = BitMask.FromString(mask);
            }

            public void Execute(Memory memory)
            {
                memory.SetMask(this.mask);
            }

            public override string ToString()
            {
                return $"SetMask {mask}";
            }
        }

        public class SetValue : IInstruction
        {
            MemoryPosition memPosition;
            MemoryValue value;

            public SetValue(long memPosition, long value)
            {
                this.memPosition = MemoryPosition.FromLong(memPosition);
                this.value = MemoryValue.FromLong(value);
            }

            public void Execute(Memory memory)
            {
                memory.SetValue(memPosition, value);
            }

            public override string ToString()
            {
                return $"SetValue {memPosition}, {value}";
            }
        }

        public Memory RunProgram(int chipVersion)
        {
            Memory memory = new Memory(chipVersion);

            for (int n = 0; n < instructions.Count; n++)
            {
                instructions[n].Execute(memory);
            }

            return memory;
        }

        public object GetResult1()
        {
            return RunProgram(1).GetSummedValue();
        }

        public object GetResult2()
        {
            return RunProgram(2).GetSummedValue();
        }
    }
}
