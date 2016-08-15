using UnityEngine;
using System.Collections;
using TelemetryTools;

namespace experiment
{
    public static class ExperimentEvent
    {
        public const string ExperimentStart = "ES";
        public const string ExperimentEnd = "EE";
        public const string MenuShown = "MS";
        public const string LoaderShown = "LS";
        public const string LevelShown = "LvlS";
    }

    public static class ExperimentKeys
    {
        public const string LevelLoaded = "ll";
    }

    [RequireComponent(typeof(TelemetryMonitor))]
    public class ExperimentController : MonoBehaviour
    {
        private TelemetryMonitor telemetryMonitor;
        public Telemetry Telemetry { get { return telemetryMonitor.Telemetry; } }
        public string DataKey { get { if (Telemetry.Exists) return telemetryMonitor.GetKey(); else return null; } }

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
                switch (Application.loadedLevelName)
                {
                    case "main":
                        Telemetry.SendEvent(ExperimentEvent.LevelShown);
                        break;
                    case "main-menu":
                        Telemetry.SendEvent(ExperimentEvent.MenuShown);
                        break;
                    case "load":
                        Telemetry.SendEvent(ExperimentEvent.LoaderShown);
                        break;
                }
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