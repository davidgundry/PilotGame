using UnityEngine;
using System.Collections;
using TelemetryTools;

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

    [RequireComponent(typeof(TelemetryMonitor))]
    public class ExperimentController : MonoBehaviour
    {
        private TelemetryMonitor telemetryMonitor;
        public Telemetry Telemetry { get { return telemetryMonitor.Telemetry; } }
        public string DataKey { get { if (Telemetry.Exists) return telemetryMonitor.GetKey(); else return null; } }
        public int SyllablesDetectedDuringPlay { get; set; }
        
        private const string remoteURL = "";



        void Awake()
        {
            DontDestroyOnLoad(this);
            telemetryMonitor = GetComponent<TelemetryMonitor>();
        }

        void Start()
        {
            ConfigureTelemetry();
            ExperimentStart();
        }

        void OnLevelWasLoaded()
        {
            if (Telemetry.Exists)
                Telemetry.SendKeyValuePair(experiment.ExperimentKeys.SceneLoaded, Application.loadedLevelName);
        }

        private void ConfigureTelemetry()
        {
            Telemetry.Create();
            telemetryMonitor.SetRemoteURLs(remoteURL);
            Telemetry.KeyManager.ReuseOrCreateKey();
        }
        
        public void ExperimentStart()
        {
            Telemetry.SendEvent(ExperimentEvent.ExperimentStart);
        }

        public void ExperimentEnd()
        {
            Telemetry.SendEvent(ExperimentEvent.ExperimentEnd);
        }

    }
}