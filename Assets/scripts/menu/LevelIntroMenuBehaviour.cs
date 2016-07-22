using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace menu
{
    public class LevelIntroMenuBehaviour : MonoBehaviour
    {
        public Image background;
        public Text text;

        void Start()
        {
            background.gameObject.SetActive(false);
            text.gameObject.SetActive(false);
        }

        public void Show()
        {
            background.gameObject.SetActive(true);
            text.gameObject.SetActive(true);
        }

        public void Hide()
        {
            background.gameObject.SetActive(false);
            text.gameObject.SetActive(false);
        }
    }
}