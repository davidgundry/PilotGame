﻿#if (!UNITY_WEBPLAYER)
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

        void OnLevelWasLoaded()
        {
            if (Telemetry != null)
                Telemetry.SendKeyValuePair(experiment.ExperimentKeys.SceneLoaded, Application.loadedLevelName);
        }

        private void ConfigureTelemetry()
        {

            Telemetry Telemetry = telemetryController.CreateTelemetry(remoteURL);
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

        public bool AllDataUploaded()
        {
            return Telemetry.IsAllDataUploaded();
        }

    }
}