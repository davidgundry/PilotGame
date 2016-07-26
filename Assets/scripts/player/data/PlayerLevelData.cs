namespace player.data
{
    public class PlayerLevelData
    {
        public float StartTime { get; set; }
        public float EndTime { get; set; }
        public LevelResult LevelResult { get; set; }
        public StarScore StarScore { get; set; }
        public float Time { get { return EndTime - StartTime - frozenTime; } }
        public int Crashes { get; set; }
        public float Distance { get; set; }
        public float DamageTaken { get; set; }
        public float FuelUsed { get; set; }
        public int Coins { get; set; }
        public int Pickups { get; set; }


        private float frozenTime;
        private float freezeTimeStart;

        public PlayerLevelData()
        {
        }

        public void FreezeTime(bool frozen, float time)
        {
            if (frozen)
                freezeTimeStart = time;
            else
                frozenTime += time - freezeTimeStart;
        }

    }


}