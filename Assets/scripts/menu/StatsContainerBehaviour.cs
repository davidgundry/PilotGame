using UnityEngine;
using UnityEngine.UI;
using player;

namespace menu
{
    public class StatsContainerBehaviour : MonoBehaviour
    {

        public Text stats1;
        public Text stats2;

        public void Create(PlayerLevelData playerLevelData)
        {
            stats1.text = playerLevelData.Time + "\n" + playerLevelData.Crashes + "\n" + playerLevelData.Distance;
            stats2.text = playerLevelData.Time + "\n" + playerLevelData.Crashes + "\n" + playerLevelData.Distance;
        }
    }
}