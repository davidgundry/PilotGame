using UnityEngine;
using System.Collections;
using level.data;
using level;
using player;
using player.data;
using menu;
using menu.inlevel;
using hud;

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

    private GameController gameController;
    private PlayerLevelData playerLevelData;
    private LevelBehaviour levelBehaviour;
    private LevelData levelData;
    private LevelSessionState levelSessionState;

    private TimerBehaviour timer;

    public LevelListData CurrentLevelListData { get { return gameController.CurrentLevel; } }
    public StarScore CurrentLevelStarScore { get { if (gameController.CurrentLevel.PlayerLevelRecord != null) return gameController.CurrentLevel.PlayerLevelRecord.starScore; else return StarScore.scores[0]; } }



	void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        timer = GameObject.FindObjectOfType<TimerBehaviour>();
        playerLevelData = new PlayerLevelData();
        inGameMenu.LevelSession = this;
	}

    void Start()
    {
        gameController = GameObject.FindObjectOfType<GameController>();
        if (gameController == null)
        {
            Debug.LogError("No GameController found. This is probably because you are running the main scene without loading from the menu. Trying to instantiate a new Game Controller");
            gameController = GameObject.Instantiate<GameController>(Resources.Load<GameController>("prefabs/gameController"));
        }

        levelData = LoadLevel("levels/" + gameController.CurrentLevel.filename);
        CreateLevel(levelData);
        StartCoroutine(ShowIntroMenu(levelData));
        //CrossedFinishLine(); // For testing purposes
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
            timer.ClockRunning = false;
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
            timer.ClockRunning = false;
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
            timer.ClockRunning = false;
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
        gameController.MoveOnToNextLevel();
    }

    public void Menu()
    {
        Application.LoadLevel("level-menu");
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
        gameController.LevelComplete(gameController.CurrentLevelID, record);
    }

    private void SetPlayerEndTime()
    {
        playerLevelData.EndTime = Time.time;
        playerLevelData.Time = timer.Time;
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
        timer.ClockRunning = !frozen;
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
