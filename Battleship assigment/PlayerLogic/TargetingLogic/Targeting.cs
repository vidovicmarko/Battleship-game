using Battleship_assigment.Model;

namespace Battleship_assigment.PlayerLogic.TargetingLogic;

public sealed partial class Targeting 
{
    private static readonly Random random = new();

    private readonly int boardSize;
    private readonly HashSet<(int row, int col)> firedShots;
    private readonly List<int> remainingShipSizes;

    private (int row, int col)? firstHitInCurrentPhase;
    private (int row, int col)? latestHit;
    private Direction? activeDirection;

    private List<Direction> availableDirections = new();
    private readonly List<Direction> blockedDirections = new();

    private readonly Queue<(int row, int col)> queuedTargets = new();
    private readonly HashSet<(int row, int col)> enqueuedTargets = new();

    public Targeting(int boardSize, HashSet<(int row, int col)> firedShots, List<int> remainingShipSizes)
    {
        this.boardSize = boardSize;
        this.firedShots = firedShots;
        this.remainingShipSizes = remainingShipSizes;
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
}

