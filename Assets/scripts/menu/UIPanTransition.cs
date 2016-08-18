using UnityEngine;
using System.Collections;

namespace menu
{
    public class UIPanTransition : MonoBehaviour
    {
        public UIPane[] panes;
        public RectTransform background;
        private RectTransform rt;
        private float paralax;
        
        public const float paneWidth = 1960;
        public int CurrentPane { get; private set; }

        public void Awake()
        {
            rt = GetComponent<RectTransform>();
        }

        public void Start()
        {
            Create(panes);
        }

        public void Create(UIPane[] panes)
        {
            this.panes = panes;

            if (panes.Length > 0)
            {
                panes[0].Refresh();
                float totalWidth = paneWidth * panes.Length;
                paralax = (background.rect.width - paneWidth) / (paneWidth * panes.Length);
                SetProgressBars();
                SetPositions();
            }
        }

        private void SetProgressBars()
        {
            int paneIndex = 0;
            foreach (UIPane pane in panes)
            {
                ProgressBarBehaviour progressBar = pane.GetProgressBar();
                if (progressBar != null)
                    progressBar.SetProportionComplete(paneIndex / (panes.Length - 1f));
                paneIndex++;
            }
        }

        private void SetPositions()
        {
            float inset = 0;
            foreach (UIPane pane in panes)
            {
                pane.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, inset, paneWidth);
                inset+=paneWidth;
            }
        }

        public void TransitionTo(int paneID)
        {
            if ((paneID >= 0) && (paneID < panes.Length))
            {
                CurrentPane = paneID;
                panes[paneID].Refresh();
                StartCoroutine(Transition(paneID, 0.3f));
            }
        }

        public void SkipTo(int paneID)
        {
            if ((paneID >= 0) && (paneID < panes.Length))
            {
                CurrentPane = paneID;
                panes[paneID].Refresh();
                StartCoroutine(Transition(paneID, 0f));
            }
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