using UnityEngine;
using menu;
namespace menu.experiment
{
    public class ExperimentLoaderBehaviour : MonoBehaviour
    {

        public WaitingBar waitingBar;

        void Start()
        {
            AsyncOperation async = Application.LoadLevelAsync("main-menu");
            async.allowSceneActivation = false;
            waitingBar.OnComplete += delegate() { async.allowSceneActivation = true; };
            waitingBar.StartWaitingBar(2);
        }
    }
}