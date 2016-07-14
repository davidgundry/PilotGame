using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace menu
{
    public class PauseMenuBehaviour : MonoBehaviour
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