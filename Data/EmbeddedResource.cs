using System.Reflection;

namespace AdventOfCode2025.Data;

internal static class EmbeddedResource
{
    public static string ReadInput(string fileName)
    {
        using Stream stream = Assembly.GetExecutingAssembly()!.GetManifestResourceStream(
            $"{nameof(AdventOfCode2025)}.{nameof(Data)}.{fileName}")
            ?? throw new Exception("Failed to create stream");
        using StreamReader reader = new(stream);
        return reader.ReadToEnd();
    }
}
