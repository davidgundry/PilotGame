using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using menu;

namespace experiment.menu
{
    public class ExperimentEndBehaviour : MonoBehaviour
    {
        public UploadBar uploadBar;
        private ExperimentController experimentController;
        public Text connectionInfoText;

        void Start()
        {
            connectionInfoText.text = "";
            AsyncOperation async = Application.LoadLevelAsync("main-menu");
            async.allowSceneActivation = false;

            experimentController = GameObject.FindObjectOfType<ExperimentController>();
            if (experimentController != null)
            {
                experimentController.ExperimentEnd();
                uploadBar.SetStartingCachedFiles(experimentController.Telemetry.CachedFiles);
                StartCoroutine(UpdateProgress(async));
            }
            else
                ReadyToLeaveScene(async);
        }

        private IEnumerator UpdateProgress(AsyncOperation async)
        {
            connectionInfoText.text = "Uploading data...";
            bool done = false;
            while (!done)
            {
                yield return new WaitForSeconds(0.2f);
                uploadBar.SetRemainingCachedFiles(experimentController.Telemetry.CachedFiles);
                if (experimentController.Telemetry.IsAllDataUploaded())
                    done = true;
            }
            ReadyToLeaveScene(async);
        }

        private void ReadyToLeaveScene(AsyncOperation async)
        {
            GameObject.Destroy(experimentController);
            async.allowSceneActivation = true;
        }
    }
}