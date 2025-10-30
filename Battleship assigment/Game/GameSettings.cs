namespace Battleship_assigment.Game;

public static class GameSettings
{
    public const int BoardSize = 10;
    
    public static readonly IReadOnlyDictionary<string, int> ShipSizes = new Dictionary<string, int>
    {
        ["Carrier"] = 5,
        ["Battleship"] = 4,
        ["Cruiser"] = 3,
        ["Submarine"] = 3,
        ["Destroyer"] = 2
    };

    public static readonly IReadOnlyDictionary<string, ConsoleColor> ShipColors = new Dictionary<string, ConsoleColor>
    {
        ["Carrier"] = ConsoleColor.Blue,
        ["Battleship"] = ConsoleColor.Yellow,
        ["Cruiser"] = ConsoleColor.Magenta,
        ["Submarine"] = ConsoleColor.Cyan,
        ["Destroyer"] = ConsoleColor.Green
    };

    public static readonly IReadOnlyDictionary<char, string> ShipSymbols = new Dictionary<char, string>
    {
        ['C'] = "Carrier",
        ['B'] = "Battleship",
        ['R'] = "Cruiser",
        ['S'] = "Submarine",
        ['D'] = "Destroyer"
    };
}