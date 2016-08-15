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
            experimentController = GameObject.FindObjectOfType<ExperimentController>();
            if (experimentController != null)
            {
                experimentController.ExperimentEnd();
            }

            AsyncOperation async = Application.LoadLevelAsync("main-menu");
            async.allowSceneActivation = false;
            uploadBar.SetStartingCachedFiles(experimentController.Telemetry.CachedFiles);
            StartCoroutine(UpdateProgress(async));
        }

        private IEnumerator UpdateProgress(AsyncOperation async)
        {
            bool done = false;
            while (!done)
            {
                yield return new WaitForSeconds(0.2f);
                uploadBar.SetRemainingCachedFiles(experimentController.Telemetry.CachedFiles);
                if (experimentController.Telemetry.CachedFiles == 0)
                    done = true;
            }
            async.allowSceneActivation = true;
        }
    }
}