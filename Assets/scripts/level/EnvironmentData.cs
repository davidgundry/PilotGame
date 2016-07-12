namespace level
{

    public enum LevelPalate
    {
        Day,
        Night
    }

    public class EnvironmentData
    {
        readonly float wind;
        readonly float shadowAngle;
        readonly LevelPalate palate;

        public EnvironmentData(float wind, float shadowAngle, LevelPalate palate)
        {
            this.wind = wind;
            this.shadowAngle = shadowAngle;
            this.palate = palate;
        }
    }
}