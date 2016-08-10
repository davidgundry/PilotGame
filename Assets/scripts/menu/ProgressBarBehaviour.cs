using UnityEngine;
using UnityEngine.UI;

namespace menu
{
    public class ProgressBarBehaviour : MonoBehaviour
    {
        public Text text;
        public GameObject childBar;

        private float fullWidth;
        private RectTransform childRectTransform;
        private RectTransform textRect;

        void Awake()
        {
            childBar.transform.SetParent(this.transform, false);
            RectTransform rectTransform = GetComponent<RectTransform>();
            fullWidth = rectTransform.rect.width;

            childRectTransform = childBar.GetComponent<RectTransform>();
            childRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, rectTransform.rect.height);

            textRect = text.GetComponent<RectTransform>();
            textRect.SetParent(childBar.transform, false);

            SetProportionComplete(0);
        }

        public void SetProportionComplete(float proportionComplete)
        {
            int percentage = (int)(proportionComplete * 100);
            childRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, fullWidth * proportionComplete);
            text.text = percentage + "%";
            if (fullWidth * proportionComplete > textRect.rect.width)
            {
                textRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, textRect.rect.width);
                text.alignment = TextAnchor.MiddleRight;
            }
            else
            {
                textRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, childRectTransform.rect.width, textRect.rect.width);
                text.alignment = TextAnchor.MiddleLeft;
            }
        }

    }
}