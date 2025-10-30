using System;

namespace Battleship_assigment.Model
{
    public sealed class ShotOutcomeHandler
    {
        private readonly Player player;

        public ShotOutcomeHandler(Player player)
        {
            this.player = player;
        }
        public void OnShotResult((int row, int col) shot, string result)
        {
            if (IsInvalid(result)) return;

            player.FiredShots.Add(shot);

            if (IsMiss(result))
            {
                player.MissedShots.Add(shot);
                return;
            }

            if (IsHit(result))
            {
                player.Targeting.OnHit(shot);
                return;
            }

            if (IsSunk(result))
            {
                var shipName = GetSunkName(result);
                player.Targeting.OnHit(shot);
                player.RemoveSunkByName(shipName);
                player.Targeting.OnSunk();
                return;
            }
        }
        

        private static bool IsInvalid(string result) 
        {
            return string.Equals(result, "Invalid");
        }

        private static bool IsMiss(string result)
        {
            return result.Equals("Miss");
        }

        private static bool IsHit(string result)
        {
            return result.Equals("Hit");
        }

        private static bool IsSunk(string result)
        {
            return result.StartsWith("Sunk");
        }
        
        private static string GetSunkName(string result)
        {
            var parts = result.Split(':');
            return parts.Length == 2 ? parts[1].Trim() : string.Empty;
        }
    }
}
