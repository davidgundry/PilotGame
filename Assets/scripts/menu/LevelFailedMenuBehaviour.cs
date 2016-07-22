using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using player;
namespace menu
{
    public class LevelFailedMenuBehaviour : MonoBehaviour
    {

        public Image background;
        public Text text;
        public ButtonContainerBehaviour buttonContainer;

        void Start()
        {
            background.gameObject.SetActive(false);
            text.gameObject.SetActive(false);
            buttonContainer.gameObject.SetActive(false);
        }

        public void Create(PlayerLevelData playerLevelData)
        {
            Show();
        }

        public void Show()
        {
            background.gameObject.SetActive(true);
            text.gameObject.SetActive(true);
            buttonContainer.gameObject.SetActive(true);
        }

        public void Hide()
        {
            background.gameObject.SetActive(false);
            text.gameObject.SetActive(false);
            buttonContainer.gameObject.SetActive(false);
        }
    }
}