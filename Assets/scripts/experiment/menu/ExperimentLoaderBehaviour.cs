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
        public GameObject startButton;
        public GameObject explainText;
        private AsyncOperation async;

        void Start()
        {
            startButton.SetActive(false);
            explainText.SetActive(false);
            connectionInfoText.text = "";
            experimentController = GameObject.FindObjectOfType<ExperimentController>();

            async = Application.LoadLevelAsync("load");  
            async.allowSceneActivation = false;
            StartCoroutine(WaitForMicrophoneAuthorisation());
            waitingBar.StartWaitingBar(0);
        }

        private void CheckTelemetryKey()
        {
            if (!experimentController.Telemetry.CurrentKeyIsFetched)
                StartCoroutine(WaitForTelemetryKey());
            else
                LoadingDone();
        }

        private IEnumerator WaitForMicrophoneAuthorisation()
        {
            while (!Application.HasUserAuthorization(UserAuthorization.Microphone))
            {
                yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
            }
            explainText.SetActive(true);
            waitingBar.OnComplete += delegate()
            {
                CheckTelemetryKey();
            };
            waitingBar.StartWaitingBar(1f);
            
        }

        private IEnumerator WaitForTelemetryKey()
        {
            connectionInfoText.text = "Waiting for server...";
            /*while (!experimentController.Telemetry.KeyManager.CurrentKeyIsFetched)
            {
                yield return new WaitForSeconds(0.2f);
            }*/
            yield return new WaitForSeconds(0.2f);
            LoadingDone();
        }

        private void LoadingDone()
        {
            connectionInfoText.gameObject.SetActive(false);
            waitingBar.gameObject.SetActive(false);
            startButton.SetActive(true);
        }        

        public void StartButton()
        {
            async.allowSceneActivation = true;
        }
    }
}