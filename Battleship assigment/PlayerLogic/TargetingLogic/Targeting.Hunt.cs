namespace Battleship_assigment.PlayerLogic.TargetingLogic;

public sealed partial class Targeting
{
    private (int row, int col) HuntForNewShip()
    {
        int minShipSize = GetSmallestRemainingShipSize();
        int maxRandomTries = 1000;

        var shot = TryRandomAlternateCells(maxRandomTries, minShipSize, even: true);
        if (shot != null) return shot.Value;

        shot = TryScanAlternateCells(minShipSize, even: true);
        if (shot != null) return shot.Value;

        shot = TryRandomAlternateCells(maxRandomTries, minShipSize, even: false);
        if (shot != null) return shot.Value;

        shot = TryScanAlternateCells(minShipSize, even: false);
        if (shot != null) return shot.Value;

        shot = TryScanAllCells();
        if (shot != null) return shot.Value;

        return (0, 0);
    }

    private (int row, int col)? TryRandomAlternateCells(int tries, int minShipSize, bool even)
    {
        for (int i = 0; i < tries; i++)
        {
            int row = random.Next(0, boardSize);
            int col = random.Next(0, boardSize);

            if (minShipSize >= 2 && !IsAlternateCell(row, col, even))
                continue;

            if (!firedShots.Contains((row, col)) && CanMinShipFitAt(row, col, minShipSize))
                return (row, col);
        }
        return null;
    }

    private (int row, int col)? TryScanAlternateCells(int minShipSize, bool even)
    {
        var candidates = new List<(int, int)>();
        for (int row = 0; row < boardSize; row++)
        {
            for (int col = 0; col < boardSize; col++)
            {
                if (minShipSize >= 2 && !IsAlternateCell(row, col, even))
                    continue;

                if (!firedShots.Contains((row, col)) && CanMinShipFitAt(row, col, minShipSize))
                    candidates.Add((row, col));
            }
        }

        if (candidates.Count > 0)
            return candidates[random.Next(candidates.Count)];

        return null;
    }

    private (int row, int col)? TryScanAllCells()
    {
        var allCells = new List<(int, int)>();
        for (int row = 0; row < boardSize; row++)
        {
            for (int col = 0; col < boardSize; col++)
            {
                if (!firedShots.Contains((row, col)))
                    allCells.Add((row, col));
            }
        }

        if (allCells.Count > 0)
            return allCells[random.Next(allCells.Count)];

        return null;
    }

    private bool IsAlternateCell(int row, int col, bool even)
    {
        return even ? (row + col) % 2 == 0 : (row + col) % 2 != 0;
    }
}