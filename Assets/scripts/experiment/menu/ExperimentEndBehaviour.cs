using UnityEngine;
using System.Collections;
using menu;

namespace experiment.menu
{
    public class ExperimentEndBehaviour : MonoBehaviour
    {
        public UploadBar uploadBar;
        private ExperimentController experimentController;

        void Start()
        {
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
                async.allowSceneActivation = true;
        }

        private IEnumerator UpdateProgress(AsyncOperation async)
        {
            bool done = false;
            while (!done)
            {
                yield return new WaitForSeconds(0.2f);
                uploadBar.SetRemainingCachedFiles(experimentController.Telemetry.CachedFiles);
                if (experimentController.Telemetry.AllDataUploaded())
                    done = true;
            }
            async.allowSceneActivation = true;
        }
    }
}