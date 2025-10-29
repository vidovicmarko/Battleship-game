namespace Battleship_assigment.Model;
public class Ship
{
    public string Name { get; set; }
    public int Size { get; set; }
    
    public List<(int row, int col)> Coordinates { get; } = new();
    
    private readonly HashSet<(int row, int col)> hits = new();
    public bool IsSunk => hits.Count == Size;
    public Ship(string name, int size)
    {
        Name = name;
        Size = size;
    }
    public bool Occupies(int row, int col)
    {
        return Coordinates.Contains((row, col));
    }

    public void RegisterHit(int row, int col)
    {
        if (Occupies(row, col)) hits.Add((row, col));
    }
    
}