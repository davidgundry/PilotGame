using UnityEngine;
using UnityEngine.UI;
using player.data;

namespace menu.pregame
{
    public class LevelSelectMenuBehaviour : UIPane
    {
        public LevelScrollListBehaviour levelScrollList;
        private GameController gameController;

        public Text statsText;
        private UIPanTransition uiPanTransition;

        void Awake()
        {
            gameController = GameObject.FindObjectOfType<GameController>();
            uiPanTransition = GameObject.FindObjectOfType<UIPanTransition>();
        }

        void Start()
        {
            if (gameController == null)
            {
                Debug.LogError("No GameController found. This is probably because you are running the game without loading from the first menu. Trying to instantiate a new Game Controller");
                gameController = GameObject.Instantiate<GameController>(Resources.Load<GameController>("prefabs/gameController"));
            }
        }

        public void ContinueButton()
        {
            gameController.PlayCurrentLevel();
        }

        public void BackButton()
        {
            uiPanTransition.TransitionTo(0);
        }

        private string CreateStatsText(PlayerGameProgress playerGameProgress)
        {
            return string.Format("{0}/{1}\n{2}/{3}\n{4}/{5}", playerGameProgress.Levels, playerGameProgress.TotalLevels, playerGameProgress.Stars, playerGameProgress.TotalStars, playerGameProgress.Coins,playerGameProgress.TotalCoins);
        }

        public override void Refresh()
        {
            statsText.text = CreateStatsText(gameController.PlayerGameProgress);
            levelScrollList.Create(gameController.LevelList);
        }

    }
}