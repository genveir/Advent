namespace Advent2024.Day23;

public class Solution
{
    public List<Computer> Computers { get; set; } = [];

    public Solution(string input)
    {
        var lines = Input.GetInputLines(input).ToArray();

        var inputParser = new InputParser<string, string>("comp-comp");
        var parsed = inputParser.Parse(lines);

        HashSet<string> computers = [];
        foreach (var (comp1, comp2) in parsed)
        {
            computers.Add(comp1);
            computers.Add(comp2);
        }

        foreach (var comp in computers)
        {
            Computers.Add(new Computer(comp));
        }

        foreach (var (comp1, comp2) in parsed)
        {
            var computer1 = Computers.Single(c => c.Name == comp1);
            var computer2 = Computers.Single(c => c.Name == comp2);
            computer1.Connect(computer2);
        }
    }
    public Solution() : this("Input.txt") { }

    public class Computer
    {
        public string Name { get; set; }
        public HashSet<Computer> Connections { get; set; }

        public Computer(string name)
        {
            Name = name;
            Connections = [];
        }

        public void Connect(Computer other)
        {
            Connections.Add(other);
            other.Connections.Add(this);
        }

        public override string ToString()
        {
            return $"Computer {Name}";
        }
    }

    public object GetResult1()
    {
        var computersWithT = Computers
            .Where(c => c.Name.StartsWith("t"))
            .ToArray();

        long num = 0;
        foreach (var computer in computersWithT)
        {
            // tb => vc
            foreach (var connection in computer.Connections)
            {
                if (connection.Name.StartsWith("t") && string.Compare(computer.Name, connection.Name) < 0)
                    continue;

                // vc -> wq
                foreach (var c2 in connection.Connections)
                {
                    if (c2.Name.StartsWith("t") && string.Compare(computer.Name, c2.Name) < 0)
                        continue;

                    if (c2.Connections.Contains(computer))
                        num++;
                }
            }
        }

        return num / 2;
    }

    public List<Computer> FindMaximalClique()
    {
        HashSet<Computer> hasBeenChecked = [];
        List<Computer> largestGroup = [];
        foreach (var computer in Computers)
        {
            if (hasBeenChecked.Contains(computer))
                continue;

            List<Computer> clique = FindMaximalClique(computer);

            foreach (var comp in clique)
                hasBeenChecked.Add(comp);

            if (clique.Count > largestGroup.Count)
                largestGroup = clique;
        }

        return largestGroup;
    }

    public List<Computer> FindMaximalClique(Computer computer)
    {
        var connected = computer.Connections;

        return FindMaximalClique([computer], connected.ToList(), []).Item2;
    }

    public (bool, List<Computer>) FindMaximalClique(List<Computer> inClique, List<Computer> candidates, List<Computer> excluded)
    {
        while (true)
        {
            if (candidates.Count == 0 && excluded.Count == 0)
                return (true, inClique);

            foreach (var vertex in candidates)
            {
                var newClique = inClique.ToList();
                newClique.Add(vertex);

                var newCandidates = candidates.Intersect(vertex.Connections).ToList();
                var newExcluded = excluded.Intersect(vertex.Connections).ToList();

                var (succeeded, result) = FindMaximalClique(newClique, newCandidates, newExcluded);
                if (succeeded)
                    return (true, result);

                candidates.Remove(vertex);
                excluded.Add(vertex);
            }
        }
    }

    public object GetResult2()
    {
        List<Computer> largestGroup = FindMaximalClique();

        return string.Join(',', largestGroup.OrderBy(c => c.Name).Select(c => c.Name));
    }
}
