namespace Battleship_assigment.Model;

public class FleetSetup
{
    public static List<Ship> CreateStandardFleet() => new()
    {
        new Ship("Carrier",     5),
        new Ship("Battleship",  4),
        new Ship("Cruiser",     3),
        new Ship("Submarine",   3),
        new Ship("Destroyer",   2),
    };
}