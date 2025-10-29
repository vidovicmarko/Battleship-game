using Battleship_assigment.Model;

namespace Battleship_assigment.Presentation
{
    public static class BoardRenderer
    {
        private static readonly Dictionary<string, ConsoleColor> ShipColors = new()
        {
            { "Carrier", ConsoleColor.Blue },
            { "Battleship", ConsoleColor.Yellow },
            { "Cruiser", ConsoleColor.Magenta },
            { "Submarine", ConsoleColor.Cyan },
            { "Destroyer", ConsoleColor.Green }
        };
        
        private static readonly Dictionary<char, string> ShipNamesByChar = new()
        {
            { 'C', "Carrier"    },
            { 'B', "Battleship" },
            { 'R', "Cruiser"    },
            { 'S', "Submarine"  },
            { 'D', "Destroyer"  }
        };

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

            foreach (var ship in ShipColors)
            {
                Console.ForegroundColor = ship.Value;
                Console.Write("■");
                Console.ResetColor();
                Console.WriteLine($" = {ship.Key} ({ShipSize(ship.Key)})");
            }
            Console.WriteLine();
        }

        private static int ShipSize(string shipType) => shipType switch
        {
            "Carrier"    => 5,
            "Battleship" => 4,
            "Cruiser"    => 3,
            "Submarine"  => 3,
            "Destroyer"  => 2,
            _            => 0
        };

        private static void PrintColoredCell(char cell)
        {
            switch (cell)
            {
                case 'E':
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(" . ");
                    break;

                case 'M':
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(" o ");
                    break;

                case 'H':
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(" X ");
                    break;

                case 'C': // Carrier
                    Console.ForegroundColor = ShipColors["Carrier"];
                    Console.Write(" ■ ");
                    break;

                case 'B': // Battleship
                    Console.ForegroundColor = ShipColors["Battleship"];
                    Console.Write(" ■ ");
                    break;

                case 'R': // Cruiser
                    Console.ForegroundColor = ShipColors["Cruiser"];
                    Console.Write(" ■ ");
                    break;

                case 'S': // Submarine
                    Console.ForegroundColor = ShipColors["Submarine"];
                    Console.Write(" ■ ");
                    break;

                case 'D': // Destroyer
                    Console.ForegroundColor = ShipColors["Destroyer"];
                    Console.Write(" ■ ");
                    break;

                default:
                    Console.ResetColor();
                    Console.Write(" ? ");
                    break;
            }
            Console.ResetColor();
        }
    }
}
