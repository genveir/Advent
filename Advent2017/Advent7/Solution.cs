using Advent2017.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2017.Advent7
{
    public class Solution : ISolution
    {
        TreeNode rootNode;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            var protoNodes = lines.Select(line =>
            {
                var split = line.Split(new char[] { ' ', '-', '>', '(', ')', ',' }, StringSplitOptions.RemoveEmptyEntries);

                return new ProtoNode()
                {
                    Name = split[0],
                    Weight = long.Parse(split[1]),
                    Children = split.Segment(2, split.Length - 2)
                };
            }).ToDictionary(pn => pn.Name, pn => pn);

            var treeNodes = new Dictionary<string, TreeNode>();

            var tree = protoNodes.Select(pn => TreeNode.GetOrCreate(pn.Value, protoNodes, treeNodes)).ToList();

            var node = tree.First();
            while (node.Parent != null) node = node.Parent;
            rootNode = node;
        }
        public Solution() : this("Input.txt") { }

        public class ProtoNode
        {
            public string Name;
            public long Weight;
            public string[] Children;
        }

        public class TreeNode
        {
            public string Name;
            public long Weight;

            public TreeNode Parent;
            public List<TreeNode> Children = new List<TreeNode>();

            private long _subTreeWeight = -1;
            public long GetSubTreeWeight()
            {
                if (_subTreeWeight == -1)
                {
                    _subTreeWeight = this.Weight + Children.Select(c => c.GetSubTreeWeight()).Sum();
                }
                return _subTreeWeight;
            }

            public static TreeNode GetOrCreate(ProtoNode pn, Dictionary<string, ProtoNode> pnodes, Dictionary<string, TreeNode> constructedNodes)
            {
                if (constructedNodes.ContainsKey(pn.Name)) return constructedNodes[pn.Name];

                var treeNode = new TreeNode();
                
                treeNode.Name = pn.Name;
                treeNode.Weight = pn.Weight;

                constructedNodes.Add(treeNode.Name, treeNode);
                foreach (var child in pn.Children)
                {
                    var childNode = GetOrCreate(pnodes[child], pnodes, constructedNodes);

                    treeNode.Children.Add(childNode);
                    childNode.Parent = treeNode;
                }
                
                return treeNode;
            }
        }

        public object GetResult1()
        {
            return rootNode.Name;
        }

        public object GetResult2()
        {
            var node = rootNode;
            long difference = 0;

            while (true)
            {
                var subTreesByWeight = node.Children.GroupBy(c => c.GetSubTreeWeight());
                if (subTreesByWeight.Count() != 1)
                {
                    var correctWeight = subTreesByWeight.Where(st => st.Count() > 1).Single().Key;
                    var invalidSubTree = subTreesByWeight.Where(st => st.Count() == 1).Single();
                    difference = correctWeight - invalidSubTree.Key;
                    node = invalidSubTree.Single();
                }
                else
                {
                    return node.Weight + difference;
                }
            }
        }
    }
}
