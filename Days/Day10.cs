using AdventOfCode2025.Data;

namespace AdventOfCode2025.Days;

internal class Day10 : IDay
{
    public string Solve()
    {
        var input = EmbeddedResource.ReadInput($"Input{GetType().Name}.txt");

        var part1Result = CalculateMinimumPresses(new Machine(input.Split(Environment.NewLine)));
        var part2Result = "Not implemented";

        return $"Part 1: {part1Result}, Part 2: {part2Result}";
    }

    private static long CalculateMinimumPresses(Machine machine) => machine.Lights.Sum(x => MinimumPresses(x));

    private static int MinimumPresses(Light light)
    {
        foreach (var entry in GeneratePotentialPresses(light.Buttons))
        {
            var state = 0L;
            foreach (var press in entry)
            {
                state ^= press;
            }
            if (state == light.DesiredState)
            {
                return entry.Count;
            }
        }
        throw new Exception("No solution found");
    }

    /// <summary>
    /// Generate all potential presses, "power set"
    /// </summary>
    private static List<List<long>> GeneratePotentialPresses(List<long> buttons)
    {
        var potentialPresses = new List<List<long>>();

        for (var i = 0; i < (1 << buttons.Count); i++)
        {
            var presses = new List<long>();
            for (var j = 0; j < buttons.Count; j++)
            {
                if ((i & (1 << j)) != 0)
                {
                    presses.Add(buttons[j]);
                }
            }
            potentialPresses.Add(presses);
        }
        potentialPresses = potentialPresses.OrderBy(x => x.Count).ToList();
        return potentialPresses;
    }

    private class Machine(string[] lines)
    {
        public List<Light> Lights { get; } = [.. lines.Select(line => new Light(line))];
    }

    private class Light
    {
        public long DesiredState { get; }
        public List<long> Buttons { get; } = [];

        public Light(string line)
        {
            var state = line.Split(']')[0].Trim('[').Replace('.', '0').Replace('#', '1');
            var stateWithTrim = state.TrimStart('0');
            DesiredState = Convert.ToInt64(stateWithTrim, 2);

            var buttonsWithVoltage = line.Split(']')[1].Trim().Split(' ');
            var buttons = buttonsWithVoltage.Take(buttonsWithVoltage.Length - 1);
            foreach (var button in buttons)
            {
                var initial = state.Replace('1', '0');
                var ones = button.Split(',').Select(x => x.Trim('(').Trim(')'));
                foreach (var one in ones)
                {
                    var newInitial = initial.ToCharArray();
                    newInitial[int.Parse(one)] = '1';
                    initial = new string(newInitial);
                }
                Buttons.Add(Convert.ToInt64(initial.TrimStart('0'), 2));
            }
        }
    }
}
