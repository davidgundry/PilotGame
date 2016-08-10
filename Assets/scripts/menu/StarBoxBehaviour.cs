using UnityEngine;
using System.Collections;
using player.data;

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

        private void Create()
        {          
            int padding = 15;
            stars = new StarBehaviour[3];
            for (int i = 0; i < 3; i++)
            {
                GameObject g = Instantiate(prefabStar);
                g.transform.SetParent(this.transform,false);

                RectTransform rt = g.GetComponent<RectTransform>();
                rt.anchorMin = new Vector2(0, 0.5f);
                rt.anchorMax = new Vector2(0, 0.5f);
                rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, i * (rt.rect.width + padding), 100);

                g.name = "star_"+i;
                stars[i] = g.GetComponent<StarBehaviour>();
                stars[i].SetStar(false);
            }
        }

        public void Refresh(StarScore starScore)
        {
            if (stars == null)
            {
                Create();
            }
            starCount = starScore.stars;
            for (int i = 0; i < 3; i++)
            {
                if (i < starCount)
                    stars[i].SetStar(true);
                else
                    stars[i].SetStar(false);
            }
        }
    }
}