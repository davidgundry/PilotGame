using UnityEngine;
using System.Collections;
using level.data;
using level;
using player;
using menu;

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
    public LevelFailedMenuBehaviour levelFailedMenu;
    public LevelIntroMenuBehaviour levelIntroMenu;
    public PauseMenuBehaviour pauseMenu;
    private LevelBehaviour levelBehaviour;

    private LevelSessionState levelSessionState;

	void Awake() {
        PlayerLevelData = new PlayerLevelData();
	    CreateLevel(LoadLevel("levels/islands"));   
	}

    void Start()
    {
        StartCoroutine(ShowIntroMenu());
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
        levelBehaviour.Create(this, levelData);

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
        PlayerLevelData.Distance = levelBehaviour.PlayerDistance();
    }

    private IEnumerator ShowIntroMenu()
    {
        levelSessionState = LevelSessionState.Intro;
        levelIntroMenu.Show();
        FreezePlay(true);
        yield return new WaitForSeconds(1f);
        levelIntroMenu.Hide();
        FreezePlay(false);
        levelSessionState = LevelSessionState.Playing;
    }

    private IEnumerator ShowEndMenu(PlayerLevelData PlayerLevelData)
    {
        yield return new WaitForSeconds(2);
        levelEndMenu.Create(PlayerLevelData);
        yield return new WaitForSeconds(2);
        FreezePlay(true);
    }

    private IEnumerator ShowFailedMenu(PlayerLevelData PlayerLevelData)
    {
        yield return new WaitForSeconds(2);
        levelFailedMenu.Create(PlayerLevelData);
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
        {
            levelSessionState = LevelSessionState.Paused;
            levelBehaviour.FreezePlay(true);
            pauseMenu.Show();
                
        }
        else if (levelSessionState == LevelSessionState.Paused)
        {
            levelSessionState = LevelSessionState.Playing;
            levelBehaviour.FreezePlay(false);
            pauseMenu.Hide();
        }
    }

}
