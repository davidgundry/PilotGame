using player.data;

public class LevelListData {

    public readonly string name;
    public readonly string filename;
    public bool Locked { get; set; }
    public PlayerLevelRecord PlayerLevelRecord { get; set; }

    public LevelListData(string name, string filename, PlayerLevelRecord playerLevelRecord)
    {
        this.name = name;
        this.filename = filename;
        PlayerLevelRecord = playerLevelRecord;
        this.Locked = true;
    }

}
