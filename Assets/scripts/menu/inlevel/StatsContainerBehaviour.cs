using UnityEngine;
using UnityEngine.UI;
using player.data;
using System;
using level.data;

namespace menu.inlevel
{
    public class StatsContainerBehaviour : MonoBehaviour
    {
        public Text statsText;

        public void Create(PlayerLevelData playerLevelData, LevelData levelData, PlayerLevelRecord playerLevelRecord)
        {
            TimeSpan t = TimeSpan.FromSeconds(playerLevelData.Time);
            TimeSpan best = new TimeSpan();
            if (playerLevelRecord != null)
                best = TimeSpan.FromSeconds(playerLevelRecord.time);
            statsText.text = string.Format("{0:d2}:{1:d2} ({2:d2}:{3:d2})\n   {4}/{5}\n   {6}/{7}", t.Minutes, t.Seconds, best.Minutes, best.Seconds, playerLevelData.Hoops, levelData.hoopCount, playerLevelData.Coins, levelData.coinCount);
        }
    }
}