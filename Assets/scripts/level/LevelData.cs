namespace level
{
    public class LevelData
    {
        string name;
        float length;
        float height;

        readonly GeomData[] geomData;
        readonly SpriteData[] spriteData;
        readonly EnvironmentData environmentData;

        LevelData(LevelFileLoader levelFileLoader)
        {
            this.geomData = levelFileLoader.GetGeomData();
            this.spriteData = levelFileLoader.GetSpriteData();
            this.environmentData = levelFileLoader.GetEnvironmentData();
        }
    }

}