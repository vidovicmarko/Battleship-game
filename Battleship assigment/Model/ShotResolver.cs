namespace Battleship_assigment.Model;

public sealed class ShotResolver
{
    private readonly Board board;

    public ShotResolver(Board board)
    {
        this.board = board;
    }
    
    public string ApplyShot((int row, int col) shot)
    {
        var (row, col) = shot;

        if (IsOutOfBounds(row, col))
            return "Invalid";
        
        if (IsEmptyCell(row, col))
        {
            MarkMiss(row, col);
            return "Miss";
        }

        Ship? hitShip = FindShipAt(row, col);
        if (hitShip == null)
        {
            MarkMiss(row, col);
            return "Miss";
        }

        MarkHit(row, col);
        hitShip.RegisterHit(row, col);

        return hitShip.IsSunk ? $"Sunk: {hitShip.Name}" : "Hit";
    }

    private bool IsOutOfBounds(int row, int col)
    { 
       return row < 0 || row >= board.Size || col < 0 || col >= board.Size;
    }
    
    private bool IsEmptyCell(int row, int col) 
    {
       return board.GetCell(row, col) == 'E';
    }
    private Ship? FindShipAt(int row, int col)
    {
        foreach (var ship in board.Ships)
            if (ship.Occupies(row, col))
                return ship;
        return null;
    }

    private void MarkMiss(int row, int col)
    {
        board.SetCell(row, col, 'M');
    }
    private void MarkHit(int row, int col)
    {
        board.SetCell(row, col, 'H');
    }
}