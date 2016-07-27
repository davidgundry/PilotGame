namespace level.data
{

    public enum LevelPalate
    {
        Day,
        Night
    }

    public class EnvironmentData
    {
        public readonly float wind;
        public readonly float shadowAngle;
        public readonly LevelPalate palate;
        public readonly int cloudCount;

        public EnvironmentData(float wind, float shadowAngle, LevelPalate palate, int cloudCount)
        {
            this.wind = wind;
            this.shadowAngle = shadowAngle;
            this.palate = palate;
            this.cloudCount = cloudCount;
        }
    }
}