namespace player.data
{
    public class PlayerGameProgress
    {

        public int Stars { get; private set; }
        public int Levels { get; private set; }
        public int Pickups { get; private set; }
        public int Coins { get; private set; }

        public int TotalStars { get; private set; }
        public int TotalCoins { get; private set; }
        public int TotalPickups { get; private set; }
        public int TotalLevels { get; private set; }

        public int CompletePercentage { get; private set; }

        public PlayerGameProgress(LevelListData[] levelList)
        {
            int totalStars = 0;
            int totalCoins = 0;
            int totalPickups = 0;
            int totalLevels = 0;
            foreach (LevelListData lld in levelList)
            {
                totalLevels++;
                totalStars += 3;
                totalPickups += lld.pickups;
                totalCoins += lld.coins;
            }
            TotalCoins = totalCoins;
            TotalPickups = totalPickups;
            TotalStars = totalStars;
            TotalLevels = totalLevels;
        }

        public void Update(LevelListData[] levelList)
        {
            int completeLevels = 0;
            int stars = 0;
            int pickups = 0;
            int coins = 0;
            foreach (LevelListData lld in levelList)
            {
                if (lld.Complete)
                {
                    completeLevels++;
                    stars += lld.PlayerLevelRecord.starScore.stars;
                    pickups += lld.PlayerLevelRecord.pickups;
                    coins += lld.PlayerLevelRecord.coins;
                }
            }
            Levels = completeLevels;
            Stars = stars;
            Pickups = pickups;
            Coins = coins;

            CompletePercentage = (int) ((float) (Stars + Coins + Pickups + Levels) / (TotalStars + TotalCoins + TotalPickups + TotalLevels) * 100);
        }

    }
}