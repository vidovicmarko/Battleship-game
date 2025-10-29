namespace Battleship_assigment.Game;
public class TurnRecord
{
    public int TurnNumber { get; set; }
    public string Attacker { get; set; }
    public string Coordinate { get; set; }
    public string Result { get; set; }
}

public class GameRecord
{
    public string FirstPlayer { get; set; }
    public string SecondPlayer { get; set; }
    public string Winner { get; set; }
    public int TotalTurns { get; set; }
    
    public char[,] FinalFirstBoard { get; set; }
    public char[,] FinalSecondBoard { get; set; }


    public List<TurnRecord> Turns { get; set; } = new List<TurnRecord>();
}