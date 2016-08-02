using UnityEngine;

namespace player.data
{
    public class PlayerLevelRecord
    {
        public readonly float time;
        public readonly StarScore starScore;
        public readonly int coins;
        public readonly int pickups;

        public PlayerLevelRecord(float time, StarScore starScore, int coins, int pickups)
        {
            this.time = time;
            this.starScore = starScore;
            this.coins = coins;
            this.pickups = pickups;
        }

        public PlayerLevelRecord Update(PlayerLevelRecord newRecord)
        {
            float time = Mathf.Min(newRecord.time, this.time);
            int coins = Mathf.Max(newRecord.coins, this.coins);
            int pickups = Mathf.Max(newRecord.pickups, this.pickups);

            StarScore starScore = this.starScore;
            if (newRecord.starScore > starScore)
                starScore = newRecord.starScore;

            return new PlayerLevelRecord(time, starScore, coins, pickups);
        }
    }
}