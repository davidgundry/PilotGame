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
        private TelemetryController telemetryMonitor;
        public Telemetry Telemetry { get { return telemetryMonitor.Telemetry; } }
        public string DataKey { get { if (Telemetry != null) return telemetryMonitor.GetKey(); else return null; } }
        public int SyllablesDetectedDuringPlay { get; set; }
        
        private const string remoteURL = "";



        void Awake()
        {
            DontDestroyOnLoad(this);
            telemetryMonitor = GetComponent<TelemetryController>();
        }

        void Start()
        {
            ConfigureTelemetry();
            ExperimentStart();
        }

        void OnLevelWasLoaded()
        {
            if (Telemetry != null)
                Telemetry.SendKeyValuePair(experiment.ExperimentKeys.SceneLoaded, Application.loadedLevelName);
        }

        private void ConfigureTelemetry()
        {
            Telemetry Telemetry = new Telemetry();
            telemetryMonitor.Telemetry = Telemetry;
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