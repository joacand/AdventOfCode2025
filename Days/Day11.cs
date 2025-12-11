using AdventOfCode2025.Data;

namespace AdventOfCode2025.Days;

internal class Day11 : IDay
{
    private Dictionary<Node, long> Memo { get; } = [];

    public string Solve()
    {
        var input = EmbeddedResource.ReadInput($"Input{GetType().Name}.txt");

        var part1Result = CountPaths(input.Split(Environment.NewLine));
        var part2Result = CountFftDacPaths(input.Split(Environment.NewLine));

        return $"Part 1: {part1Result}, Part 2: {part2Result}";
    }

    private long CountPaths(string[] strings)
    {
        var startNode = CreateGraph(strings, "you");
        return Visit(startNode, "out");
    }

    /// <summary>
    /// For my input only fft->dac is valid. For other inputs it might be dac->fft which would require switching the order around.
    /// </summary>
    private long CountFftDacPaths(string[] strings)
    {
        Memo.Clear();
        var startNode = CreateGraph(strings, "svr");
        var a = Visit(startNode, "fft");

        Memo.Clear();
        startNode = CreateGraph(strings, "fft");
        var b = Visit(startNode, "dac");

        Memo.Clear();
        startNode = CreateGraph(strings, "dac");
        var c = Visit(startNode, "out");

        return a * b * c;
    }

    private long Visit(Node node, string goalNode)
    {
        if (Memo.TryGetValue(node, out long value))
        {
            return value;
        }
        if (node.Value == goalNode)
        {
            Memo[node] = 1;
            return 1;
        }

        var paths = 0L;
        foreach (var n in node.Neighbors)
        {
            paths += Visit(n, goalNode);
        }
        Memo[node] = paths;
        return paths;
    }

    private static Node CreateGraph(IEnumerable<string> input, string startNodeName)
    {
        var nodes = new List<Node>();
        foreach (var line in input)
        {
            var split = line.Split(':');
            var value = split[0].Trim();
            var neighbors = split[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();

            var node = new Node { Value = value };
            if (!nodes.Contains(node))
            {
                nodes.Add(node);
            }
            else
            {
                node = nodes.First(n => n == node);
            }
            foreach (var neighborValue in neighbors)
            {
                var neighborNode = new Node { Value = neighborValue };
                if (!nodes.Contains(neighborNode))
                {
                    nodes.Add(neighborNode);
                }
                else
                {
                    neighborNode = nodes.First(n => n == neighborNode);
                }
                node.Neighbors.Add(neighborNode);
            }
        }
        return nodes.First(n => n.Value == startNodeName);
    }

    private class Node : IEquatable<Node?>
    {
        public string Value { get; set; } = string.Empty;
        public List<Node> Neighbors { get; set; } = [];

        public override bool Equals(object? obj)
        {
            return Equals(obj as Node);
        }

        public bool Equals(Node? other)
        {
            return other is not null &&
                   Value == other.Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value);
        }

        public static bool operator ==(Node? left, Node? right)
        {
            return EqualityComparer<Node>.Default.Equals(left, right);
        }

        public static bool operator !=(Node? left, Node? right)
        {
            return !(left == right);
        }
    }
}
