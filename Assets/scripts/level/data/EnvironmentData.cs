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

        public EnvironmentData(float wind, float shadowAngle, LevelPalate palate)
        {
            this.wind = wind;
            this.shadowAngle = shadowAngle;
            this.palate = palate;
        }
    }
}