using UnityEngine;
using System.Collections;
using level.data;
using player;
using player.data;
using menu;
using menu.inlevel;
using hud;
using experiment;
using experiment.level;
using MicTools;

namespace level
{
    public class LevelSession : MonoBehaviour
    {

        private enum LevelSessionState
        {
            Intro,
            Playing,
            Paused,
            End
        }

        public LevelEndMenuBehaviour levelEndMenu;
        public InGameMenuBehaviour inGameMenu;

        public GameObject HoopPrefab;
        public GameObject PlayerPrefab;

        public ParticleSystem splashParticleSystem;
        public ParticleSystem SplashParticleSystem { get { return splashParticleSystem; } }
        public Material waterLineMaterial;
        public Material WaterLineMaterial { get { return waterLineMaterial; } }

        private GameController gameController;
        private PlayerLevelData playerLevelData;
        private LevelBehaviour levelBehaviour;
        private LevelData levelData;
        private LevelSessionState levelSessionState;

        private TimerBehaviour timer;

        public LevelListData CurrentLevelListData { get { return gameController.CurrentLevel; } }
        public StarScore CurrentLevelStarScore { get { if (gameController.CurrentLevel.PlayerLevelRecord != null) return gameController.CurrentLevel.PlayerLevelRecord.starScore; else return StarScore.scores[0]; } }

        private ExperimentController experimentController;
        private MicrophoneInput microphoneInput;

        void Awake()
        {
            timer = GameObject.FindObjectOfType<TimerBehaviour>();
            playerLevelData = new PlayerLevelData();
            inGameMenu.LevelSession = this;
        }

        void Start()
        {
            gameController = GameObject.FindObjectOfType<GameController>();
            if (gameController == null)
            {
                Debug.LogError("No GameController found. This is probably because you are running the main scene without loading from the menu.");
                GameObject g = Instantiate(Resources.Load<GameObject>("GameController"));
                gameController = g.GetComponent<GameController>();
                
            }

            levelData = LoadLevel("levels/" + gameController.CurrentLevel.filename);
            CreateLevel(levelData);
            StartCoroutine(ShowIntroMenu(levelData));
            InitialiseExperimentLogging();
        }

        private void InitialiseExperimentLogging()
        {
            experimentController = GameObject.FindObjectOfType<ExperimentController>();
            if (experimentController != null)
            {
                experimentController.Telemetry.SendKeyValuePair(experiment.ExperimentKeys.LevelLoaded, gameController.CurrentLevel.filename);
                microphoneInput = GameObject.FindObjectOfType<MicrophoneInput>();
                microphoneInput.OnSyllablePeak += delegate()
                {
                    if (levelSessionState == LevelSessionState.Playing)
                        experimentController.SyllablesDetectedDuringPlay++;
                };
            }
        }

        private void ExperimentLogging()
        {
            if (experimentController != null)
            {
                if (levelSessionState == LevelSessionState.Playing)
                {
                    experimentController.Telemetry.SendStreamValue(LevelStream.Timer, timer.Time);
                    experimentController.Telemetry.SendStreamValue(LevelStream.MicrophoneInput, microphoneInput.Level);
                    experimentController.Telemetry.SendStreamValue(LevelStream.MicrophonePitch, microphoneInput.Pitch);
                    experimentController.Telemetry.SendStreamValue(LevelStream.MicrophoneSyllables, experimentController.SyllablesDetectedDuringPlay);
                    experimentController.Telemetry.SendFrame();
                }
            }
        }

        void Update()
        {
            if (Input.GetKeyDown("p"))
                Pause();

            ExperimentLogging();
        }

        public void CrossedFinishLine()
        {
            if (levelSessionState != LevelSessionState.End)
            {
                if (experimentController != null)
                    experimentController.Telemetry.SendEvent(LevelEvent.CrossedFinishLine);

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
                if (experimentController != null)
                    experimentController.Telemetry.SendEvent(LevelEvent.FellOffBottomOfScreen);

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
                if (experimentController != null)
                    experimentController.Telemetry.SendEvent(LevelEvent.GeomCrash);

                timer.ClockRunning = false;
                SetPlayerEndTime();
                playerLevelData.LevelResult = LevelResult.Crash;
                levelSessionState = LevelSessionState.End;
                StartCoroutine(ShowFailedMenu(playerLevelData));
            }
        }

        public void PlayerSunk()
        {
            if (levelSessionState != LevelSessionState.End)
            {
                if (experimentController != null)
                    experimentController.Telemetry.SendEvent(LevelEvent.OceanCrash);

                timer.ClockRunning = false;
                SetPlayerEndTime();
                playerLevelData.LevelResult = LevelResult.Sunk;
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
            Application.LoadLevel("main-menu");
        }

        private LevelData LoadLevel(string filePath)
        {
            return new LevelData(new LevelFileLoader(filePath));
        }

        private void CreateLevel(LevelData levelData)
        {
            GameObject g = new GameObject();
            g.name = "LevelBehaviour";
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
            if (experimentController != null)
                experimentController.Telemetry.SendEvent(LevelEvent.ShowIntroMenu);

            levelSessionState = LevelSessionState.Intro;
            inGameMenu.InGameMenu = new LevelIntroMenu(levelData);
            FreezePlay(true);
            yield return new WaitForSeconds(1f);
            inGameMenu.Destroy();
            FreezePlay(false);
            levelSessionState = LevelSessionState.Playing;

            if (experimentController != null)
                experimentController.Telemetry.SendEvent(LevelEvent.HideIntroMenu);
        }

        private IEnumerator ShowEndMenu(PlayerLevelData playerLevelData)
        {
            yield return new WaitForSeconds(1);
            levelEndMenu.Create(playerLevelData, levelData, gameController.CurrentLevel.PlayerLevelRecord);
            if (experimentController != null)
                experimentController.Telemetry.SendEvent(LevelEvent.ShowEndMenu);

            yield return new WaitForSeconds(1);
            FreezePlay(true);
        }

        private IEnumerator ShowFailedMenu(PlayerLevelData playerLevelData)
        {
            yield return new WaitForSeconds(1);
            inGameMenu.InGameMenu = new LevelFailedMenu(playerLevelData);
            if (experimentController != null)
                experimentController.Telemetry.SendEvent(LevelEvent.ShowFailedMenu);

            yield return new WaitForSeconds(1);
            FreezePlay(true);
        }

        private void FreezePlay(bool frozen)
        {
            levelBehaviour.FreezePlay(frozen);
            timer.ClockRunning = !frozen;
        }

        private void EnPause()
        {
            if (experimentController != null)
                experimentController.Telemetry.SendEvent(LevelEvent.EnPause);

            levelSessionState = LevelSessionState.Paused;
            levelBehaviour.FreezePlay(true);
            inGameMenu.InGameMenu = new PauseMenu();
            timer.ClockRunning = false;
        }

        private void UnPause()
        {
            if (experimentController != null)
                experimentController.Telemetry.SendEvent(LevelEvent.UnPause);

            levelSessionState = LevelSessionState.Playing;
            levelBehaviour.FreezePlay(false);
            inGameMenu.Destroy();
            timer.ClockRunning = true;
        }



    }
}