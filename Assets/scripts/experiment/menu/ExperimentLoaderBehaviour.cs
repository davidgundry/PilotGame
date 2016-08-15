using UnityEngine;
using System.Collections;
using menu;

namespace experiment.menu
{
    public class ExperimentLoaderBehaviour : MonoBehaviour
    {

        public WaitingBar waitingBar;

        void Start()
        {
            StartCoroutine(AskMicrophoneAuthorisation());

            AsyncOperation async = Application.LoadLevelAsync("main-menu");  
            async.allowSceneActivation = false;
            waitingBar.OnComplete += delegate()
            { 
                if (Application.HasUserAuthorization(UserAuthorization.Microphone))
                    LoadingDone(async);
                else
                    StartCoroutine(WaitForMicrophoneAuthorisation(async));
            };
            waitingBar.StartWaitingBar(2);
        }

        private IEnumerator AskMicrophoneAuthorisation()
        {
            yield return new WaitForSeconds(0.5f);
            Application.RequestUserAuthorization(UserAuthorization.Microphone);
        }

        private IEnumerator WaitForMicrophoneAuthorisation(AsyncOperation async)
        {
            
            while (!Application.HasUserAuthorization(UserAuthorization.Microphone))
            {
                yield return new WaitForSeconds(0.2f);
            }
            LoadingDone(async);
        }

        private void LoadingDone(AsyncOperation async)
        {
            async.allowSceneActivation = true;
        }
    }
}