using level.data;
namespace player.data
{
    public class StarScoreCalculator {

        public static StarScore Calculate(LevelData levelData, PlayerLevelData playerLevelData)
        {
            float coinProportion = playerLevelData.Coins / (float) levelData.coinCount;

            if (coinProportion > 0.9)
                return StarScore.scores[2];
            else if (coinProportion > 0.4)
                return StarScore.scores[1];

            return StarScore.scores[0];
        }

    }
}