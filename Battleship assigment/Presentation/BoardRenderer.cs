using Battleship_assigment.Game;
using Battleship_assigment.Model;

namespace Battleship_assigment.Presentation
{
    public static class BoardRenderer
    {
        public static void Render(Board board)
        {
            int size = board.Size;

            Console.WriteLine();
            PrintHeader(size);
            PrintBorder(size);

            for (int r = 0; r < size; r++)
            {
                Console.Write($"{r + 1,2} |");
                for (int c = 0; c < size; c++)
                {
                    var cell = board.GetCell(r, c);
                    PrintColoredCell(cell);
                }
                Console.ResetColor();
                Console.WriteLine("|");
            }

            PrintBorder(size);
            PrintLegend();
        }

        private static void PrintHeader(int size)
        {
            Console.Write("     ");
            for (int c = 0; c < size; c++)
                Console.Write($" { (char)('A' + c) } ");
            Console.WriteLine();
        }

        private static void PrintBorder(int size)
        {
            Console.WriteLine("   +" + new string('-', size * 3 + 1));
        }

        private static void PrintLegend()
        {
            Console.WriteLine("\nLegend:");
            Console.ForegroundColor = ConsoleColor.DarkGray; Console.Write("."); Console.ResetColor(); Console.WriteLine(" = Empty");
            Console.ForegroundColor = ConsoleColor.Blue;     Console.Write("o"); Console.ResetColor(); Console.WriteLine(" = Miss");
            Console.ForegroundColor = ConsoleColor.Red;      Console.Write("X"); Console.ResetColor(); Console.WriteLine(" = Hit");

            foreach (var ship in GameSettings.ShipColors)
            {
                Console.ForegroundColor = ship.Value;
                Console.Write("■");
                Console.ResetColor();
                Console.WriteLine($" = {ship.Key} ({GameSettings.ShipSizes[ship.Key]})");
            }
            Console.WriteLine();
        }
        
        private static void PrintColoredCell(char cell)
        {
            if (cell == 'E')
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(" . ");
            }
            else if (cell == 'M')
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(" o ");
            }
            else if (cell == 'H')
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(" X ");
            }
            else if (GameSettings.ShipSymbols.TryGetValue(cell, out string shipName) &&
                     GameSettings.ShipColors.TryGetValue(shipName, out ConsoleColor color))
            {
                Console.ForegroundColor = color;
                Console.Write(" ■ ");
            }
            else
            {
                Console.ResetColor();
                Console.Write(" ? ");
            }

            Console.ResetColor();
        }
    }
}
