using Advent2017.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2017.Advent6
{
    public class Solution : ISolution
    {
        long[] inputNums;

        public Solution(string input)
        {
            inputNums = Input.GetNumbers(input, new char[] { ' ', '\t' });
        }
        public Solution() : this("Input.txt") { }

        public class MemoryBanks
        {
            public long[] memory;
            

            public MemoryBanks(long[] memory)
            {
                this.memory = memory.DeepCopy();
            }

            public MemoryBanks ReAllocate()
            {
                long highest = -1;
                int topIndex = -1;
                for (int n = 0; n < memory.Length; n++)
                {
                    if (memory[n] > highest)
                    {
                        highest = memory[n];
                        topIndex = n;
                    }
                }

                long toAllocate = memory[topIndex];

                var newBank = new MemoryBanks(this.memory);
                newBank.memory[topIndex] = 0;
                var index = new ModNum(topIndex + 1, memory.Length);

                for (int n = 0; n < toAllocate; n++)
                {
                    newBank.memory[index.number]++;
                    index++;
                }
                return newBank;
            }

            public override int GetHashCode()
            {
                long result = 1;
                for (int n = 0; n < memory.Length; n++)
                {
                    result += n * memory[n];
                }
                return (int)result;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(this, obj)) return true;
                var other = obj as MemoryBanks;

                if (other == null) return false;
                return this.memory.SequenceEqual(other.memory);
            }

            public static bool operator ==(MemoryBanks first, MemoryBanks second)
            {
                if (ReferenceEquals(second, null)) return false;
                if (first.GetHashCode() == second.GetHashCode()) return first.Equals(second);
                return false;
            }

            public static bool operator !=(MemoryBanks first, MemoryBanks second) => !(first == second);
        }

        public object GetResult1()
        {
            var seenStates = new HashSet<MemoryBanks>();
            var currentState = new MemoryBanks(inputNums);

            int numRuns = 0;
            while(!seenStates.Contains(currentState))
            {
                seenStates.Add(currentState);
                currentState = currentState.ReAllocate();

                numRuns++;
            }
            return numRuns;
        }

        public object GetResult2()
        {
            var seenStates = new HashSet<MemoryBanks>();
            var currentState = new MemoryBanks(inputNums);

            int numRuns = 0;
            while (!seenStates.Contains(currentState))
            {
                seenStates.Add(currentState);
                currentState = currentState.ReAllocate();

                numRuns++;
            }

            var checkState = currentState;
            numRuns = 0;
            do
            {
                currentState = currentState.ReAllocate();
                numRuns++;
            } while (currentState != checkState);

            return numRuns;
        }
    }
}
