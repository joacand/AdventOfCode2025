using AdventOfCode2025.Data;

namespace AdventOfCode2025.Days;

internal class Day5 : IDay
{
    public string Solve()
    {
        var input = EmbeddedResource.ReadInput($"Input{GetType().Name}.txt");
        var database = new Database(input.Split(Environment.NewLine));

        var part1Result = CalculateFreshIngredients(database);
        var part2Result = CalculateNumberOfFreshIngredients(database.FreshRanges);

        return $"Part 1: {part1Result}, Part 2: {part2Result}";
    }

    private static long CalculateFreshIngredients(Database database) =>
        database.Ingredients.Count(x => database.FreshRanges.Any(range => IsInRange(x, range.start, range.end)));

    private static bool IsInRange(long ingredient, long start, long end) =>
        ingredient >= start && ingredient <= end;

    private static long CalculateNumberOfFreshIngredients(List<(long start, long end)> freshRanges)
    {
        freshRanges = freshRanges.OrderBy(x => x.start).ToList();

        bool overlapFound;
        do
        {
            overlapFound = false;
            for (var i = 0; i < freshRanges.Count - 1; i++)
            {
                var currentRange = freshRanges[i];
                var (nextRangeStart, nextRangeEnd) = freshRanges[i + 1];

                if (nextRangeStart <= currentRange.end + 1)
                {
                    overlapFound = true;
                    currentRange.end = Math.Max(currentRange.end, nextRangeEnd);
                    freshRanges[i] = currentRange;
                    freshRanges.RemoveAt(i + 1);
                }
            }
        } while (overlapFound);

        return freshRanges.Sum(range => range.end - range.start + 1);
    }

    private class Database
    {
        public List<(long start, long end)> FreshRanges = [];
        public List<long> Ingredients = [];

        public Database(string[] lines)
        {
            bool ingredientsSection = false;
            foreach (var line in lines)
            {
                if (line == string.Empty)
                {
                    ingredientsSection = true;
                    continue;
                }

                if (!ingredientsSection)
                {
                    var rangeParts = line.Split('-');
                    FreshRanges.Add((long.Parse(rangeParts[0]), long.Parse(rangeParts[1])));
                }
                else
                {
                    Ingredients.Add(long.Parse(line));
                }
            }
        }
    }
}
