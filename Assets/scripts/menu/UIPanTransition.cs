using UnityEngine;
using System.Collections;

namespace menu
{
    public class UIPanTransition : MonoBehaviour
    {
        public UIPane[] panes;
        public RectTransform background;
        private RectTransform rt;
        public float paralax;

        private float paneWidth = 1960;

        public void Awake()
        {
            rt = GetComponent<RectTransform>();
        }

        public void Start()
        {
            panes[0].Refresh();
            float totalWidth = paneWidth * panes.Length;
            paralax = (background.rect.width - paneWidth) / (paneWidth*panes.Length);
        }

        public void TransitionTo(int paneID)
        {
            panes[paneID].Refresh();
            StartCoroutine(Transition(paneID, 0.3f));   
        }

        private IEnumerator Transition(int paneID, float totalTime)
        {
            Vector2 origin = rt.localPosition;
            Vector2 target = new Vector2(-paneID * paneWidth, 0);
            float time = 0;
            while (time < totalTime)
            {
                time += Time.deltaTime;
                rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, Mathf.Lerp(origin.x, target.x, time / totalTime),rt.rect.width);
                background.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, Mathf.Lerp(origin.x, target.x, time / totalTime) * paralax, background.rect.width);
                yield return null;
            }
            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, target.x, rt.rect.width);
            background.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, target.x * paralax, background.rect.width);
        }
    }
}