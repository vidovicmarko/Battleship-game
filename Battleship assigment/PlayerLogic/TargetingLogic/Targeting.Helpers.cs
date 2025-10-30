using Battleship_assigment.Model;

namespace Battleship_assigment.PlayerLogic.TargetingLogic;

public sealed partial class Targeting
{
    private static (int deltaRow, int deltaCol) GetDirectionOffset(Direction direction)
    {
        return DirectionHelper.GetOffset(direction);
    }

    private static Direction GetOppositeDirection(Direction direction)
    {
        return DirectionHelper.GetOpposite(direction);
    }

    private bool IsInBounds(int row, int col)
    {
        return row >= 0 && row < boardSize && col >= 0 && col < boardSize;
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

    private int GetSmallestRemainingShipSize()
    {
        return remainingShipSizes.Count > 0 ? remainingShipSizes.Min() : 1;
    }

    private List<Direction> GetRandomizedDirections()
    {
        return Enum.GetValues(typeof(Direction))
            .Cast<Direction>()
            .OrderBy(_ => random.Next())
            .ToList();
    }
}