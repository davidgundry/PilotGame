using UnityEngine;
using UnityEngine.UI;

namespace menu.inlevel
{
    public class InGameButtonContainerBehaviour : MonoBehaviour
    {
        public Button retryButton;
        public Button menuButton;
        public Button resumeButton;

        public void Create(ButtonType[] availableButtons)
        {
            DeactivateAllButtons();
            ActivateButtons(availableButtons);
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
            Application.LoadLevel(Application.loadedLevel);
        }

        public void Next()
        {

        }

        public void Resume()
        {

        }

        public void Menu()
        {

        }

    }
}