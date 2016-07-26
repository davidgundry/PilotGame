﻿using UnityEngine;
using System.Collections;
using level.data;
using level;
using player;
using player.data;
using menu;
using menu.inlevel;

public class LevelSession : MonoBehaviour {

    private enum LevelSessionState
    {
        Intro,
        Playing,
        Paused,
        End
    }

    public LevelEndMenuBehaviour levelEndMenu;
    public InGameMenuBehaviour inGameMenu;

    private PlayerLevelData playerLevelData;
    private LevelBehaviour levelBehaviour;
    private LevelData levelData;
    private LevelSessionState levelSessionState;

	void Awake()
    {
        playerLevelData = new PlayerLevelData();
        inGameMenu.LevelSession = this;
	}

    void Start()
    {
        levelData = LoadLevel("levels/islands");
        CreateLevel(levelData);  
        StartCoroutine(ShowIntroMenu(levelData));
        CrossedFinishLine(); // For testing purposes
    }

    void Update()
    {
        if (Input.GetKeyDown("p"))
            Pause();
    }

    public void CrossedFinishLine()
    {
        if (levelSessionState != LevelSessionState.End)
        {
            SetPlayerEndTime();
            playerLevelData.LevelResult = LevelResult.Complete;
            playerLevelData.StarScore = StarScoreCalculator.Calculate(levelData, playerLevelData);
            levelSessionState = LevelSessionState.End;
            UpdateLevelRecord();
            StartCoroutine(ShowEndMenu(playerLevelData));
        }
    }

    public void FallOffBottomOfScreen()
    {
        if (levelSessionState != LevelSessionState.End)
        {
            SetPlayerEndTime();
            playerLevelData.LevelResult = LevelResult.FellOffBottom;
            levelSessionState = LevelSessionState.End;
            StartCoroutine(ShowFailedMenu(playerLevelData));
        }
    }

    public void PlayerCrashed()
    {
        if (levelSessionState != LevelSessionState.End)
        {
            SetPlayerEndTime();
            playerLevelData.LevelResult = LevelResult.Crash;
            levelSessionState = LevelSessionState.End;
            StartCoroutine(ShowFailedMenu(playerLevelData));
        }
    }

    public void Pause()
    {
        if (levelSessionState == LevelSessionState.Playing)
            EnPause();
        else if (levelSessionState == LevelSessionState.Paused)
            UnPause();
    }

    public void Retry()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void Resume()
    {
        UnPause();
    }

    public void Next()
    {
        
    }

    public void Menu()
    {
        
    }

    private LevelData LoadLevel(string filePath)
    {
        return new LevelData(new LevelFileLoader(filePath));
    }

    private void CreateLevel(LevelData levelData)
    {
        GameObject g = new GameObject();
        g.AddComponent<LevelBehaviour>();
        levelBehaviour = g.GetComponent<LevelBehaviour>();
        levelBehaviour.Create(this, levelData, playerLevelData);

        playerLevelData.StartTime = Time.time;
    }

    private void UpdateLevelRecord()
    {
        PlayerLevelRecord record = new PlayerLevelRecord(playerLevelData.Time, playerLevelData.StarScore, playerLevelData.Coins, playerLevelData.Pickups);
    }

    private void SetPlayerEndTime()
    {
        playerLevelData.EndTime = Time.time;
    }

    private IEnumerator ShowIntroMenu(LevelData levelData)
    {
        levelSessionState = LevelSessionState.Intro;
        inGameMenu.InGameMenu = new LevelIntroMenu(levelData);
        FreezePlay(true);
        yield return new WaitForSeconds(2f);
        inGameMenu.Destroy();
        FreezePlay(false);
        levelSessionState = LevelSessionState.Playing;
    }

    private IEnumerator ShowEndMenu(PlayerLevelData playerLevelData)
    {
        yield return new WaitForSeconds(2);
        levelEndMenu.Create(playerLevelData);
        yield return new WaitForSeconds(2);
        FreezePlay(true);
    }

    private IEnumerator ShowFailedMenu(PlayerLevelData playerLevelData)
    {
        yield return new WaitForSeconds(2);
        inGameMenu.InGameMenu = new LevelFailedMenu(playerLevelData);
        yield return new WaitForSeconds(2);
        FreezePlay(true);
    }

    private void FreezePlay(bool frozen)
    {
        levelBehaviour.FreezePlay(frozen);
        playerLevelData.FreezeTime(frozen, Time.time);
    }

    private void EnPause()
    {
        levelSessionState = LevelSessionState.Paused;
        levelBehaviour.FreezePlay(true);
        inGameMenu.InGameMenu = new PauseMenu();
    }

    private void UnPause()
    {
        levelSessionState = LevelSessionState.Playing;
        levelBehaviour.FreezePlay(false);
        inGameMenu.Destroy();
    }

}
