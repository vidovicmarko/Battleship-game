using Battleship_assigment.Game;
using Battleship_assigment.Model;
using Battleship_assigment.PlayerLogic.TargetingLogic;

namespace Battleship_assigment.PlayerLogic
{
    public class Player
    {
        public string Name { get; }
        public Board Board { get; }

        private readonly int boardSize = GameSettings.BoardSize;

        private readonly HashSet<(int row, int col)> firedShots = new();
        private readonly HashSet<(int row, int col)> missedShots = new();
        private readonly List<int> remainingShipSizes = new() { 5, 4, 3, 3, 2 };
        private readonly Targeting targeting;
        private readonly ShotOutcomeHandler outcomeHandler;
        
        public Player(string name, Board board)
        {
            Name  = name;
            Board = board;
            
            targeting = new Targeting(boardSize, firedShots, remainingShipSizes);
            outcomeHandler = new ShotOutcomeHandler(this);
        }
        
        public (int row, int col) ChooseShot()
        {
            return targeting.ChooseShot();
        }
        
        public void OnShotResult((int row, int col) coordinate, string result)
        {
            outcomeHandler.OnShotResult(coordinate, result);
        }
        
        internal HashSet<(int row, int col)> FiredShots => firedShots;
        internal HashSet<(int row, int col)> MissedShots => missedShots;
        internal Targeting Targeting => targeting;
        
        internal void RemoveSunkByName(string shipName)
        {
           if (GameSettings.ShipSizes.ContainsKey(shipName))
           {
               int size = GameSettings.ShipSizes[shipName];
               remainingShipSizes.Remove(size);
           }
        }
        
        
    }
}
