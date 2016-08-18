using UnityEngine;
using player.data;
using level;
using level.data;

public class GameController : MonoBehaviour
{

    private static readonly string[] levelFileNames = new string[] {"first-course","tutorial-gap", "tutorial-mountain", "tutorial-hoop", "first-course", "second-course", "plane-sailing", "cave-route", "flying-plains" }; 

    public int CurrentLevelID { get; set; }
    private LevelListData[] levelList;
    public LevelListData[] LevelList { get { return levelList; } }
    public LevelListData CurrentLevel { get { return levelList[CurrentLevelID]; } }

    public int SelectedLevelID { get; set; }
    public LevelListData SelectedLevel { get { return levelList[SelectedLevelID]; } }

    public bool? UsingMicrophone { get; set; }

    public PlayerGameProgress PlayerGameProgress;

    void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        DontDestroyOnLoad(this);
        levelList = new LevelListData[levelFileNames.Length];
        int i = 0;
        foreach (string filename in levelFileNames)
        {
            levelList[i] = new LevelListData(filename, new LevelData(new LevelFileLoader("levels/" + filename)));
            levelList[i].Locked = false; //For testing purposes
            i++;
        }
        PlayerGameProgress = new PlayerGameProgress(LevelList);
    }

    void Start()
    {
       
        SelectedLevelID = 0;
        CurrentLevelID = 0;
        CurrentLevel.Locked = false;
        PlayerGameProgress.Update(LevelList);
    }

    public void StartGame()
    {
        CurrentLevelID = 0;
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
        if (CurrentLevelID < LevelList.Length)
        {
            Application.LoadLevel("main");
        }
        else
        {
            CurrentLevelID = 0;
            Application.LoadLevel("complete");
        }
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

        PlayerGameProgress.Update(LevelList);
    }


}
