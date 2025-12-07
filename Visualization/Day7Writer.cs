using System.Text;

namespace AdventOfCode2025.Visualization;

internal class Day7Writer(TextWriter original) : TextWriter
{
    public override Encoding Encoding => Encoding.UTF8;

    public override void Write(char value)
    {
        switch (value)
        {
            case 'S':
                Console.ForegroundColor = ConsoleColor.Yellow;
                original.Write("██");
                break;
            case '.':
                original.Write("  ");
                break;
            case '|':
                Console.ForegroundColor = ConsoleColor.Green;
                original.Write("██");
                break;
            case '^':
                Console.ForegroundColor = ConsoleColor.Red;
                original.Write("██");
                break;
            default:
                original.Write(value);
                break;
        }
    }

    public override void WriteLine(string? value)
    {
        if (value == null) { return; }

        if (value.Equals("CaretReset"))
        {
            Console.SetCursorPosition(0, 0);
            return;
        }
        foreach (var c in value)
        {
            Write(c);
        }
    }
}
