using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using menu;

namespace experiment.menu
{
    public class ExperimentLoaderBehaviour : MonoBehaviour
    {
        ExperimentController experimentController;
        public WaitingBar waitingBar;
        public Text connectionInfoText;

        void Start()
        {
            connectionInfoText.text = "";
            experimentController = GameObject.FindObjectOfType<ExperimentController>();
            StartCoroutine(AskMicrophoneAuthorisation());

            AsyncOperation async = Application.LoadLevelAsync("main-menu");  
            async.allowSceneActivation = false;
            waitingBar.OnComplete += delegate()
            { 
                CheckMicrophone(async);
            };
            waitingBar.StartWaitingBar(2);
        }

        private void CheckMicrophone(AsyncOperation async)
        {
            if (!Application.HasUserAuthorization(UserAuthorization.Microphone))
                StartCoroutine(WaitForMicrophoneAuthorisation(async));
            else
                CheckTelemetryKey(async);
        }

        private void CheckTelemetryKey(AsyncOperation async)
        {
            if (!experimentController.Telemetry.KeyManager.CurrentKeyIsFetched)
                StartCoroutine(WaitForTelemetryKey(async));
            else
                LoadingDone(async);
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
            CheckTelemetryKey(async);
        }

        private IEnumerator WaitForTelemetryKey(AsyncOperation async)
        {
            connectionInfoText.text = "Waiting for server...";
            /*while (!experimentController.Telemetry.KeyManager.CurrentKeyIsFetched)
            {
                yield return new WaitForSeconds(0.2f);
            }*/
            yield return new WaitForSeconds(0.2f);
            LoadingDone(async);
        }

        private void LoadingDone(AsyncOperation async)
        {
            async.allowSceneActivation = true;
        }
    }
}