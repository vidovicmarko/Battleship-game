namespace Battleship_assigment.Model
{

    public sealed class Targeting
    {
        private static readonly Random random = new();
        
        private readonly int boardSize;
        private readonly HashSet<(int row, int col)> firedShots;
        private readonly List<int> remainingShipSizes;

        private (int row, int col)? firstHitInCurrentPhase;
        private (int row, int col)? latestHit;
        private Direction? activeDirection;
        
        private List<Direction> availableDirections  = new();
        private readonly List<Direction> blockedDirections  = new();

        private readonly Queue<(int row, int col)> queuedTargets = new();
        private readonly HashSet<(int row, int col)> enqueuedTargets = new();

        public Targeting(int boardSize, HashSet<(int row, int col)> firedShots, List<int> remainingShipSizes)
        {
            this.boardSize = boardSize;
            this.firedShots = firedShots;
            this.remainingShipSizes = remainingShipSizes;
        }
        
        private int GetSmallestRemainingShipSize() 
        {
            return remainingShipSizes.Count > 0 ? remainingShipSizes.Min() : 1;
        }
        
        public (int row, int col) ChooseShot()
        {
            if (firstHitInCurrentPhase is null)
                return HuntForNewShip();

            if (activeDirection is null)
                return ExploreFromFirstHit();

            return FollowLine();
        }

        public void OnHit((int row, int col) coordinate)
        {
            if (firstHitInCurrentPhase is null)
            {
                StartNewPhase(coordinate);
                return;
            }
            
            latestHit = coordinate;

            if (activeDirection is null && firstHitInCurrentPhase is not null)
                TryLockDirectionFrom(coordinate);
        }

        public void OnSunk()
        {
            ResetAndUseNextTarget();
        }
        
        private void StartNewPhase((int row, int col) firstHit)
        {
            firstHitInCurrentPhase = firstHit;
            latestHit = firstHit;

            activeDirection = null;
            blockedDirections .Clear();

            availableDirections  = Enum.GetValues(typeof(Direction))
                .Cast<Direction>()
                .OrderBy(_ => random.Next())
                .ToList();

            queuedTargets.Clear();
            enqueuedTargets.Clear();
        }
        
        private (int row, int col) HuntForNewShip()
        {
            int minShipSize = GetSmallestRemainingShipSize();
            int maxRandomTries = 1000;

            for (int i = 0; i < maxRandomTries; i++)
            {
                int row = random.Next(0, boardSize);
                int col = random.Next(0, boardSize);
                
                if (minShipSize >= 2 && (row + col) % 2 != 0)
                    continue;

                if (!firedShots.Contains((row, col)) && CanMinShipFitAt(row, col, minShipSize))
                    return (row, col);
            }

            for (int row = 0; row < boardSize; row++)
            {
                for (int col = 0; col < boardSize; col++)
                {
                    if (minShipSize >= 2 && (row + col) % 2 != 0) continue;
                    if (!firedShots.Contains((row, col)) && CanMinShipFitAt(row, col, minShipSize))
                        return (row, col);
                }
            }

            return (0, 0);
        }

        private (int row, int col) ExploreFromFirstHit()
        {
            var (row0, col0) = firstHitInCurrentPhase!.Value;
            int minShipSize = GetSmallestRemainingShipSize();
            ;

            while (availableDirections.Count > 0)
            {
                var direction = availableDirections [0];
                availableDirections.RemoveAt(0);

                var (deltaRow, deltaCol) = GetDirectionOffset(direction);

                int openForward = CountConsecutiveOpenCells(row0, col0, deltaRow, deltaCol);
                if (openForward + 1 < minShipSize)
                    continue;

                int targetRow = row0 + deltaRow;
                int targetCol = col0 + deltaCol;

                if (IsInBounds(targetRow, targetCol) && !firedShots.Contains((targetRow, targetCol)))
                    return (targetRow, targetCol);
            }

            if (queuedTargets.Count > 0)
                return UseNextTarget();

            ResetTargeting();
            return HuntForNewShip();
        }

        private (int row, int col) FollowLine()
        {
            if (activeDirection is null) return ExploreFromFirstHit();

            var direction = activeDirection.Value;
            var next = GetNextStep(latestHit ?? firstHitInCurrentPhase!.Value, direction);
            if (next is not null)
                return next.Value;

            MarkDirectionFailed(direction);

            var reverse = GetOppositeDirection(direction);
            var (reverseDeltaRow, reverselDeltaCol) = GetDirectionOffset(reverse);
            var (originalRow, originalCol) = firstHitInCurrentPhase!.Value;
            int reverseRow = originalRow + reverseDeltaRow; 
            int reverseCol= originalCol + reverselDeltaCol;

            if (IsInBounds(reverseRow, reverseCol) && !firedShots.Contains((reverseRow, reverseCol)))
            {
                activeDirection = reverse;
                return (reverseRow, reverseCol);
            }

            MarkDirectionFailed(reverse);

            if (queuedTargets.Count > 0)
                return UseNextTarget();

            var remainingDirections = Enum.GetValues(typeof(Direction))
                .Cast<Direction>()
                .Where(d => !blockedDirections.Contains(d))
                .ToList();

            if (remainingDirections.Count > 0)
            {
                activeDirection = remainingDirections[random.Next(remainingDirections.Count)];
                return FollowLine();
            }

            ResetTargeting();
            return HuntForNewShip();
        }
        
        private (int row, int col)? GetNextStep((int row, int col) from, Direction direction)
        {
            var (deltaRow, deltaCol) = GetDirectionOffset(direction);

            int newRow = from.row + deltaRow;
            int newCol = from.col + deltaCol;

            if (!IsInBounds(newRow, newCol))
                return null;

            var next = (newRow, newCol);
            if (firedShots.Contains(next))
                return null;

            return next;
        }
        

        private void ResetTargeting()
        {
            firstHitInCurrentPhase = null;
            latestHit = null;
            activeDirection = null;
            availableDirections.Clear();
            blockedDirections.Clear();
        }

        private void ResetAndUseNextTarget()
        {
            if (queuedTargets.Count > 0)
            {
                var target = queuedTargets.Dequeue();
                enqueuedTargets.Remove(target);

                firstHitInCurrentPhase = target;
                latestHit = null;
                activeDirection = null;
                blockedDirections .Clear();

                availableDirections = GetRandomizedDirections();
                return;
            }

            ResetTargeting();
        }

        private (int row, int col) UseNextTarget()
        {
            var target = queuedTargets.Dequeue();
            enqueuedTargets.Remove(target);

            firstHitInCurrentPhase = target;
            latestHit = null;
            activeDirection = null;
            blockedDirections .Clear();

            availableDirections = GetRandomizedDirections();

            return ExploreFromFirstHit();
        }
        
        private List<Direction> GetRandomizedDirections()
        {
            return Enum.GetValues(typeof(Direction))
                .Cast<Direction>()
                .OrderBy(_ => random.Next())
                .ToList();
        }

        private void TryLockDirectionFrom((int row, int col) shot)
        {
            var (originalRow, originalCol) = firstHitInCurrentPhase.Value;
            var (row, col) = shot;

            if (originalRow == row && col != originalCol)
            {
                activeDirection = (col > originalCol) ? Direction.Right : Direction.Left;
                return;
            }

            if (originalCol == col && row != originalRow)
            {
                activeDirection = (row > originalRow) ? Direction.Down : Direction.Up;
                return;
            }

            EnqueueNextTargetIfNew(shot);
        }

        private void EnqueueNextTargetIfNew((int row, int col) shot)
        {
            if (firstHitInCurrentPhase.HasValue && shot == firstHitInCurrentPhase.Value) return;
            if (enqueuedTargets.Add(shot))
                queuedTargets.Enqueue(shot);
        }

        private void MarkDirectionFailed(Direction d)
        {
            if (!blockedDirections .Contains(d))
                blockedDirections .Add(d);
        }

        private bool IsInBounds(int row, int col) =>
            row >= 0 && row < boardSize && col >= 0 && col < boardSize;

        private static (int deltaRow, int deltaCol) GetDirectionOffset(Direction direction)
        {
            return DirectionHelper.GetOffset(direction);
        }

        private static Direction GetOppositeDirection(Direction direction)
        {
            return DirectionHelper.GetOpposite(direction);
        }

        private bool CanMinShipFitAt(int r, int c, int minSize)
        {
            if (!IsInBounds(r, c) || firedShots.Contains((r, c))) return false;

            int up    = CountConsecutiveOpenCells(r, c, -1,  0);
            int down  = CountConsecutiveOpenCells(r, c, +1,  0);
            int left  = CountConsecutiveOpenCells(r, c,  0, -1);
            int right = CountConsecutiveOpenCells(r, c,  0, +1);

            int verticalSpan   = up + 1 + down;
            int horizontalSpan = left + 1 + right;

            return verticalSpan >= minSize || horizontalSpan >= minSize;
        }

        private int CountConsecutiveOpenCells(int row, int col, int dr, int dc)
        {
            int count = 0;
            while (true)
            {
                row += dr; col += dc;
                if (!IsInBounds(row, col)) break;
                if (firedShots.Contains((row, col))) break;
                count++;
            }
            return count;
        }
        
    }
}
