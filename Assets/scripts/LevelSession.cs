using UnityEngine;
using System.Collections;
using level.data;
using level;
using player;
using menu;

public enum LevelSessionState
{
    Playing,
    Paused,
    End
}

public class LevelSession : MonoBehaviour {

    private PlayerLevelData PlayerLevelData { get; set; }
    public LevelEndMenuBehaviour levelEndMenu;
    public PauseMenuBehaviour pauseMenu;
    private LevelBehaviour levelBehaviour;

    private LevelSessionState levelSessionState;

	void Awake () {
        PlayerLevelData = new PlayerLevelData();
	    CreateLevel(LoadLevel("levels/test"));
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
        //LevelEnd();
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
            StartCoroutine(ShowEndMenu(PlayerLevelData));
        }
    }

    public void PlayerCrashed()
    {
        if (levelSessionState != LevelSessionState.End)
        {
            UpdatePlayerLevelDataAtEnd();
            PlayerLevelData.LevelResult = LevelResult.Crash;
            levelSessionState = LevelSessionState.End;
            StartCoroutine(ShowEndMenu(PlayerLevelData));
        }
    }

    private void UpdatePlayerLevelDataAtEnd()
    {
        PlayerLevelData.EndTime = Time.time;
        PlayerLevelData.Distance = levelBehaviour.PlayerDistance();
    }

    private IEnumerator ShowEndMenu(PlayerLevelData PlayerLevelData)
    {
        yield return new WaitForSeconds(2);
        levelEndMenu.Create(PlayerLevelData);
        yield return new WaitForSeconds(2);
        FreezePlay(true);
    }

    private void FreezePlay(bool frozen)
    {
        levelBehaviour.FreezePlay(frozen);
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
