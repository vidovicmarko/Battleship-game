namespace Battleship_assigment.Model.Placement
{

    public sealed class ShipPlacer
    {
        private readonly Random random;

        public ShipPlacer(int? target = null)
        {
            random = target.HasValue ? new Random(target.Value) : new Random();
        }

        public void PlaceFleet(Board board, IEnumerable<Ship> ships)
        {
            foreach (var ship in ships)
            {
                PlaceShip(board, ship);
                board.Ships.Add(ship);
            }
        }

        private void PlaceShip(Board board, Ship ship)
        {
            int shipSize = ship.Size;
            int boardSize = board.Size;
            int attempts = 0;

            while (true)
            {
                if (attempts++ >= 1000)
                    throw new Exception("Can't place the ship");

                int startRow = random.Next(0, boardSize);
                int startCol = random.Next(0, boardSize);

                if (board.GetCell(startRow, startCol) != 'E') 
                    continue;

                int directionIndex = random.Next(0, 4);

                for (int tries = 0; tries < 4; tries++)
                {
                    if (CanPlaceShip(board, startRow, startCol, directionIndex, shipSize))
                    {
                        Place(board, startRow, startCol, directionIndex, shipSize, ship);
                        return;
                    }
                    directionIndex = (directionIndex + 1) % 4;
                }
            }
        }

        private void Place(Board board, int startRow, int startCol, int direction, int shipSize, Ship ship)
        {
            var (deltaRow, deltaCol) = GetDirectionDelta(direction);
            char symbol = Board.ShipSymbol(ship);

            for (int i = 0; i < shipSize; i++)
            {
                board.SetCell(startRow, startCol, symbol);
                ship.Coordinates.Add((startRow, startCol));
                startRow += deltaRow;
                startCol += deltaCol;
            }
        }

        private bool CanPlaceShip(Board board, int startRow, int startCol, int direction, int shipSize)
        {
            int boardSize = board.Size;
            var (deltaRow, deltaCol) = GetDirectionDelta(direction);

            int endRow = startRow + deltaRow * (shipSize - 1);
            int endCol = startCol + deltaCol * (shipSize - 1);

            if (IsOutOfBounds(boardSize, endRow, endCol))
                return false;

            for (int i = 0; i < shipSize; i++)
            {
                if (board.GetCell(startRow, startCol) != 'E') 
                    return false;
                
                startRow += deltaRow;
                startCol += deltaCol;
            }
            return true;
        }

        private static (int deltaRow, int deltaCol) GetDirectionDelta(int direction)
        {
            return DirectionHelper.GetOffset((Direction)direction);
        }
        
        private static bool IsOutOfBounds(int boardSize, int row, int col)
        {
            return (row < 0 || row >= boardSize || col < 0 || col >= boardSize);
        }
    }
}
