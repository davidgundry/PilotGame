using UnityEngine;
using System.Collections;

namespace menu
{
    public class StarBoxBehaviour : MonoBehaviour
    {

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
            stars = new StarBehaviour[starCount];
            for (int i = 0; i < starCount; i++)
            {
                GameObject g = new GameObject();
                g.AddComponent<StarBehaviour>();
                g.name = "star_"+i;
                stars[i] = g.GetComponent<StarBehaviour>();
            }
        }
    }
}