using AdventOfCode2025.Data;

namespace AdventOfCode2025.Days;

internal class Day8 : IDay
{
    public string Solve()
    {
        var input = EmbeddedResource.ReadInput($"Input{GetType().Name}.txt");

        var part1Result = ConnectCircuits(input.Split(Environment.NewLine), false);
        var part2Result = ConnectCircuits(input.Split(Environment.NewLine), true);

        return $"Part 1: {part1Result}, Part 2: {part2Result}";
    }

    private static long ConnectCircuits(string[] junctionsStr, bool part2)
    {
        var numberOfJunctions = junctionsStr.Length;
        List<Junction> junctions = ParseJunctions(junctionsStr).ToList();
        List<Circuit> circuits = [];
        List<(Junction, Junction, double)> distances = [];

        for (var i = 0; i < junctions.Count; i++)
        {
            for (var j = i + 1; j < junctions.Count; j++)
            {
                distances.Add((junctions[i], junctions[j], junctions[i].DistanceTo(junctions[j])));
            }
        }
        distances = distances.OrderBy(x => x.Item3).ToList();

        var connectionsToMake = part2 ? distances.Count : numberOfJunctions > 20 ? 1000 : 10;
        for (var i = 0; i < connectionsToMake; i++)
        {
            var entry = distances[i];

            var matchingCircuits = circuits.Where(c => c.ContainsAny(entry.Item1, entry.Item2)).ToList();

            if (matchingCircuits.Count == 0)
            {
                var newCircuit = new Circuit([entry.Item1, entry.Item2]);
                circuits.Add(newCircuit);
            }
            else if (matchingCircuits.Count == 1)
            {
                matchingCircuits[0].Add(entry.Item1, entry.Item2);
            }
            else
            {
                matchingCircuits[0].Add(entry.Item1, entry.Item2);
                // Merge circuits
                for (var c = 1; c < matchingCircuits.Count; c++)
                {
                    matchingCircuits[0].Add(matchingCircuits[c].Junctions);
                    circuits.Remove(matchingCircuits[c]);
                }
            }

            if (part2 && circuits.Count == 1 && circuits[0].Size == numberOfJunctions)
            {
                return (long)entry.Item1.X * entry.Item2.X;
            }
        }

        return circuits.OrderByDescending(x => x.Size).Take(3).Select(x => x.Size).Aggregate(1, (long acc, long val) => acc * val);
    }

    private static IEnumerable<Junction> ParseJunctions(string[] junctionsStr)
    {
        return junctionsStr.Select(x =>
        {
            var parts = x.Split(',');
            return new Junction(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
        });
    }

    private record Circuit(List<Junction> Junctions)
    {
        public long Size => Junctions.Count;
        public bool ContainsAny(Junction l, Junction r) => Junctions.Contains(l) || Junctions.Contains(r);
        public bool ContainsAll(Junction l, Junction r) => Junctions.Contains(l) && Junctions.Contains(r);
        public bool Add(Junction l, Junction r)
        {
            if (ContainsAll(l, r)) { return false; }
            if (!Junctions.Contains(l)) { Junctions.Add(l); }
            if (!Junctions.Contains(r)) { Junctions.Add(r); }
            return true;
        }

        internal void Add(List<Junction> junctions)
        {
            foreach (var j in junctions)
            {
                if (!Junctions.Contains(j)) { Junctions.Add(j); }
            }
        }
    }

    private record Junction(int X, int Y, int Z)
    {
        public double DistanceTo(Junction other)
        {
            return Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2) + Math.Pow(Z - other.Z, 2));
        }
    }
}
