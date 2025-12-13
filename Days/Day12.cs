using AdventOfCode2025.Data;

namespace AdventOfCode2025.Days;

internal class Day12 : IDay
{
    public string Solve()
    {
        var input = EmbeddedResource.ReadInput($"Input{GetType().Name}.txt");

        var treeRegions = new TreeRegions(input.Split(Environment.NewLine));

        var part1Result = CalculatePossibleRegions(treeRegions);
        var part2Result = "Not implemented";

        return $"Part 1: {part1Result}, Part 2: {part2Result}";
    }

    private static long CalculatePossibleRegions(TreeRegions treeRegions) => treeRegions.SumObviousRegions();

    private class TreeRegions
    {
        public List<(long, long)> RegionToPacketSizes = [];

        public TreeRegions(string[] lines)
        {
            foreach (var line in lines.Skip(30))
            {
                var lineSplit = line.Split(':');
                var treeRegionSplit = lineSplit[0].Split('x');
                var regionSize = long.Parse(treeRegionSplit[0]) * long.Parse(treeRegionSplit[1]);

                var roughPacketSizeSplit = lineSplit[1].Trim().Split(' ');
                var roughPacketSize = roughPacketSizeSplit.Select(x => long.Parse(x) * 9).Sum();

                RegionToPacketSizes.Add((regionSize, roughPacketSize));
            }
        }

        public long SumObviousRegions() => RegionToPacketSizes.Count(x => x.Item1 >= x.Item2);
    }
}
