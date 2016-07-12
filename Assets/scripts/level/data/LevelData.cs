using level;
namespace level.data
{
    public class LevelData
    {
        public readonly string name;
        public readonly float length;
        public readonly float height;

        public readonly GeomData[] geomData;
        public readonly SpriteData[] spriteData;
        public readonly EnvironmentData environmentData;

        public int GeomCount { get { return geomData.Length; } }
        public int SpriteCount { get { return spriteData.Length; } }

        public LevelData(LevelFileLoader levelFileLoader)
        {
            this.name = levelFileLoader.GetName();
            this.length = levelFileLoader.GetLength();
            this.height = levelFileLoader.GetHeight();
            this.geomData = levelFileLoader.GetGeomData();
            this.spriteData = levelFileLoader.GetSpriteData();
            this.environmentData = levelFileLoader.GetEnvironmentData();
        }
    }

}