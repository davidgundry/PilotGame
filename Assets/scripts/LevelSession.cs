using UnityEngine;
using System.Collections;
using level.data;
using level;
using player;
using menu;
using menu.inlevel;

public enum LevelSessionState
{
    Intro,
    Playing,
    Paused,
    End
}

public class LevelSession : MonoBehaviour {

    private PlayerLevelData PlayerLevelData { get; set; }
    public LevelEndMenuBehaviour levelEndMenu;
    public InGameMenuBehaviour inGameMenu;
    private LevelBehaviour levelBehaviour;

    private LevelSessionState levelSessionState;

	void Awake()
    {
        PlayerLevelData = new PlayerLevelData();
        inGameMenu.LevelSession = this;
	}

    void Start()
    {
        LevelData levelData = LoadLevel("levels/islands");
        CreateLevel(levelData);  
        StartCoroutine(ShowIntroMenu(levelData));
    }

    void Update()
    {
        if (Input.GetKeyDown("p"))
            Pause();
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
        levelBehaviour.Create(this, levelData, PlayerLevelData);

        PlayerLevelData.StartTime = Time.time;
    }

    public void LevelEnd()
    {
        if (levelSessionState != LevelSessionState.End)
        {
            UpdatePlayerLevelDataAtEnd();
            PlayerLevelData.LevelResult = LevelResult.Complete;
            PlayerLevelData.StarScore = StarScore.scores[0];
            levelSessionState = LevelSessionState.End;
            StartCoroutine(ShowEndMenu(PlayerLevelData));
        }
    }

    public void FallOffBottomOfScreen()
    {
        if (levelSessionState != LevelSessionState.End)
        {
            UpdatePlayerLevelDataAtEnd();
            PlayerLevelData.LevelResult = LevelResult.FellOffBottom;
            levelSessionState = LevelSessionState.End;
            StartCoroutine(ShowFailedMenu(PlayerLevelData));
        }
    }

    public void PlayerCrashed()
    {
        if (levelSessionState != LevelSessionState.End)
        {
            UpdatePlayerLevelDataAtEnd();
            PlayerLevelData.LevelResult = LevelResult.Crash;
            levelSessionState = LevelSessionState.End;
            StartCoroutine(ShowFailedMenu(PlayerLevelData));
        }
    }

    private void UpdatePlayerLevelDataAtEnd()
    {
        PlayerLevelData.EndTime = Time.time;
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
        PlayerLevelData.FreezeTime(frozen, Time.time);
    }

    public void Pause()
    {
        if (levelSessionState == LevelSessionState.Playing)
            EnPause();
        else if (levelSessionState == LevelSessionState.Paused)
            UnPause();
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

    public void Retry()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void Next()
    {
        
    }

    public void Resume()
    {
        UnPause();
    }

    public void Menu()
    {
        
    }

}
