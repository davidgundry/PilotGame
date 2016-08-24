using UnityEngine;
using level;
namespace level.data
{
    public class LevelData
    {
        public readonly string name;
        public readonly string subtitle;
        public readonly float length;
        public readonly float height;
        public readonly int cloudCount;
        public readonly Vector2 playerStart;
        public readonly int coinCount;
        public readonly int pickupCount;
        public readonly float targetTime;

        public readonly GeomData[] geomData;
        public readonly SpriteData[] spriteData;
        public readonly EnvironmentData environmentData;
        public readonly PickupData[] pickupData;
        public readonly HoopData[] hoopData;

        public int GeomCount { get { return geomData.Length; } }
        public int SpriteCount { get { return spriteData.Length; } }
        public int hoopCount { get { return hoopData.Length; } }

        public LevelData(LevelFileLoader levelFileLoader)
        {
            this.name = levelFileLoader.GetName();
            this.subtitle = levelFileLoader.GetSubtitle();
            this.length = levelFileLoader.GetLength();
            this.height = levelFileLoader.GetHeight();
            this.playerStart = levelFileLoader.GetPlayerStart();
            this.geomData = levelFileLoader.GetGeomData();
            this.spriteData = levelFileLoader.GetSpriteData();
            this.pickupData = levelFileLoader.GetPickupData();
            this.hoopData = levelFileLoader.GetHoopData();
            this.environmentData = levelFileLoader.GetEnvironmentData();
            this.targetTime = levelFileLoader.GetTargetTime();

            this.coinCount = CountCoins(this.pickupData);
            this.pickupCount = CountPickups(this.pickupData);
        }

        private static int CountCoins(PickupData[] pickupData)
        {
            int count = 0;
            foreach (PickupData pickup in pickupData)
            {
                if (pickup.pickupType == PickupType.Coin)
                    count++;
            }
            return count;
        }

        private static int CountPickups(PickupData[] pickupData)
        {
            int count = 0;
            foreach (PickupData pickup in pickupData)
            {
                if ((pickup.pickupType == PickupType.Fuel) || (pickup.pickupType == PickupType.Repair) || (pickup.pickupType == PickupType.Speed))
                    count++;
            }
            return count;
        }
    }

}