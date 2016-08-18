using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace menu.pregame
{
    public class LoaderBehaviour : MonoBehaviour
    {

        public WaitingBar waitingBar;

        void Start()
        {
            AsyncOperation async = Application.LoadLevelAsync("main");
            async.allowSceneActivation = false;
            waitingBar.OnComplete += delegate() { async.allowSceneActivation = true; };
            waitingBar.StartWaitingBar(0);
        }

        public void MicrophoneYesButton()
        {

        }

        public void MicrophoneNoButton()
        {

        }

    }
}