using UnityEngine;
using UnityEngine.UI;

namespace menu.inlevel
{
    /// <summary>
    /// Manages creating and displaying an in-game menu in Unity
    /// </summary>
    public class InGameMenuBehaviour : MonoBehaviour
    {

        private InGameMenu inGameMenu;
        public InGameMenu InGameMenu
        {
            get
            {
                return inGameMenu;
            }
            set
            {
                inGameMenu = value;
                if (inGameMenu != null)
                {
                    SetContents(inGameMenu);
                    Show(inGameMenu);
                }
                else
                    Hide();
            }
        }

        public LevelSession LevelSession { get; set; }

        public Image background;
        public Text titleText;
        public Text subtitleText;
        public InGameButtonContainerBehaviour buttonContainer;
        public StarBoxBehaviour starBox;
        public CompleteBoxBehaviour completeBox;

        private void SetContents(InGameMenu menuToDisplay)
        {
            titleText.text = menuToDisplay.title;
            subtitleText.text = menuToDisplay.subtitle;
            buttonContainer.Create(menuToDisplay.availableButtons, LevelSession);
            starBox.StarCount = 0;
        }

        public void Destroy()
        {
            InGameMenu = null;
        }

        void Start()
        {
            InGameMenu = null;
            starBox.Create(3);
        }

        private void Show(InGameMenu menuToDisplay)
        {
            SetActiveAll(true);
            if (!menuToDisplay.headerVisible)
            {
                starBox.gameObject.SetActive(false);
                completeBox.gameObject.SetActive(false);
            }
        }

        private void Hide()
        {
            SetActiveAll(false);
        }

        private void SetActiveAll(bool active)
        {
            background.gameObject.SetActive(active);
            titleText.gameObject.SetActive(active);
            subtitleText.gameObject.SetActive(active);
            buttonContainer.gameObject.SetActive(active);
            starBox.gameObject.SetActive(active);
            completeBox.gameObject.SetActive(active);
        }
    }
}