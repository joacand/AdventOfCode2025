using AdventOfCode2025.Data;

namespace AdventOfCode2025.Days;

internal class Day1 : IDay
{
    private int dialPosition = 50;
    private int password = 0;
    private bool countAnyClick;

    public string Solve()
    {
        var input = EmbeddedResource.ReadInput($"Input{GetType().Name}.txt");
        countAnyClick = false;

        Rotate(input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
        var part1Result = password;

        countAnyClick = true;
        dialPosition = 50;
        password = 0;

        Rotate(input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
        var part2Result = password;

        return $"Part 1: {part1Result}, Part 2: {part2Result}";
    }

    private void Rotate(string[] instructions)
    {
        foreach (var instruction in instructions)
        {
            var direction = instruction[0];
            var clicks = int.Parse(instruction[1..]);

            if (direction == 'R')
            {
                RotateRight(clicks);
            }
            else
            {
                RotateLeft(clicks);
            }
        }
    }

    private void RotateRight(int clicks)
    {
        dialPosition += clicks;

        if (countAnyClick)
        {
            password += dialPosition / 100;
        }
        else
        {
            if (dialPosition % 100 == 0) { password++; }
        }

        dialPosition %= 100;
    }

    private void RotateLeft(int clicks)
    {
        dialPosition -= clicks;

        if (dialPosition > 0)
        {
            return;
        }

        if (dialPosition == 0)
        {
            password++;
            return;
        }

        if (countAnyClick)
        {
            var rotations = Math.Abs(dialPosition / 100);
            password += rotations;
            if ((dialPosition + clicks) != 0)
            {
                password++;
            }
        }

        dialPosition = ((dialPosition % 100) + 100) % 100;

        if (!countAnyClick)
        {
            if (dialPosition == 0) { password++; }
        }
    }
}
