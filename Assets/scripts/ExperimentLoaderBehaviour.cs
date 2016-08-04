using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExperimentLoaderBehaviour : MonoBehaviour {

    public Image proportionCompleteBar;
    public Image fullBar;

    void Start()
    {
        StartCoroutine(LoadingBar());
    }

    private IEnumerator LoadingBar()
    {
        float fullWidth = fullBar.GetComponent<RectTransform>().rect.width;
        RectTransform rt = proportionCompleteBar.GetComponent<RectTransform>();
        float width = 0;

        float startTime = Time.time;
        float totalTime = 0;
        float proportion = 0;

        while (width < fullWidth)
        {
            proportion = (Time.time - startTime) / totalTime;
            width = proportion * fullWidth;
            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, width);
            yield return null;
        }
        Application.LoadLevel("main-menu");
    }
}
