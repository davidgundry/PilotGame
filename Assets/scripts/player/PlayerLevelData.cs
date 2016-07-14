namespace player
{
    public class PlayerLevelData
    {
        public float StartTime { get; set; }
        public float EndTime { get; set; }
        public LevelResult LevelResult { get; set; }
        public StarScore StarScore { get; set; }
        public float Time { get { return EndTime - StartTime; } }
        public int Crashes { get; set; }
        public float Distance { get; set; }

        public PlayerLevelData()
        {
        }

    }

    public enum LevelResult
    {
        Complete,
        Crash,
        FellOffBottom
    }

    public struct StarScore
    {
        public readonly string text;
        public readonly int stars;

        private StarScore(string text, int stars)
        {
            this.text = text;
            this.stars = stars;
        }
        public static readonly StarScore[] scores = new StarScore[] { new StarScore("Well Done!", 1),new StarScore("Good Job!", 2),new StarScore("Excellent!", 3)};

    }
}