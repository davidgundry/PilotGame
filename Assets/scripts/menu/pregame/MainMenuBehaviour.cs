using UnityEngine;

namespace menu.pregame
{
    public class MainMenuBehaviour : UIPane
    {
        GameController gameController;
        UIPanTransition uiPanTransition;

        void Start()
        {
            gameController = GameObject.FindObjectOfType<GameController>();
            uiPanTransition = GameObject.FindObjectOfType<UIPanTransition>();
        }

        public void StartGame()
        {
            gameController.StartGame();
        }

        public void Levels()
        {
            uiPanTransition.TransitionTo(1);
            //Application.LoadLevel("level-menu");
        }

        public override void Refresh()
        {
            
        }
    }
}