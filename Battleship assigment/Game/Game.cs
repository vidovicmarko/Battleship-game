using Battleship_assigment.Model;
using Battleship_assigment.PlayerLogic;
using Battleship_assigment.Presentation;

namespace Battleship_assigment.Game;

public class Game
{
    private readonly Player firstPlayer;
    private readonly Player secondPlayer;
    private readonly Board firstBoard;
    private readonly Board secondBoard;
    
    public Game()
    {
        firstBoard = new Board();
        secondBoard = new Board();
        
        var fleet1 = FleetSetup.CreateStandardFleet();
        var fleet2 = FleetSetup.CreateStandardFleet();

        firstBoard.PlaceFleet(fleet1);
        secondBoard.PlaceFleet(fleet2);

        firstPlayer = new Player("Player 1", firstBoard);
        secondPlayer = new Player("Player 2", secondBoard);
    }
    
    
    public GameRecord Run()
    {
        GameRecord gameRecord = new GameRecord
        {
            FirstPlayer = firstPlayer.Name,
            SecondPlayer =  secondPlayer.Name
        };
        
        DisplayStartingBoards();
        
        Player attacker = firstPlayer;
        Board defenderBoard = secondBoard;
        int turn = 0;
        
        
        
        while (true)
        {
            turn++;
            string result = ExecuteTurn(turn, attacker, defenderBoard, gameRecord);

            if (defenderBoard.AllShipsSunk())
            {
                gameRecord.Winner = attacker.Name;
                gameRecord.TotalTurns = turn;
                
                AnnouceWinner(attacker, gameRecord);
                break;
            }

            if (attacker == firstPlayer)
            {
                attacker = secondPlayer;
                defenderBoard = firstBoard;
            }
            else
            {
                attacker = firstPlayer;
                defenderBoard = secondBoard;
            }
        }
        return gameRecord;
    }
    
    private void DisplayStartingBoards()
    {
        Console.Clear();
        Console.WriteLine("Game started");
        
        Console.WriteLine($"{firstPlayer.Name}'s Board:");
        BoardRenderer.Render(firstBoard);
        Console.WriteLine();

        Console.WriteLine($"{secondPlayer.Name}'s Board:");
        BoardRenderer.Render(secondBoard);
        Console.WriteLine();
    }
    
    private string ExecuteTurn(int turn, Player attacker, Board defenderBoard, GameRecord gameRecord)
    {
        var (row, col) = attacker.ChooseShot();
        string result = defenderBoard.ApplyShot((row, col));
        attacker.OnShotResult((row, col), result);

        string coordinate = $"{(char)('A' + col)}{row + 1}";
        
        gameRecord.Turns.Add(new TurnRecord
            {
                TurnNumber = turn,
                Attacker = attacker.Name,
                Coordinate = coordinate,
                Result = result
            }
            );

        Console.WriteLine($"Turn {turn}: {attacker.Name} fires at {coordinate} -> {result}");
        Console.WriteLine();
        BoardRenderer.Render(defenderBoard);

        return result;
    }

    private void AnnouceWinner(Player winner, GameRecord gameRecord)
    {
        Console.WriteLine($"\n{winner.Name} wins! All enemy ships sunk.");
        Console.WriteLine($"TotalTurns: {gameRecord.TotalTurns}");
        
        gameRecord.FinalFirstBoard = CopyBoardState(firstBoard);
        gameRecord.FinalSecondBoard = CopyBoardState(secondBoard);
        
        Console.WriteLine("\nFinal state of the boards:");
        Console.WriteLine($"{gameRecord.FirstPlayer}'s final board:");
        BoardRenderer.Render(firstBoard);
        Console.WriteLine();
        Console.WriteLine($"{gameRecord.SecondPlayer}'s final board:");
        BoardRenderer.Render(secondBoard);
    }
    
    private static char[,] CopyBoardState(Board board)
    {
        int size = board.Size;
        char[,] state = new char[size, size];

        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                state[row, col] = board.GetCell(row, col);
            }
        }

        return state;
    }


}