using UnityEngine;
using UnityEngine.UI;

namespace menu.inlevel
{
    public class InGameButtonContainerBehaviour : MonoBehaviour
    {
        public Button retryButton;
        public Button menuButton;
        public Button resumeButton;

        private LevelSession levelSession;

        public void Create(ButtonType[] availableButtons, LevelSession levelSession)
        {
            DeactivateAllButtons();
            ActivateButtons(availableButtons);
            this.levelSession = levelSession;

            if ((levelSession == null) && (availableButtons.Length > 0))
                throw new System.ArgumentNullException("LevelSession is null and there are active buttons. A valid LevelSession is required for buttons to work.");
        }

        private void DeactivateAllButtons()
        {
            menuButton.gameObject.SetActive(false);
            retryButton.gameObject.SetActive(false);
            resumeButton.gameObject.SetActive(false);
        }

        private void ActivateButtons(ButtonType[] availableButtons)
        {
            if (availableButtons != null)
                foreach (ButtonType button in availableButtons)
                {
                    switch (button)
                    {
                        case ButtonType.Menu:
                            menuButton.gameObject.SetActive(true);
                            break;
                        case ButtonType.Retry:
                            retryButton.gameObject.SetActive(true);
                            break;
                        case ButtonType.Resume:
                            resumeButton.gameObject.SetActive(true);
                            break;
                    }
                }
        }

        public void Retry()
        {
            levelSession.Retry();
        }

        public void Next()
        {
            levelSession.Next();
        }

        public void Resume()
        {
            levelSession.Resume();
        }

        public void Menu()
        {
            levelSession.Menu();
        }

    }
}