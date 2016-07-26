using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using player.data;

public class StarsContainerBehaviour : MonoBehaviour {

    public Text text;
    public Image[] stars;
    public Sprite star;
    public Sprite emptyStar;

    public void SetStars(StarScore starScore)
    {
        text.text = starScore.text;
        for (int i = 0; i < stars.Length; i++)
        {
            if (starScore.stars > i)
                stars[i].sprite = star;
            else
                stars[i].sprite = emptyStar;
        }
    }
}
