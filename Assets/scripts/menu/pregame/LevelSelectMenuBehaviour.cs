using UnityEngine;
using UnityEngine.UI;
using player.data;

namespace menu.pregame
{
    public class LevelSelectMenuBehaviour : MonoBehaviour
    {
        public LevelScrollListBehaviour levelScrollList;
        private GameController gameController;

        public Text statsText;

        void Awake()
        {
            gameController = GameObject.FindObjectOfType<GameController>();
            if (gameController == null)
            {
                Debug.LogError("No GameController found. This is probably because you are running the main scene without loading from the menu. Trying to instantiate a new Game Controller");
                gameController = GameObject.Instantiate<GameController>(Resources.Load<GameController>("prefabs/gameController"));
            }
            statsText.text = CreateStatsText(gameController.PlayerGameProgress);
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

        private string CreateStatsText(PlayerGameProgress playerGameProgress)
        {
            return string.Format("{0}/{1}\n{2}/{3}", playerGameProgress.Levels, gameController.LevelList.Length, playerGameProgress.Stars, "?");
        }

    }
}