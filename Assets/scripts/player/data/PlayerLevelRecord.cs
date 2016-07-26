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
    }
}