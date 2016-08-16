using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace experiment.menu
{
    public class UploadBar : MonoBehaviour
    {
        public Image proportionCompleteBar;
        private int startingCachedFiles;

        public void SetStartingCachedFiles(int count)
        {
            startingCachedFiles = count;
        }

        public void SetRemainingCachedFiles(int count)
        {
            SetProportion(count / (float) startingCachedFiles);
        }

        public void SetProportion(float proportion)
        {
            float fullWidth = GetComponent<RectTransform>().rect.width;
            RectTransform rt = proportionCompleteBar.GetComponent<RectTransform>();
            float width = 0;

            width = proportion * fullWidth;
            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, width);
        }
    }
}