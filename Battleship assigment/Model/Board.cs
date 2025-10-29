using Battleship_assigment.Game;

namespace Battleship_assigment.Model;

public class Board
{
    private readonly int boardSize = GameSettings.BoardSize;
    private readonly char[,] grid;
    private readonly ShotResolver shotResolver;
    public List<Ship> Ships { get; set; } = new();
    public int Size => boardSize;
    
    public Board()
    {
        grid = new char[boardSize, boardSize];
        shotResolver = new ShotResolver(this);
        
        for(int row = 0; row < boardSize; row++)
            for(int col = 0; col < boardSize; col++)
                grid[row, col] = 'E';

    }
   
    public char GetCell(int row, int col)
    {
        return grid[row, col];
    }
    internal void SetCell(int row, int col, char symbol)
    { 
        grid[row, col] = symbol;
    } 
    
    public void PlaceFleet(IEnumerable<Ship> fleet)
    {
        new Placement.ShipPlacer().PlaceFleet(this, fleet);
    }
    
    public bool AllShipsSunk()
    {
        return Ships.All(s => s.IsSunk);
    }
    public string ApplyShot((int row, int col) shot)
    {
        return shotResolver.ApplyShot(shot);
    }
    internal static char ShipSymbol(Ship ship)
    {
        char symbol = ship.Name switch
        {
            "Carrier"    => 'C',
            "Battleship" => 'B',
            "Cruiser"    => 'R',
            "Submarine"  => 'S',
            "Destroyer"  => 'D',
            _            => '?'
        };

        return symbol;
    }
}