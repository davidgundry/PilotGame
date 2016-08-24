using level.data;
namespace player.data
{
    public class StarScoreCalculator {

        private readonly LevelData levelData;
        private readonly PlayerLevelData playerLevelData;

        public StarScoreCalculator(LevelData levelData, PlayerLevelData playerLevelData)
        {
            this.levelData = levelData;
            this.playerLevelData = playerLevelData;
        }

        public StarScore GetScore()
        {
            if ((AllHoops) && (AllCoins) && (InTime))
                return StarScore.scores[3];
            if ((AllHoops) && (AllCoins) && (!InTime))
                return StarScore.scores[2];
            if ((AllHoops) && (!AllCoins) && (!InTime))
                return StarScore.scores[1];

            return StarScore.scores[0];
        }

        private bool InTime
        {
            get
            {
                return playerLevelData.Time < levelData.targetTime;
            }
        }

        private bool AllHoops
        {
            get
            {
                return playerLevelData.Hoops == levelData.hoopCount;
            }
        }

        private bool AllCoins
        {
            get
            {
                return playerLevelData.Coins == levelData.coinCount;
            }
        }
    }
}