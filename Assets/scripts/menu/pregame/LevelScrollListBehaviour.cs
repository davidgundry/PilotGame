using UnityEngine;

namespace menu.pregame
{
    public class LevelScrollListBehaviour : MonoBehaviour
    {
        public LevelScrollItemBehaviour levelScrollItemPrefab;

        public void Create(LevelListData[] levelListData)
        {
            float padding = 10;

            if (levelScrollItemPrefab != null)
            {
                float insetFromTop = padding/2;
                float height = levelScrollItemPrefab.GetComponent<RectTransform>().rect.height;
                foreach (LevelListData lld in levelListData)
                {
                    LevelScrollItemBehaviour lsi = GameObject.Instantiate(levelScrollItemPrefab);
                    lsi.Create(lld);
                    
                    RectTransform rt = lsi.GetComponent<RectTransform>();
                    rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, insetFromTop, height);
                    rt.SetParent(this.transform,false);
                    insetFromTop += height + padding;
                }
                SetHeightOfScrollBox(insetFromTop);
                
            }
            else
                throw new System.MemberAccessException("levelScrollItemPrefab not assigned in inspector");
        }


        private void SetHeightOfScrollBox(float height)
        {
            GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, height);
        }

    }
}