using AdventOfCode2025.Data;

namespace AdventOfCode2025.Days;

internal class Day9 : IDay
{
    public string Solve()
    {
        var input = EmbeddedResource.ReadInput($"Input{GetType().Name}.txt");
        List<(int l, int r)> redTiles = input.Split(Environment.NewLine).Select(line =>
        {
            var parts = line.Split(',');
            return (int.Parse(parts[0]), int.Parse(parts[1]));
        }).ToList();
        List<(int l, int r)> greenTiles = GetGreenTiles(redTiles);

        var part1Result = CalculateMaxArea(redTiles);
        var part2Result = "Not implemented";

        return $"Part 1: {part1Result}, Part 2: {part2Result}";
    }

    /// <summary>
    /// Creates the green tiles (borders of the polygon).
    /// </summary>
    private static List<(int l, int r)> GetGreenTiles(List<(int l, int r)> redTiles)
    {
        var greenSquares = new List<(int l, int r)>();

        for (var i = 0; i < redTiles.Count; i++)
        {
            var (l, r) = redTiles[i];
            var redTileAhead = redTiles[(i + 1) % redTiles.Count];

            if (l == redTileAhead.l)
            {
                // Same row
                var minx = Math.Min(r, redTileAhead.r);
                var maxx = Math.Max(r, redTileAhead.r);

                for (var x = minx + 1; x < maxx; x++)
                {
                    greenSquares.Add((l, x));
                }
            }
            else if (r == redTileAhead.r)
            {
                // Same column
                var minx = Math.Min(l, redTileAhead.l);
                var maxx = Math.Max(l, redTileAhead.l);

                for (var x = minx + 1; x < maxx; x++)
                {
                    greenSquares.Add((x, r));
                }
            }
        }

        return greenSquares;
    }

    private static long CalculateMaxArea(List<(int l, int r)> redTiles)
    {
        var areas = new List<((int l, int r) a, (int l, int r) b, long)>();
        for (var i = 0; i < redTiles.Count; i++)
        {
            for (var j = i + 1; j < redTiles.Count; j++)
            {
                areas.Add((redTiles[i], redTiles[j], Area(redTiles[i], redTiles[j])));
            }
        }
        var candidates = areas.OrderByDescending(x => x.Item3);
        return candidates.First().Item3;
    }

    private static long Area((int x, int y) a1, (int x, int y) a2)
    {
        return (long)Math.Abs(a1.x - a2.x + 1) * Math.Abs(a1.y - a2.y + 1);
    }
}
