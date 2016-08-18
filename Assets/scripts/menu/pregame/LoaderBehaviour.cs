using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace menu.pregame
{
    public class LoaderBehaviour : MonoBehaviour
    {

        public WaitingBar waitingBar;
        UIPanTransition uiPanTransition;

        private AsyncOperation async;

        private bool awaitingMicrophoneResponse;

        void Start()
        {
            GameController gameController = GameObject.FindObjectOfType<GameController>();
            awaitingMicrophoneResponse = (gameController.UsingMicrophone == null);

            uiPanTransition = GameObject.FindObjectOfType<UIPanTransition>();
            if (awaitingMicrophoneResponse)
                uiPanTransition.SkipTo(0);
            else
                uiPanTransition.SkipTo(1);

            async = Application.LoadLevelAsync("main");
            async.allowSceneActivation = false;
            waitingBar.OnComplete += delegate()
            {
                if (!awaitingMicrophoneResponse)
                    ChangeScene();
                else
                    StartCoroutine(WaitForPlayerInput());
            };
            waitingBar.StartWaitingBar(0);
        }

        private IEnumerator WaitForPlayerInput()
        {
            while (awaitingMicrophoneResponse)
            {
                yield return new WaitForSeconds(0.1f);
            }
            waitingBar.StartWaitingBar(2);
        }

        private void ChangeScene()
        {
            async.allowSceneActivation = true;
        }

        public void MicrophoneYesButton()
        {
            GameController gameController = GameObject.FindObjectOfType<GameController>();
            gameController.UsingMicrophone = true;
            uiPanTransition.SkipTo(1);
            awaitingMicrophoneResponse = false;
            Application.RequestUserAuthorization(UserAuthorization.Microphone);

        }

        public void MicrophoneNoButton()
        {
            GameController gameController = GameObject.FindObjectOfType<GameController>();
            gameController.UsingMicrophone = false;
            uiPanTransition.SkipTo(2);
            awaitingMicrophoneResponse = false;
        }

    }
}