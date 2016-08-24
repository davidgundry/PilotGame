using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace menu
{
    public class WaitingBar : MonoBehaviour
    {

        public delegate void OnCompleteEventHandler();
        public event OnCompleteEventHandler OnComplete = delegate() { };

        public Image proportionCompleteBar;
        public bool Done { get; private set; }

        public void StartWaitingBar(float time)
        {
            StartCoroutine(WaitingBarCoroutine(time));
        }

        private IEnumerator WaitingBarCoroutine(float time)
        {
            float fullWidth = GetComponent<RectTransform>().rect.width;
            RectTransform rt = proportionCompleteBar.GetComponent<RectTransform>();
            float width = 0;

            float startTime = Time.time;
            float totalTime = time;
            float proportion = 0;

            while (width < fullWidth)
            {
                proportion = (Time.time - startTime) / totalTime;
                width = proportion * fullWidth;
                rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, width);
                yield return null;
            }
            OnComplete.Invoke();
        }
    }
}