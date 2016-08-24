using UnityEngine;
using UnityEngine.UI;

namespace menu.pregame
{
    public class MainMenuBehaviour : UIPane
    {
        GameController gameController;
        UIPanTransition uiPanTransition;

        public Button levelsButton;

        void Start()
        {
            gameController = GameObject.FindObjectOfType<GameController>();
            uiPanTransition = GameObject.FindObjectOfType<UIPanTransition>();

            if (gameController.UnlockedALevel)
                levelsButton.interactable = true;
            else
                levelsButton.interactable = false;
        }

        public void StartGame()
        {
            gameController.StartGame();
        }

        public void Levels()
        {
            uiPanTransition.TransitionTo(1);
        }

        public override void Refresh()
        {
            
        }
    }
}