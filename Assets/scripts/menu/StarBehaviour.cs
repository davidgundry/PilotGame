using UnityEngine;
using UnityEngine.UI;

namespace menu
{
    [RequireComponent(typeof(Image))]
    public class StarBehaviour : MonoBehaviour
    {
        private Image image;
        public Sprite fullStar;
        public Sprite emptyStar;

        void Awake()
        {
            image = GetComponent<Image>();
        }

        public void SetStar(bool filled)
        {
            if (filled)
                image.sprite = fullStar;
            else
                image.sprite = emptyStar;
        }
    }
}