using AdventOfCode2025.Data;

namespace AdventOfCode2025.Days;

internal class Day6 : IDay
{
    public string Solve()
    {
        var input = EmbeddedResource.ReadInput($"Input{GetType().Name}.txt");

        var part1Result = SumAllProblems(input, false);
        var part2Result = SumAllProblems(input, true);

        return $"Part 1: {part1Result}, Part 2: {part2Result}";
    }

    /// <summary>
    /// Sums the problems.
    /// Since there are no negative numbers we can be lazy and represent addition as -1 and multiplcation as -2.
    /// </summary>
    private static long SumAllProblems(string input, bool isCephalopodMath)
    {
        var structure = isCephalopodMath
            ? ParseCephalopodStructure(input)
            : ParseVerticalStructure(input);

        return structure.Sum(x =>
            x.Last() == -1
                ? x.Take(x.Count - 1).Sum()
                : x.Take(x.Count - 1).Aggregate(1L, (acc, val) => acc * val));
    }

    private static List<List<long>> ParseVerticalStructure(string input)
    {
        var structure = new List<List<long>>();
        foreach (var line in input.Split(Environment.NewLine))
        {
            var entries = line.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < entries.Length; i++)
            {
                if (entries[i] == "+") { structure[i].Add(-1); }
                else if (entries[i] == "*") { structure[i].Add(-2); }
                else
                {
                    if (structure.Count <= i) { structure.Add([]); }
                    structure[i].Add(long.Parse(entries[i]));
                }
            }
        }
        return structure;
    }

    private static List<List<long>> ParseCephalopodStructure(string input)
    {
        var lines = input.Split(Environment.NewLine);
        var result = new List<List<long>>();
        var problems = new List<List<string>>();

        var columnLengths = GetColumnLengths(lines);

        foreach (var line in lines)
        {
            var counter = 0;
            for (var i = 0; i < columnLengths.Count; i++)
            {
                var problem = new string([.. line.Skip(counter).Take(columnLengths[i])]);
                if (problems.Count <= i) { problems.Add([]); }
                problems[i].Add(problem);
                counter += columnLengths[i];
            }
        }

        for (var i = 0; i < problems.Count; i++)
        {
            var verticalStructure = ConvertToVerticalStructure(problems[i]);
            var entry = verticalStructure.Select(x =>
            {
                if (x == "+") { return -1; }
                else if (x == "*") { return -2; }
                return long.Parse(x.Trim());
            });
            result.Add([.. entry]);
        }

        return result;
    }

    private static IEnumerable<string> ConvertToVerticalStructure(List<string> list)
    {
        var results = new List<string>();

        for (var i = list.Last().Length - 1; i >= 0; i--)
        {
            var result = string.Empty;
            foreach (var line in list)
            {
                if (line[i] != ' ' && line[i] != '*' && line[i] != '+')
                {
                    result += line[i];
                }
            }
            results.Add(result);
        }

        results.Add(list.Last().Trim());

        return [.. results.Where(x => !string.IsNullOrWhiteSpace(x))];
    }

    private static List<int> GetColumnLengths(string[] lines)
    {
        var result = new List<int>();
        var operations = lines.Last();

        var currentCount = 0;
        for (var i = 1; i < operations.Length; i++)
        {
            if (operations[i] == ' ')
            {
                currentCount++;
            }
            else
            {
                result.Add(currentCount + 1);
                currentCount = 0;
            }
        }
        result.Add(currentCount + 1);
        return result;
    }
}
