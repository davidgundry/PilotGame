using UnityEngine;
using UnityEngine.UI;
using player.data;
using System;

namespace menu
{
    public class StatsContainerBehaviour : MonoBehaviour
    {

        public Text stats1;
        public Text stats2;

        public void Create(PlayerLevelData playerLevelData)
        {
            TimeSpan t = TimeSpan.FromSeconds(playerLevelData.Time);
            stats1.text = string.Format("{0:D2}:{1:D2}\n{2}\n{3}", t.Minutes, t.Seconds, (int) playerLevelData.Distance, playerLevelData.FuelUsed);
            stats2.text = string.Format("{0}\n{1}\n{2}", playerLevelData.Crashes, playerLevelData.Coins, playerLevelData.Pickups);
        }
    }
}