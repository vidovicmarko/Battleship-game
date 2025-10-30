namespace Battleship_assigment.Model;

public enum Direction
{
    Up = 0,
    Right = 1,
    Down = 2,
    Left = 3
}

public class DirectionHelper
{
    private static readonly (int deltaRow, int deltaCol)[] Deltas =
    {
        (-1, 0), // Up
        (0, 1),  // Right
        (1, 0),  // Down
        (0, -1)  // Left
    };
    
    private static readonly Direction[] Opposites =
    {
        Direction.Down,
        Direction.Left,
        Direction.Up,
        Direction.Right 
    };
    
    public static (int deltaRow, int deltaCol) GetOffset(Direction direction)
    {
        int i = (int)direction;
        if (i < 0 || i >= Deltas.Length)
            throw new Exception(nameof(direction));
        return Deltas[i];
    }

    public static Direction GetOpposite(Direction direction)
    {
        int i = (int)direction;
        if (i < 0 || i >= Opposites.Length)
            throw new Exception(nameof(direction));
        return Opposites[i];
    }
    
}