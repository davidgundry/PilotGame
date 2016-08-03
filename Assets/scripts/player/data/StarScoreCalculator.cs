using level.data;
namespace player.data
{
    public class StarScoreCalculator {

        public static StarScore Calculate(LevelData levelData, PlayerLevelData playerLevelData)
        {
            if (levelData.coinCount == 0)
                return StarScore.scores[3];

            float coinProportion = playerLevelData.Coins / (float) levelData.coinCount;

            if (coinProportion > 0.9)
                return StarScore.scores[3];
            else if (coinProportion > 0.4)
                return StarScore.scores[2];

            return StarScore.scores[1];
        }

    }
}