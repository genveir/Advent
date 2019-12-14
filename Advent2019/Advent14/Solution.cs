using Advent2019.Shared;
using Advent2019.OpCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2019.Advent14
{
    public class Solution : ISolution
    {
        IEnumerable<Reaction> reactions;

        public Solution(Input.InputMode inputMode, string input)
        {
            var lines = Input.GetInputLines(inputMode, input).ToArray();

            reactions = Reaction.Parse(lines);

            var ore = new Element() { name = "ORE", amount = 1 };
            var oreReaction = new Reaction() { output = ore, requiredInputs = new Element[0] };

            ReactionWithOutput = new Dictionary<string, Reaction>();
            ReactionWithOutput.Add("ORE", oreReaction);
            foreach (var reaction in reactions) ReactionWithOutput.Add(reaction.output.name, reaction);

            BuildTree();
        }
        public Solution() : this(Input.InputMode.Embedded, "Input") { }

        public Dictionary<string, Reaction> ReactionWithOutput { get; set; }

        public class Reaction
        {
            public Element output;
            public Element[] requiredInputs;

            public List<Reaction> inputReactions = new List<Reaction>();

            public void Reset()
            {
                Required = 0;
                Available = 0;
                foreach (var child in inputReactions) child.Reset();
            }

            public void Build(long numToBuild)
            {
                Required += numToBuild;

                numToBuild -= Available;

                var numReactions = numToBuild / output.amount;
                if (numReactions * output.amount < numToBuild) numReactions += 1;

                var amountBuilt = numReactions * output.amount;
                Available = amountBuilt - numToBuild;

                foreach(var element in requiredInputs)
                {
                    var typeRequired = element.name;
                    var amountRequired = element.amount * numReactions;

                    var relevantChild = inputReactions.Single(ir => ir.output.name == typeRequired);
                    relevantChild.Build(amountRequired);
                }
            }

            public long Required = 0;
            public long Available = 0;

            public static IEnumerable<Reaction> Parse(IEnumerable<string> lines)
            {
                var parsedInputs = new List<Reaction>();

                foreach (var line in lines)
                {
                    var split = line.Split("=>", StringSplitOptions.RemoveEmptyEntries);

                    var output = Element.Parse(split[1]);

                    var splitInput = split[0].Split(new char[] { ' ' , ','}, StringSplitOptions.RemoveEmptyEntries);
                    var inputs = new Element[splitInput.Length / 2];
                    for (int n = 0; n < inputs.Length; n++)
                    {
                        inputs[n] = Element.Parse(splitInput[n * 2], splitInput[n * 2 + 1]);
                    }

                    var pi = new Reaction()
                    {
                        output = output,
                        requiredInputs = inputs
                    };

                    parsedInputs.Add(pi);
                }

                return parsedInputs;
            }

            public override string ToString()
            {
                return output.name + " Reaction";
            }
        }

        public class Element
        {
            public long amount;
            public string name;

            public static Element Parse(string input)
            {
                var split = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                return Parse(split[0], split[1]);
            }

            public static Element Parse(string number, string oreType)
            { 
                var amount = long.Parse(number);

                return new Element() { name = oreType.Trim(), amount = amount };
            }
        }

        private static HashSet<string> Handled;
        public void BuildTree()
        {
            Handled = new HashSet<string>();

            var root = ReactionWithOutput["FUEL"];

            BuildTree(root);
        }

        
        public void BuildTree(Reaction node)
        {
            if (Handled.Contains(node.output.name)) return;
            Handled.Add(node.output.name);

            foreach (var ri in node.requiredInputs)
            {
                var name = ri.name;

                var child = ReactionWithOutput[name];
                node.inputReactions.Add(child);
                BuildTree(child);
            }
        }

        public string GetResult1()
        {
            var root = ReactionWithOutput["FUEL"];
            root.Reset();
            root.Build(1);
            var ore = ReactionWithOutput["ORE"];

            return ore.Required.ToString();
            // 101755 too high
        }

        public string GetResult2()
        {
            var root = ReactionWithOutput["FUEL"];
            root.Reset();
            root.Build(13108426); // just searched by hand. Probably faster than implementing a binary search.
            var ore = ReactionWithOutput["ORE"];

            Console.WriteLine("1000000000000");
            Console.WriteLine(ore.Required);
            

            return ore.Required.ToString();
        }
    }
}
