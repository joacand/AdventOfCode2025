using AdventOfCode2025.Data;

namespace AdventOfCode2025.Days;

internal class Day3 : IDay
{
    public string Solve()
    {
        var input = EmbeddedResource.ReadInput($"Input{GetType().Name}.txt");

        var part1Result = MaximumJoltageSum(input.Split(Environment.NewLine), 2);
        var part2Result = MaximumJoltageSum(input.Split(Environment.NewLine), 12);

        return $"Part 1: {part1Result}, Part 2: {part2Result}";
    }

    private static long MaximumJoltageSum(string[] batteryBanks, int numberOfBatteries) =>
        batteryBanks.Sum(x => MaximumJoltage(x, numberOfBatteries));

    private static long MaximumJoltage(string batteryBank, int numberOfBatteries)
    {
        var maxIndexes = new int[numberOfBatteries];

        var currentMax = 0;
        for (var batteryIndex = 0; batteryIndex < maxIndexes.Length; batteryIndex++)
        {
            currentMax = 0;
            var lastBatteryIndex = batteryIndex - 1;
            var startIndex = lastBatteryIndex < 0 ? batteryIndex : maxIndexes[lastBatteryIndex] + 1;

            for (var i = startIndex; i < batteryBank.Length - (numberOfBatteries - batteryIndex - 1); i++)
            {
                if (ToInt(batteryBank[i]) > currentMax)
                {
                    currentMax = ToInt(batteryBank[i]);
                    maxIndexes[batteryIndex] = i;
                }
            }
        }

        return long.Parse(string.Join("", maxIndexes.Select(i => batteryBank[i].ToString())));
    }

    private static int ToInt(char c) => c - '0';
}
