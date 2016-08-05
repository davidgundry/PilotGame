using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using player;
using player.data;
using menu.inlevel;
using level.data;

namespace menu.inlevel
{
    public class LevelEndMenuBehaviour : MonoBehaviour
    {
        public StarsContainerBehaviour starsContainer;
        public StatsContainerBehaviour statsContainer;
        public Button retryButton;
        public Button nextButton;
        public Image background;

        void Awake()
        {
            SetAllActive(false);
        }

        private void SetAllActive(bool active)
        {
            starsContainer.gameObject.SetActive(active);
            statsContainer.gameObject.SetActive(active);
            retryButton.gameObject.SetActive(active);
            nextButton.gameObject.SetActive(active);
            background.gameObject.SetActive(active);
        }

        public void Create(PlayerLevelData playerLevelData, LevelData levelData, PlayerLevelRecord playerLevelRecord)
        {
            StartCoroutine(LevelCompleteMenu(playerLevelData, levelData, playerLevelRecord));
        }

        private IEnumerator LevelCompleteMenu(PlayerLevelData playerLevelData, LevelData levelData, PlayerLevelRecord playerLevelRecord)
        {
            starsContainer.SetStars(playerLevelData.StarScore);
            starsContainer.gameObject.SetActive(true);
            background.gameObject.SetActive(true);
            RectTransform rt = starsContainer.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector3(0, 0, 0);
            
            yield return new WaitForSeconds(0.5f);

            while (rt.anchoredPosition.y < 250)
            {
                rt.anchoredPosition = new Vector3(rt.anchoredPosition.x, rt.anchoredPosition.y + Time.deltaTime * 200, 0);
                yield return null;
            }

            statsContainer.gameObject.SetActive(true);
            statsContainer.Create(playerLevelData, levelData, playerLevelRecord);
            yield return new WaitForSeconds(0.5f);
            retryButton.gameObject.SetActive(true);

            if (playerLevelData.StarScore > StarScore.scores[0])
                nextButton.gameObject.SetActive(true);
             
        }
    }
}