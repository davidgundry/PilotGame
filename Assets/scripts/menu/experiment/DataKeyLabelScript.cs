using UnityEngine;
using UnityEngine.UI;

namespace menu.experiment
{
    [RequireComponent(typeof(Text))]
    public class DataKeyLabelScript : MonoBehaviour
    {
        void Start()
        {
#if TELEMETRY
            TelemetryMonitor telemetryMonitor = GameObject.FindObjectOfType<TelemetryMonitor>();
            GetComponent<Text>().text = "Data Key: " + telemetryMonitor.Key;
#endif
        }

    }
}