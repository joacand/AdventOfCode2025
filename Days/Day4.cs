using AdventOfCode2025.Data;

namespace AdventOfCode2025.Days;

internal class Day4 : IDay
{
    public string Solve()
    {
        var input = EmbeddedResource.ReadInput($"Input{GetType().Name}.txt");
        var grid = CreateGrid(input.Split(Environment.NewLine));

        var part1Result = RollsOfAccessiblePapers(grid);
        var part2Result = RollsOfAccessiblePapersWithRemoval(grid);

        return $"Part 1: {part1Result}, Part 2: {part2Result}";
    }

    private static long RollsOfAccessiblePapers(char[,] grid)
    {
        var accessibleRolls = 0;
        for (var r = 0; r < grid.GetLength(0); r++)
        {
            for (var c = 0; c < grid.GetLength(1); c++)
            {
                var entry = grid[r, c];
                if (entry != '@') { continue; }

                if (HasMaxNeighbors(grid, r, c, 3))
                {
                    accessibleRolls++;
                }
            }
        }
        return accessibleRolls;
    }

    private static long RollsOfAccessiblePapersWithRemoval(char[,] grid)
    {
        var totalRemovedRolls = 0;

        List<(int, int)> toBeRemoved = [];
        do
        {
            toBeRemoved.Clear();
            for (var r = 0; r < grid.GetLength(0); r++)
            {
                for (var c = 0; c < grid.GetLength(1); c++)
                {
                    var entry = grid[r, c];
                    if (entry != '@') { continue; }

                    if (HasMaxNeighbors(grid, r, c, 3))
                    {
                        toBeRemoved.Add((r, c));
                    }
                }
            }
            toBeRemoved.ForEach(x => grid[x.Item1, x.Item2] = 'x');
            totalRemovedRolls += toBeRemoved.Count;
        } while (toBeRemoved.Count > 0);

        return totalRemovedRolls;
    }

    private static bool HasMaxNeighbors(char[,] grid, int r, int c, int maxNeighbors)
    {
        var count = 0;
        int[] directionRows = { -1, -1, -1, 0, 0, 1, 1, 1 };
        int[] directionColumns = { -1, 0, 1, -1, 1, -1, 0, 1 };

        for (var n = 0; n < directionRows.Length; n++)
        {
            var nr = r + directionRows[n];
            var nc = c + directionColumns[n];

            if (nr >= 0 && nr < grid.GetLength(0) && nc >= 0 && nc < grid.GetLength(1) &&
                grid[nr, nc] == '@')
            {
                count++;
                if (count > maxNeighbors)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private static char[,] CreateGrid(string[] rows)
    {
        var grid = new char[rows.Length, rows[0].Length];
        for (int r = 0; r < rows.Length; r++)
        {
            for (int c = 0; c < rows[r].Length; c++)
            {
                grid[r, c] = rows[r][c];
            }
        }
        return grid;
    }
}
