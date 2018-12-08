﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Advent
{
    class Advent8
    {
        private TreeNode GetInput()
        {
            string resourceName = "Advent.Input.Advent8Input.txt";
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
            Console.WriteLine(GetInput().MetadataSum);
            Console.WriteLine(GetInput().Part2Sum);
        }
    }
}
