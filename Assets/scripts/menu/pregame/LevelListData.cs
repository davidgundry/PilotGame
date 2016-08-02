using player.data;
using level.data;

public class LevelListData {

    public readonly string name;
    public readonly string filename;
    public readonly int coins;
    public readonly int pickups;

    public bool Locked { get; set; }
    public PlayerLevelRecord PlayerLevelRecord { get; set; }

    public LevelListData(string filename, LevelData levelData)
    {
        this.name = levelData.name;
        this.filename = filename;
        PlayerLevelRecord = null;
        this.Locked = true;
        this.coins = levelData.coinCount;
        this.pickups = levelData.pickupCount;
    }

    public bool Complete
    {
        get
        {
            if (PlayerLevelRecord == null)
                return false;
            return PlayerLevelRecord.starScore > StarScore.scores[0];
        }
    }

}
