using UnityEngine;
using UnityEngine.UI;
using player.data;
using System;
using level;
using level.data;

namespace menu.pregame
{
    public class LevelDetailsMenuBehaviour : UIPane
    {
        public Image levelScreenshot;
        public Text statsText;
        public Text titleText;
        public StarBoxBehaviour starBox;

        private GameController gameController;
        private UIPanTransition uiPanTransition;

        void Start()
        {
            gameController = GameObject.FindObjectOfType<GameController>();
            uiPanTransition = GameObject.FindObjectOfType<UIPanTransition>();
            if (gameController == null)
            {
                Debug.LogError("No GameController found. This is probably because you are running the main scene without loading from the menu. Trying to instantiate a new Game Controller");
                gameController = GameObject.Instantiate<GameController>(Resources.Load<GameController>("GameController"));
            }
        }

        public void BackButton()
        {
            uiPanTransition.TransitionTo(1);
        }

        public void PlayButton()
        {
            gameController.PlaySelectedLevel();
        }

        private string CreateStatsText(LevelListData levelListData)
        {
            PlayerLevelRecord playerLevelRecord = levelListData.PlayerLevelRecord;
            if (playerLevelRecord == null)
                playerLevelRecord = new PlayerLevelRecord(0,StarScore.scores[0],0,0);
            TimeSpan t = TimeSpan.FromSeconds(playerLevelRecord.time);
            if (playerLevelRecord.time == 0)
                return string.Format("\n-\n{0}/{1}", playerLevelRecord.coins, levelListData.coins);
            else
                return string.Format("\n{0:d2}:{1:d2}\n{2}/{3}", t.Minutes, t.Seconds, playerLevelRecord.coins, levelListData.coins);
        }

        public override void Refresh()
        {
            LevelListData levelListData = gameController.SelectedLevel;
            titleText.text = levelListData.name;
            levelScreenshot.sprite = Resources.Load<Sprite>("screenshots/" + levelListData.filename);


            statsText.text = CreateStatsText(levelListData);
            if (levelListData.PlayerLevelRecord != null)
                starBox.Refresh(levelListData.PlayerLevelRecord.starScore);
            else
                starBox.Refresh(StarScore.scores[0]);

            starBox.transform.localScale = new Vector2(0.75f, 0.75f);
        }
    }
}