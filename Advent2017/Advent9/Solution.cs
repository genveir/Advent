using Advent2017.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2017.Advent9
{
    public class Solution : ISolution
    {
        Group outerGroup;

        public Solution(string input)
        {
            var stream = Input.GetInput(input);

            int index = 0;
            outerGroup = Group.Parse(stream, ref index, 1);
        }
        public Solution() : this("Input.txt") { }

        public interface StreamItem 
        {
            long GetSumValue();
            long GetGarbageLength();
        }

        public class Group : StreamItem
        {
            public List<StreamItem> items = new List<StreamItem>();
            public long Value { get; set; }

            public long GetSumValue() => Value + items.Select(i => i.GetSumValue()).Sum();
            public long GetGarbageLength() => items.Select(i => i.GetGarbageLength()).Sum();

            public static Group Parse(string input, ref int index, int value)
            {
                index++;
                Group newGroup = new Group() { Value = value };
                while (true)
                {
                    switch (input[index])
                    {
                        case '}': return newGroup;
                        case '{': newGroup.items.Add(Group.Parse(input, ref index, value + 1)); break;
                        case '<': newGroup.items.Add(Garbage.Parse(input, ref index)); break;
                    }
                    index++;
                }
            }
        }

        public class Garbage : StreamItem
        {
            public string content;

            public long GetSumValue() => 0;
            public long GetGarbageLength() => content.Length;

            public static Garbage Parse(string input, ref int index)
            {
                index++;
                List<char> content = new List<char>();

                while (true)
                {
                    switch(input[index])
                    {
                        case '!': index++; break;
                        case '>': return new Garbage() { content = new string(content.ToArray()) };
                        default: content.Add(input[index]); break;
                    }
                    index++;
                }
            }
        }

        public object GetResult1()
        {
            return outerGroup.GetSumValue();
        }

        public object GetResult2()
        {
            return outerGroup.GetGarbageLength();
        }
    }
}
