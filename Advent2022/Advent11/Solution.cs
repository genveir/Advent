using Advent2022.ElfFileSystem;
using Advent2022.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2022.Advent11
{
    public class Solution : ISolution
    {
        private string input;

        public int SimulatedRounds = 0;
        public List<Monkey> Monkeys = new();
        public long modulus = 1;

        public Solution(string input)
        {
            this.input = input;
        }
        public Solution() : this("Input.txt") { }

        public void Reset()
        {
            SimulatedRounds = 0;
            Monkeys = new();
            modulus = 1;

            var lines = Input.GetInputLines(input).ToArray();

            for (int n = 0; n < lines.Length; n += 7)
            {
                long index = lines[n].Split(' ')[1].Parse<long>("val:");
                List<Item> items = lines[n + 1].Trim().Split(' ', 3)[2].Split(',').Select(s => s.Parse<Item>("id")).ToList();
                Action<Item> operation = ParseFunc(lines[n + 2].Trim().Split('=')[1].Trim());
                long divisor = lines[n + 3].Split(' ').Last().Parse<long>("val");
                int throwOnTrue = lines[n + 4].Split(' ').Last().Parse<int>("val");
                int throwOnFalse = lines[n + 5].Split(' ').Last().Parse<int>("val");

                modulus *= divisor;

                Monkeys.Add(new Monkey(index, items, operation, divisor, throwOnTrue, throwOnFalse));
            }
        }

        public Action<Item> ParseFunc(string func)
        {
            var parts = func.Split(' ');

            return parts[1] switch
            {
                "+" => ParseAddition(parts[0], parts[2]),
                "*" => ParseMultiplication(parts[0], parts[2]),
                _ => throw new NotSupportedException()
            };
        }

        public Action<Item> ParseAddition(string first, string second)
        {
            if (first == "old" && second == "old") return (Item item) => item.WorryLevel = item.WorryLevel + item.WorryLevel;

            var lSecond = long.Parse(second);
            return (Item item) => item.WorryLevel = item.WorryLevel + lSecond;
        }

        public Action<Item> ParseMultiplication(string first, string second)
        {
            if (first == "old" && second == "old") return (Item item) => item.WorryLevel = item.WorryLevel * item.WorryLevel;

            var lSecond = long.Parse(second);
            return (Item item) => item.WorryLevel = item.WorryLevel * lSecond;
        }

        public class Monkey
        {
            public long Index;
            public List<Item> Items;
            public Action<Item> Operation;
            public long DivisorForTest;
            public int ThrowOnTrue;
            public int ThrowOnFalse;

            public Monkey(long index, List<Item> items, Action<Item> operation, long divisor, int throwOnTrue, int throwOnFalse)
            {
                Index = index;
                Items = items;
                Operation = operation;
                DivisorForTest = divisor;
                ThrowOnTrue = throwOnTrue;
                ThrowOnFalse = throwOnFalse;
            }

            public void InspectItems(List<Monkey> monkeys, long modulus, bool useModulus)
            {
                var toCheck = new List<Item>(Items);

                for (int n = 0; n < toCheck.Count; n++)
                {
                    int target = InspectItem(toCheck[n], modulus, useModulus);

                    Throw(toCheck[n], monkeys[target]);
                }
            }

            public long NumberOfInspections = 0;

            public int InspectItem(Item item, long modulus, bool useModulus)
            {
                NumberOfInspections++;

                item.UpdateWorry(Operation);

                item.CalmDown(modulus, useModulus);

                return (item.WorryLevel % DivisorForTest == 0) ? ThrowOnTrue: ThrowOnFalse;
            }

            public void Throw(Item item, Monkey monkey)
            {
                Items.Remove(item);
                monkey.Catch(item);
            }

            public void Catch(Item item)
            {
                Items.Add(item);
            }
        }

        public class Item
        {
            public static long ItemIdCounter = 0;
            public long Id;
            public long WorryLevel;

            public Item(long worryLevel)
            {
                Id = ItemIdCounter++;
                WorryLevel = worryLevel;
            }

            public long WorryUpdated = 0;

            public void UpdateWorry(Action<Item> action)
            {
                WorryUpdated++;

                action(this);
            }

            public void CalmDown(long modulus, bool useModulus)
            {
                if (useModulus) WorryLevel = WorryLevel % modulus;
                else WorryLevel = WorryLevel / 3;
            }

            public override string ToString()
            {
                return $"Item {Id}, Worry {WorryLevel}";
            }
        }

        public void SimulateRound(bool useModulus)
        {
            for (int n = 0; n < Monkeys.Count; n++)
            {
                Monkeys[n].InspectItems(Monkeys, modulus, useModulus);
            }

            SimulatedRounds++;
        }

        public void SimulateToRound(int round, bool useModulus)
        {
            Reset();
            while(SimulatedRounds < round)
            {
                SimulateRound(useModulus);
            }
        }

        public object GetResult1()
        {
            SimulateToRound(20, false);

            return Monkeys
                .Select(m => m.NumberOfInspections)
                .OrderByDescending(n => n)
                .Take(2)
                .Aggregate((a, b) => a * b);
        }

        public object GetResult2()
        {
            SimulateToRound(10000, true);

            return Monkeys
                .Select(m => m.NumberOfInspections)
                .OrderByDescending(n => n)
                .Take(2)
                .Aggregate((a, b) => a * b);
        }
    }
}
