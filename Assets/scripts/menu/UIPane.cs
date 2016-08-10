using UnityEngine;

namespace menu
{
    public class UIPane : MonoBehaviour
    {
        public ProgressBarBehaviour progressBar;
        public int PaneNumber { get; set; }

        public virtual void Refresh()
        {

        }

        public ProgressBarBehaviour GetProgressBar()
        {
            return progressBar;
        }

        public void ForwardButton()
        {
            GameObject.FindObjectOfType<UIPanTransition>().TransitionTo(PaneNumber + 1);
        }

        public void BackButton()
        {
            GameObject.FindObjectOfType<UIPanTransition>().TransitionTo(PaneNumber - 1);
        }

    }
}