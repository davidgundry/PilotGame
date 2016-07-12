//#define TELEMETRYTOOLSEXISTS

using UnityEngine;
using System.Collections;

namespace MicTools
{
    public enum SoundEvent
    {
        PermissionRequired,
        PermissionGranted,
        MicrophoneReady,
        BufferReady,
        SyllableStart,
        SyllableEnd,
        InputStart,
        InputEnd,
        AudioStart,
        AudioEnd,
        SyllablePeak
    }

    public class LogMT
    {
        private const bool mtLoggingOn = false;

        public static void Log(string text)
        {
#if TELEMETRYTOOLSEXISTS
            if (mtLoggingOn)
                TelemetryTools.Telemetry.Instance.SendEvent("Log: " + text);
#endif
        }
        public static void LogWarning(string text)
        {
#if TELEMETRYTOOLSEXISTS
            if (mtLoggingOn)
                TelemetryTools.Telemetry.Instance.SendEvent("LogWarning: " + text);
#endif
        }
        public static void LogError(string text)
        {
#if TELEMETRYTOOLSEXISTS
            if (mtLoggingOn)
                TelemetryTools.Telemetry.Instance.SendEvent("LogError: " + text);
#endif
        }
        public static void LogError(string text, Object o)
        {
#if TELEMETRYTOOLSEXISTS
            if (mtLoggingOn)
                TelemetryTools.Telemetry.Instance.SendEvent("LogError: " + text);
#endif
        }
        public static void SendStreamValue(string tag, System.ValueType value)
        {
#if TELEMETRYTOOLSEXISTS
            if (mtLoggingOn)
                TelemetryTools.Telemetry.Instance.SendStreamValue(tag, value);
#endif
        }

        public static void SendByteDataBase64(string tag, byte[] data)
        {
#if TELEMETRYTOOLSEXISTS
            if (mtLoggingOn)
                TelemetryTools.Telemetry.Instance.SendByteDataBase64(tag, data);
#endif
        }

        public static void SendStreamValueBlock(string tag, float[] data)
        {
#if TELEMETRYTOOLSEXISTS
            if (mtLoggingOn)
                TelemetryTools.Telemetry.Instance.SendStreamValueBlock(tag, data);
#endif
        }

        public static void LogSoundEvent(SoundEvent e)
        {
#if TELEMETRYTOOLSEXISTS
            if (mtLoggingOn)
                TelemetryTools.Telemetry.Instance.SendEvent("SoundEvent:" + SoundEventToString(e));
#endif
        }

        private static string SoundEventToString(SoundEvent e)
        {
            switch (e)
            {
                case SoundEvent.PermissionRequired:
                    return "Permission Required";
                case SoundEvent.PermissionGranted:
                    return "Permission Granted";
                case SoundEvent.MicrophoneReady:
                    return "Microphone Ready";
                case SoundEvent.SyllableStart:
                    return "Syllable Start";
                case SoundEvent.SyllableEnd:
                    return "Syllable End";
                case SoundEvent.InputStart:
                    return "Input Start";
                case SoundEvent.InputEnd:
                    return "Input End";
                case SoundEvent.AudioStart:
                    return "Audio Start";
                case SoundEvent.AudioEnd:
                    return "Audio End";
                case SoundEvent.SyllablePeak:
                    return "Syllable Peak";
            }
            return "Unrecognised Event";
        }

    }
}