using AdventOfCode2025.Data;

namespace AdventOfCode2025.Days;

internal class Day2 : IDay
{
    public string Solve()
    {
        var input = EmbeddedResource.ReadInput($"Input{GetType().Name}.txt");

        var part1Result = FindInvalidIdsSum(input, true);
        var part2Result = FindInvalidIdsSum(input, false);

        return $"Part 1: {part1Result}, Part 2: {part2Result}";
    }

    private static long FindInvalidIdsSum(string input, bool part1)
    {
        var ranges = input.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x =>
            {
                var startEnd = x.Split('-');
                var range = (long.Parse(startEnd[0]), long.Parse(startEnd[1]));
                return range;
            }).ToList();

        var invalidIdSum = 0L;
        foreach (var range in ranges)
        {
            var invalidSums = FetchInvalidIds(range, part1);
            invalidIdSum += invalidSums;
        }
        return invalidIdSum;
    }

    private static long FetchInvalidIds((long start, long end) range, bool part1)
    {
        long result = 0;
        for (var i = range.start; i < range.end + 1; i++)
        {
            if (part1)
            {
                if (InvalidIdPart1(i))
                {
                    result += i;
                }
            }
            else
            {
                if (InvalidIdPart2(i))
                {
                    result += i;
                }
            }
        }
        return result;
    }

    private static bool InvalidIdPart1(long number)
    {
        var numberStr = number.ToString();
        if (numberStr.Length % 2 != 0 || numberStr[numberStr.Length / 2] == '0')
        {
            return false;
        }

        int half = numberStr.Length / 2;
        for (int i = 0, j = half; i < half; i++, j++)
        {
            if (numberStr[i] != numberStr[j])
            {
                return false;
            }
        }
        return true;
    }

    private static bool InvalidIdPart2(long number)
    {
        var numberStr = number.ToString();

        var currentCandidate = "";
        foreach (var c in numberStr)
        {
            currentCandidate += c;
            if (currentCandidate.Length > numberStr.Length / 2) { break; }

            if (IsInvalid(currentCandidate, numberStr))
            {
                return true;
            }
        }
        return false;
    }

    private static bool IsInvalid(string currentCandidate, string numberStr)
    {
        if (numberStr.Length % currentCandidate.Length != 0) { return false; }

        for (var i = 0; i < numberStr.Length; i++)
        {
            if (currentCandidate[i % currentCandidate.Length] != numberStr[i])
            {
                return false;
            }
        }
        return true;
    }
}
