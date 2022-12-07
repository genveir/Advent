using Advent2022.ElfFS;
using Advent2022.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2022.Advent07
{
    public class Solution : ISolution
    {
        public State FileSystem;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            FileSystem = new();
            foreach (var line in lines) FileSystem.ExecuteCommand(line);
        }

        public Solution() : this("Input.txt") { }

        public object GetResult1()
        {
            return FileSystem.Root.GetFolderAndAllSubFolders()
                .Where(f => f.TotalSize <= 100000)
                .Sum(f => f.TotalSize);
        }

        public object GetResult2()
        {
            long emptySpace = 70000000 - FileSystem.Root.TotalSize;

            var requiredSpace = 30000000 - emptySpace;

            return FileSystem.Root.GetFolderAndAllSubFolders()
                .Select(f => f.TotalSize)
                .Where(f => f >= requiredSpace)
                .Min();
        }
    }
}
