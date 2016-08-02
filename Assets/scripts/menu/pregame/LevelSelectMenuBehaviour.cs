using UnityEngine;
using System.Collections;

namespace menu.pregame
{
    public class LevelSelectMenuBehaviour : MonoBehaviour
    {
        public LevelScrollListBehaviour levelScrollList;
        private GameController gameController;

        void Awake()
        {
            gameController = GameObject.FindObjectOfType<GameController>();
            if (gameController == null)
                Debug.LogError("No GameController found. This is probably because you are running the main scene without loading from the menu. Trying to instantiate a new Game Controller");

            
            gameController = GameObject.Instantiate<GameController>(Resources.Load<GameController>("prefabs/gameController"));
        }

        void Start()
        {
            levelScrollList.Create(gameController.LevelList);
        }

        public void ContinueButton()
        {
            gameController.PlayCurrentLevel();
        }

        public void BackButton()
        {
            Application.LoadLevel("main-menu");
        }

    }
}