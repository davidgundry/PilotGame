using UnityEngine;

namespace menu.experiment
{
    public class ExperimentEndBehaviour : MonoBehaviour
    {
        public WaitingBar waitingBar;

        void Start()
        {
            AsyncOperation async = Application.LoadLevelAsync("main-menu");
            async.allowSceneActivation = false;
            waitingBar.OnComplete += delegate() { async.allowSceneActivation = true; };
            waitingBar.StartWaitingBar(2);

            ExperimentController experimentController = GameObject.FindObjectOfType<ExperimentController>();
            if (experimentController != null)
            {
                experimentController.ExperimentEnd();
            }
        }
    }
}