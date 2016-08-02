using UnityEngine;
using player.data;

public class GameController : MonoBehaviour
{

    public int CurrentLevelID { get; set; }
    private LevelListData[] levelList = new LevelListData[2] { new LevelListData("Plane Sailing", "plane-sailing", null), new LevelListData("Flying Plains", "flying-plains", null) };
    public LevelListData[] LevelList { get { return levelList; } }
    public LevelListData CurrentLevel { get { return levelList[CurrentLevelID]; } }

    public int SelectedLevelID { get; set; }
    public LevelListData SelectedLevel { get { return levelList[SelectedLevelID]; } }

    public PlayerGameProgress PlayerGameProgress;

    void Awake()
    {
        DontDestroyOnLoad(this);
        PlayerGameProgress = new PlayerGameProgress();

        CurrentLevelID = 0;
        CurrentLevel.Locked = false;
    }

    public void StartGame()
    {
        CurrentLevelID = 0;
        CurrentLevel.Locked = false;
        Application.LoadLevel("load");
    }

    public void PlaySelectedLevel()
    {
        CurrentLevelID = SelectedLevelID;
        Application.LoadLevel("load");
    }

    public void PlayCurrentLevel()
    {
        if (!CurrentLevel.Locked)
         Application.LoadLevel("load");
    }

    public void MoveOnToNextLevel()
    {
        CurrentLevelID++;
        PlayCurrentLevel();
    }

    public void LevelComplete(int levelID, PlayerLevelRecord playerLevelRecord)
    {
        UpdateLevelRecord(levelID, playerLevelRecord);
        UnlockLevel(levelID + 1);
    }

    private void UnlockLevel(int levelID, bool unlocked = true)
    {
        if (levelID < LevelList.Length)
            levelList[levelID].Locked = !unlocked;
    }

    private void UpdateLevelRecord(int levelID, PlayerLevelRecord playerLevelRecord)
    {
        if (levelList[levelID].PlayerLevelRecord != null)
            levelList[levelID].PlayerLevelRecord = levelList[levelID].PlayerLevelRecord.Update(playerLevelRecord);
        else
            levelList[levelID].PlayerLevelRecord = playerLevelRecord;
    }


}
