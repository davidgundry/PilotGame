﻿using UnityEngine;
using player.data;
using level;
using level.data;

public class GameController : MonoBehaviour
{

    private static readonly string[] levelFileNames = new string[] { "plane-sailing", "flying-plains" }; 

    public int CurrentLevelID { get; set; }
    private LevelListData[] levelList;
    public LevelListData[] LevelList { get { return levelList; } }
    public LevelListData CurrentLevel { get { return levelList[CurrentLevelID]; } }

    public int SelectedLevelID { get; set; }
    public LevelListData SelectedLevel { get { return levelList[SelectedLevelID]; } }

    public PlayerGameProgress PlayerGameProgress;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        levelList = new LevelListData[levelFileNames.Length];
        int i = 0;
        foreach (string filename in levelFileNames)
        {
            levelList[i] = new LevelListData(filename, new LevelData(new LevelFileLoader("levels/"+filename)));
            i++;
        }
        PlayerGameProgress = new PlayerGameProgress(LevelList);

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
            PlayCurrentLevel();
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