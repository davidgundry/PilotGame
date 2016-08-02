using UnityEngine;
using UnityEngine.UI;
using player.data;
using System;
using level;
using level.data;

namespace menu.pregame
{
    public class LevelDetailsMenuBehaviour : MonoBehaviour
    {
        public Image levelScreenshot;
        public Text statsText;
        public Text titleText;
        public StarBoxBehaviour starBox;

        private GameController gameController;

        void Start()
        {
            gameController = GameObject.FindObjectOfType<GameController>();
            if (gameController == null)
            {
                Debug.LogError("No GameController found. This is probably because you are running the main scene without loading from the menu. Trying to instantiate a new Game Controller");
                gameController = GameObject.Instantiate<GameController>(Resources.Load<GameController>("prefabs/gameController"));
            }

            LevelListData levelListData = gameController.SelectedLevel;
            titleText.text = levelListData.name;
            levelScreenshot.sprite = Resources.Load<Sprite>("screenshots/" + levelListData.filename);


            LevelData levelData = new LevelData(new LevelFileLoader("levels/"+levelListData.filename));

            statsText.text = CreateStatsText(levelListData.PlayerLevelRecord, levelData);
            if (levelListData.PlayerLevelRecord != null)
                starBox.Create(levelListData.PlayerLevelRecord.starScore);
            else
                starBox.Create(StarScore.scores[0]);
            starBox.transform.localScale = new Vector2(0.75f, 0.75f);
        }

        public void BackButton()
        {
            Application.LoadLevel("level-menu");
        }

        public void PlayButton()
        {
            gameController.PlaySelectedLevel();
        }

        private string CreateStatsText(PlayerLevelRecord playerLevelRecord, LevelData levelData)
        {
            if (playerLevelRecord == null)
                playerLevelRecord = new PlayerLevelRecord(0,StarScore.scores[0],0,0);
            TimeSpan t = TimeSpan.FromSeconds(playerLevelRecord.time);
            return string.Format("\n{0:d2}:{1:d2}\n{2}/{3}\n{4}/{5}", t.Minutes,t.Seconds,playerLevelRecord.coins,levelData.coinCount,playerLevelRecord.pickups, levelData.pickupCount);
        }

    }
}