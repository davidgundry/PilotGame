using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace menu.pregame
{
    public class LoaderBehaviour : MonoBehaviour
    {
        public WaitingBar waitingBar;
        UIPanTransition uiPanTransition;
        public Button[] beginButtons;

        public Button yesButton;
        public Button noButtion;
        public GameObject title;

        private GameController gameController;

        private AsyncOperation async;

        private bool awaitingMicrophoneResponse;

        void Start()
        {
            gameController = GameObject.FindObjectOfType<GameController>();
            uiPanTransition = GameObject.FindObjectOfType<UIPanTransition>();

            awaitingMicrophoneResponse = (gameController.UsingMicrophone == null);
            waitingBar.gameObject.SetActive(false);
            HideBeginButtons();
            StartLevelLoading();

            if (awaitingMicrophoneResponse)
                StartCoroutine(WaitForPlayerMicChoice());
            else
                ShowControlsPane();
        }

        public void MicrophoneYesButton()
        {
            gameController.UsingMicrophone = true;
            Application.RequestUserAuthorization(UserAuthorization.Microphone);
            HideChoiceButtonsAndTitle();
        }

        public void MicrophoneNoButton()
        {
            gameController.UsingMicrophone = false;
            awaitingMicrophoneResponse = false;
        }

        public void BeginButton()
        {
            ChangeScene();
        }

        private void ChangeScene()
        {
            async.allowSceneActivation = true;
        }

        private IEnumerator WaitForPlayerMicChoice()
        {
            ShowMicQuestionPane();
            while (awaitingMicrophoneResponse)
            {
                if (Application.HasUserAuthorization(UserAuthorization.Microphone))
                    awaitingMicrophoneResponse = false;
                    
                yield return new WaitForSeconds(0.1f);
            }
            ShowControlsPane();
        }

        private void StartWaitingBar()
        {
            waitingBar.gameObject.SetActive(true);
            waitingBar.OnComplete += delegate()
            {
                waitingBar.gameObject.SetActive(false);
                ShowBeginButtons();
            };
            waitingBar.StartWaitingBar(2);
        }

        private void StartLevelLoading()
        {
            async = Application.LoadLevelAsync("main");
            async.allowSceneActivation = false;
        }

        private void ShowControlsPane()
        {
            if (gameController.UsingMicrophone == true)
                uiPanTransition.SkipTo(1);
            else if (gameController.UsingMicrophone == false)
                uiPanTransition.SkipTo(2);

            StartWaitingBar();
        }

        private void ShowMicQuestionPane()
        {
            uiPanTransition.SkipTo(0);
        }

        private void HideChoiceButtonsAndTitle()
        {
            yesButton.gameObject.SetActive(false);
            noButtion.gameObject.SetActive(false);
            title.SetActive(false);
        }

        private void ShowChoiceButtonsAndTitle()
        {
            yesButton.gameObject.SetActive(true);
            noButtion.gameObject.SetActive(true);
            title.SetActive(true);
        }

        private void HideBeginButtons()
        {
            foreach (Button b in beginButtons)
            {
                b.gameObject.SetActive(false);
            }
        }

        private void ShowBeginButtons()
        {
            foreach (Button b in beginButtons)
            {
                b.gameObject.SetActive(true);
            }
        }
    }
}