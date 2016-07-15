using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using player;
namespace menu
{
    public class LevelEndMenuBehaviour : MonoBehaviour
    {
        public StarsContainerBehaviour starsContainer;
        public StatsContainerBehaviour statsContainer;
        public ButtonContainerBehaviour buttonContainer;
        public Image background;

        void Awake()
        {
            starsContainer.gameObject.SetActive(false);
            statsContainer.gameObject.SetActive(false);
            buttonContainer.gameObject.SetActive(false);
            background.gameObject.SetActive(false);
        }

        public void Create(PlayerLevelData playerLevelData)
        {
            switch (playerLevelData.LevelResult)
            {
                case LevelResult.Complete:
                    StartCoroutine(LevelCompleteMenu(playerLevelData));
                    break;
                case LevelResult.FellOffBottom:
                    StartCoroutine(LevelFailedMenu(playerLevelData));
                    break;
                case LevelResult.Crash:
                    StartCoroutine(LevelFailedMenu(playerLevelData));
                    break;
            }
        }

        private IEnumerator LevelCompleteMenu(PlayerLevelData playerLevelData)
        {
            starsContainer.SetStars(playerLevelData.StarScore);
            starsContainer.gameObject.SetActive(true);
            background.gameObject.SetActive(true);
            RectTransform rt = starsContainer.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector3(0, 0, 0);
            
            yield return new WaitForSeconds(0.75f);

            while (rt.anchoredPosition.y < 250)
            {
                rt.anchoredPosition = new Vector3(rt.anchoredPosition.x, rt.anchoredPosition.y + Time.deltaTime * 200, 0);
                yield return null;
            }

            statsContainer.gameObject.SetActive(true);
            statsContainer.Create(playerLevelData);
            yield return new WaitForSeconds(1f);
            buttonContainer.gameObject.SetActive(true);
             
        }

        private IEnumerator LevelFailedMenu(PlayerLevelData playerLevelData)
        {

            yield return new WaitForSeconds(3f);
        }
    }
}