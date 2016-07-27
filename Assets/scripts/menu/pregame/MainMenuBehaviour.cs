using UnityEngine;

namespace menu.pregame
{
    public class MainMenuBehaviour : MonoBehaviour
    {
        GameController gameController;

        void Start()
        {
            gameController = GameObject.FindObjectOfType<GameController>();
        }

        public void StartGame()
        {
            gameController.StartGame();
        }

        public void Levels()
        {

        }
    }
}