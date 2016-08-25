#if (!UNITY_WEBPLAYER)
#define LOCALSAVEENABLED
#endif


using UnityEngine;
using System.Collections;
using TelemetryTools;
using TelemetryTools.Behaviour;

namespace experiment
{
    public static class ExperimentEvent
    {
        public const string ExperimentStart = "ExperimentStart";
        public const string ExperimentEnd = "ExperimentEnd";
        
    }

    public static class ExperimentKeys
    {
        public const string LevelLoaded = "LevelLoaded";
        public const string SceneLoaded = "SceneLoaded";
    }

    [RequireComponent(typeof(TelemetryController))]
    public class ExperimentController : MonoBehaviour
    {
        private TelemetryController telemetryController;
        public Telemetry Telemetry { get { return telemetryController.Telemetry; } }
        public string DataKey { get { if (Telemetry != null) return telemetryController.GetKey(); else return null; } }
        public int SyllablesDetectedDuringPlay { get; set; }
        public bool TimeUp { get { if (TimerStarted) { return (startTime + duration) - Time.time < 0; } else return false; } }
        private float startTime = 0;
        private bool TimerStarted { get { return startTime != 0; } }
        private float duration = 0.1f * 60;

        private const string remoteURL = "";


        void Awake()
        {
            DontDestroyOnLoad(this);
            telemetryController = GetComponent<TelemetryController>();
        }

        void Start()
        {
            ConfigureTelemetry();
            ExperimentStart();
        }

        void Update()
        {
            StartCoroutine(CheckTimer());
        }

        private IEnumerator CheckTimer()
        {
            while (true)
            {
                if (TimeUp)
                    if (Application.loadedLevelName != "main")
                        if (Application.loadedLevelName != "questionnaire")
                            Application.LoadLevel("questionnaire");
                yield return new WaitForSeconds(1);
            }
        }

        void OnLevelWasLoaded()
        {
            if (Telemetry != null)
                Telemetry.SendKeyValuePair(experiment.ExperimentKeys.SceneLoaded, Application.loadedLevelName);
        }

        public void StartPlay()
        {
            if (startTime == 0)
                startTime = Time.time;
        }

        public void NoMicrophoneSelected()
        {
            telemetryController.EndTelemetry();
            DestroyExperiment();
        }

        private void ConfigureTelemetry()
        {
            Telemetry Telemetry = telemetryController.CreateTelemetry(new URL(remoteURL));
            telemetryController.Telemetry = Telemetry;
            Telemetry.ReuseOrCreateKey();
        }
        
        public void ExperimentStart()
        {
            Telemetry.SendEvent(ExperimentEvent.ExperimentStart);
        }

        public void ExperimentEnd()
        {
            Telemetry.SendEvent(ExperimentEvent.ExperimentEnd);
            Telemetry.SendAllBuffered();
        }

        public void ShowQuestionnaire()
        {
            Application.LoadLevel("questionnaire");
        }

        public void DestroyExperiment()
        {
            telemetryController.EndTelemetry();
            GameObject.Destroy(gameObject);
        }

        public bool AllDataUploaded()
        {
            return Telemetry.IsAllDataUploaded();
        }

    }
}