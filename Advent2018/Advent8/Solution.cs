using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Advent2018.Advent8
{
    class Solution : ISolution
    {
        private TreeNode GetInput()
        {
            string resourceName = "Advent2018.Advent8.Input.txt";
            var input = typeof(Program).Assembly.GetManifestResourceStream(resourceName);

            int[] inputInts;
            using (var txt = new StreamReader(input))
            {
                inputInts = txt.ReadToEnd().Split().Select(c => int.Parse(c)).ToArray();
            }

            int cursor = 0;
            return new TreeNode(null, inputInts, ref cursor);
        }

        private class TreeNode
        {
            public List<TreeNode> Children;
            public int MetadataSum = 0;
            public int Part2Sum = 0;

            public TreeNode(TreeNode parent, int[] input, ref int cursor)
            {
                Children = new List<TreeNode>();

                int numChildren = input[cursor++];
                int numMetadata = input[cursor++];

                for (int n = 0; n < numChildren; n++)
                {
                    Children.Add(new TreeNode(this, input, ref cursor));
                    MetadataSum += Children[n].MetadataSum;
                }

                for (int n = 0; n < numMetadata; n++)
                {
                    var metadataVal = input[cursor++];
                    
                    MetadataSum += metadataVal;

                    if (numChildren == 0) Part2Sum += metadataVal;
                    else Part2Sum += (metadataVal <= numChildren) ? Children[metadataVal-1].Part2Sum : 0;
                }
            }
        }

        public void WriteResult()
        {
            var root = GetInput();

            Console.WriteLine("part1: " + root.MetadataSum);
            Console.WriteLine("part2: " + root.Part2Sum);
        }
    }
}
