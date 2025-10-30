using Battleship_assigment.Model;

namespace Battleship_assigment.PlayerLogic.TargetingLogic;

public sealed partial class Targeting
{
    private (int row, int col) ExploreFromFirstHit()
        {
            var (row0, col0) = firstHitInCurrentPhase!.Value;
            int minShipSize = GetSmallestRemainingShipSize();

            while (availableDirections.Count > 0)
            {
                var direction = availableDirections[0];
                availableDirections.RemoveAt(0);

                var (deltaRow, deltaCol) = GetDirectionOffset(direction);
                var opposite = GetOppositeDirection(direction);
                var (backRow, backCol) = GetDirectionOffset(opposite);

                int openForward = CountConsecutiveOpenCells(row0, col0, deltaRow, deltaCol);
                int openBackward = CountConsecutiveOpenCells(row0, col0, backRow, backCol);
                if (openForward + 1 + openBackward < minShipSize)
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
            var (reverseDeltaRow, reverseDeltaCol) = GetDirectionOffset(reverse);
            var (originalRow, originalCol) = firstHitInCurrentPhase!.Value;
            int reverseRow = originalRow + reverseDeltaRow;
            int reverseCol = originalCol + reverseDeltaCol;

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

        private void ResetAndUseNextTarget()
        {
            if (queuedTargets.Count > 0)
            {
                var target = queuedTargets.Dequeue();
                enqueuedTargets.Remove(target);

                firstHitInCurrentPhase = target;
                latestHit = null;
                activeDirection = null;
                blockedDirections.Clear();

                availableDirections = GetRandomizedDirections();
                return;
            }

            ResetTargeting();
        }

        private void StartNewPhase((int row, int col) firstHit)
        {
            firstHitInCurrentPhase = firstHit;
            latestHit = firstHit;

            activeDirection = null;
            blockedDirections.Clear();

            availableDirections = GetRandomizedDirections();

            queuedTargets.Clear();
            enqueuedTargets.Clear();
        }

        private void ResetTargeting()
        {
            firstHitInCurrentPhase = null;
            latestHit = null;
            activeDirection = null;
            availableDirections.Clear();
            blockedDirections.Clear();
        }

        private (int row, int col) UseNextTarget()
        {
            var target = queuedTargets.Dequeue();
            enqueuedTargets.Remove(target);

            firstHitInCurrentPhase = target;
            latestHit = null;
            activeDirection = null;
            blockedDirections.Clear();

            availableDirections = GetRandomizedDirections();

            return ExploreFromFirstHit();
        }

        private void EnqueueNextTargetIfNew((int row, int col) shot)
        {
            if (firstHitInCurrentPhase.HasValue && shot == firstHitInCurrentPhase.Value) return;
            if (enqueuedTargets.Add(shot))
                queuedTargets.Enqueue(shot);
        }

        private void MarkDirectionFailed(Direction d)
        {
            if (!blockedDirections.Contains(d))
                blockedDirections.Add(d);
        }
}