
namespace player.data
{
    public struct StarScore
    {
        public readonly string text;
        public readonly int stars;

        private StarScore(string text, int stars)
        {
            this.text = text;
            this.stars = stars;
        }
        public static readonly StarScore[] scores = new StarScore[] { new StarScore("Failed", 0 ), new StarScore("Well Done!", 1), new StarScore("Good Job!", 2), new StarScore("Excellent!", 3) };

        public static bool operator > (StarScore a, StarScore b)
        {
            return (a.stars > b.stars);
        }

        public static bool operator < (StarScore a, StarScore b)
        {
            return (a.stars > b.stars);
        }

    }

    public enum LevelResult
    {
        Complete,
        Crash,
        FellOffBottom,
        Sunk
    }
}