using UnityEngine;
using UnityEngine.UI;
using experiment;

namespace experiment.menu
{
    [RequireComponent(typeof(Text))]
    public class DataKeyLabelScript : MonoBehaviour
    {
        void Start()
        {
            ExperimentController experimentController = GameObject.FindObjectOfType<ExperimentController>();
            if (experimentController != null)
            {
                GetComponent<Text>().text = "Data Key: " + experimentController.DataKey;
            }
            else
            {
                GetComponent<Text>().text = "";
            }
        }

    }
}