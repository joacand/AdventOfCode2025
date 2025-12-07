using AdventOfCode2025.Common;
using AdventOfCode2025.Data;
using AdventOfCode2025.Visualization;

namespace AdventOfCode2025.Days;

internal class Day7 : IDay
{
    private readonly bool enableVisualization = false;

    public Day7()
    {
        if (enableVisualization)
        {
            Console.Clear();
            Console.SetOut(new Day7Writer(Console.Out));
        }
    }

    public string Solve()
    {
        var input = EmbeddedResource.ReadInput($"Input{GetType().Name}.txt");
        var grid = CreateGrid(input.Split(Environment.NewLine));

        var part1Result = CalculateSplits(grid);
        // Part 2 solution uses the modified grid from Part 1.
        var part2Result = CalculateTimelines(grid);

        return $"Part 1: {part1Result}, Part 2: {part2Result}";
    }

    private int CalculateSplits(char[,] grid)
    {
        var splits = 0;
        for (var r = 0; r < grid.GetLength(0); r++)
        {
            List<Position> newBeams = [];
            for (var c = 0; c < grid.GetLength(1); c++)
            {
                if (grid[r, c] == 'S' || grid[r, c] == '|')
                {
                    newBeams.AddRange(CreateNewBeams(grid, new(r, c), ref splits));
                }
            }

            newBeams.ForEach(entry => grid[entry.Row, entry.Column] = '|');
            if (enableVisualization)
            {
                grid.Print();
                Console.WriteLine("CaretReset");
            }
        }
        return splits;
    }

    private static long CalculateTimelines(char[,] grid)
    {
        var sum = 0L;
        var positionalCounter = new Dictionary<Position, long>();

        for (var r = 0; r < grid.GetLength(0); r++)
        {
            for (var c = 0; c < grid.GetLength(1); c++)
            {
                var pos = new Position(r, c);
                if (!positionalCounter.ContainsKey(pos)) { positionalCounter[pos] = 0; }

                if (grid.V(pos) == 'S')
                {
                    positionalCounter[pos] = 1;
                }
                else if (grid.V(pos) == '|')
                {
                    if (grid.Up(pos) == '|' || grid.Up(pos) == 'S')
                    {
                        positionalCounter[pos] = positionalCounter[new(pos.Row - 1, pos.Column)];
                    }
                    if (grid.Left(pos) == '^')
                    {
                        var c1 = new Position(pos.Row - 1, pos.Column - 1);
                        var newVal = positionalCounter.TryGetValue(c1, out long value) ? value : 0;
                        positionalCounter[pos] += newVal;
                    }
                    if (grid.Right(pos) == '^')
                    {
                        var c1 = new Position(pos.Row - 1, pos.Column + 1);
                        var newVal = positionalCounter.TryGetValue(c1, out long value) ? value : 0;
                        positionalCounter[pos] += newVal;
                    }
                }
            }

        }

        for (var c = 0; c < grid.GetLength(1); c++)
        {
            sum += positionalCounter[new(grid.GetLength(0) - 1, c)];
        }

        return sum;
    }

    private static List<Position> CreateNewBeams(char[,] grid, Position position, ref int splits)
    {
        var result = new List<Position>();

        var c = grid.V(position);
        if (c == 'S')
        {
            result.Add(new(position.Row + 1, position.Column));
            return result;
        }
        if (c == '.')
        {
            return result;
        }
        if (c == '|' && position.Row + 1 < grid.GetLength(0))
        {
            if (grid.Down(position) == '^')
            {
                splits++;
                if (position.Row + 1 >= 0 && position.Column + 1 <= grid.GetLength(1))
                {
                    result.Add(new(position.Row + 1, position.Column + 1));
                }
                if (position.Row + 1 >= 0 && position.Column - 1 >= 0)
                {
                    result.Add(new(position.Row + 1, position.Column - 1));
                }
            }
            else if (grid.Down(position) == '.')
            {
                result.Add(new(position.Row + 1, position.Column));
            }
        }

        return result;
    }

    private static char[,] CreateGrid(string[] rows)
    {
        var grid = new char[rows.Length, rows[0].Length];
        for (var r = 0; r < rows.Length; r++)
        {
            for (var c = 0; c < rows[r].Length; c++)
            {
                grid[r, c] = rows[r][c];
            }
        }
        return grid;
    }
}
