using Advent2019.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2019.Advent6
{
    public class Solution : ISolution
    {
        Dictionary<string, Body> bodies;
        

        Body root;

        public Solution(Input.InputMode inputMode, string input)
        {
            var lines = Input.GetInputLines(inputMode, input).ToArray();

            bodies = new Dictionary<string, Body>();
            HashSet<string> allbodies = new HashSet<string>();
            HashSet<string> allbodiesInOrbit = new HashSet<string>();

            foreach (var line in lines)
            {
                var split = line.Split(")");
                var baseBody = split[0];
                var orbitBody = split[1];

                if (!bodies.ContainsKey(baseBody)) bodies.Add(baseBody, new Body(baseBody));
                if (!bodies.ContainsKey(orbitBody)) bodies.Add(orbitBody, new Body(orbitBody));

                bodies[baseBody].AddOrbit(bodies[split[1]]);

                allbodies.Add(baseBody);
                allbodies.Add(orbitBody);
                allbodiesInOrbit.Add(orbitBody);
            }

            root = bodies[allbodies.Except(allbodiesInOrbit).Single()];
        }
        public Solution() : this(Input.InputMode.Embedded, "Input") { }

        
        private class Body
        {
            public string Name;
            public List<Body> InOrbit;
            public Body Parent;

            public Body(string name)
            {
                Name = name;
                InOrbit = new List<Body>();
            }

            public void AddOrbit(Body child)
            {
                InOrbit.Add(child);
                child.Parent = this;
            }

            public IEnumerable<Body> GetParents()
            {
                if (Parent == null) return new List<Body>();

                var list = new List<Body>();
                list.Add(Parent);
                list.AddRange(Parent.GetParents());

                return list;
            }

            public int? parentCount;
            public int CountParents(bool reset = false)
            {
                if (reset) parentCount = null;
                parentCount = parentCount ?? 1 + Parent?.CountParents(reset) ?? 0;

                return parentCount.Value;
            }

            public override string ToString()
            {
                return "Body: " + Name;
            }
        }


        public string GetResult1()
        {
            var result = bodies.Values.Sum(b => b.CountParents()).ToString();

            return result;
        }

        public string GetResult2()
        {
            var myBody = bodies["YOU"];
            var sanBody = bodies["SAN"];

            var myParents = myBody.GetParents().ToArray();
            var sanParents = sanBody.GetParents().ToArray();

            var targets = myParents.Intersect(sanParents);

            foreach (var body in targets) body.Parent = null;

            var dist = myBody.Parent.CountParents(true) + sanBody.Parent.CountParents(true);
            return dist.ToString();

            // not 563
        }
    }
}
