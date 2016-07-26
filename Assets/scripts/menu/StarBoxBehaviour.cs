using UnityEngine;
using System.Collections;

namespace menu
{
    public class StarBoxBehaviour : MonoBehaviour
    {
        public GameObject prefabStar;

        private StarBehaviour[] stars;
        private int starCount;
        public int StarCount
        {
            get
            {
                return starCount;
            }
            set
            { 
                starCount = value;
                if (starCount > stars.Length)
                    starCount = stars.Length;
                if (starCount < 0)
                    starCount = 0;
                for (int i = 0; i < stars.Length; i++)
                    stars[i].SetStar(i + 1 <= starCount);
            } 
        }

        public void Create(int starCount)
        {
            int leftmargin = 30;
            int padding = 15;
            stars = new StarBehaviour[starCount];
            for (int i = 0; i < starCount; i++)
            {
                GameObject g = Instantiate(prefabStar);
                g.transform.SetParent(this.transform,false);

                RectTransform rt = g.GetComponent<RectTransform>();
                rt.anchorMin = new Vector2(0, 0.5f);
                rt.anchorMax = new Vector2(0, 0.5f);
                rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, leftmargin + i * (rt.rect.width + padding), 100);
                //rt.localPosition = new Vector2(i*(rt.rect.width+padding), 0);

                g.name = "star_"+i;
                stars[i] = g.GetComponent<StarBehaviour>();
            }
        }
    }
}