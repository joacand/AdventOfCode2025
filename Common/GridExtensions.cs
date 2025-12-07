namespace AdventOfCode2025.Common;

internal static class GridExtensions
{
    extension(char[,] grid)
    {
        public char V(Position position) => grid[position.Row, position.Column];
        public char? Up(Position position) => position.Row - 1 >= 0 ? grid[position.Row - 1, position.Column] : null;
        public char? Down(Position position) => position.Row + 1 < grid.GetLength(0) ? grid[position.Row + 1, position.Column] : null;
        public char? Left(Position position) => position.Column - 1 >= 0 ? grid[position.Row, position.Column - 1] : null;
        public char? Right(Position position) => position.Column + 1 < grid.GetLength(1) ? grid[position.Row, position.Column + 1] : null;

        public void Print()
        {
            for (var r = 0; r < grid.GetLength(0); r++)
            {
                for (var c = 0; c < grid.GetLength(1); c++)
                {
                    Console.Write(grid[r, c]);
                }
                Console.WriteLine();
            }
        }
    }
}
