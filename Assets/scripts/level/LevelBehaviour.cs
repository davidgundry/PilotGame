using UnityEngine;
using System.Collections;
using level.data;

namespace level
{
    public class LevelBehaviour : MonoBehaviour
    {
        GeomBehaviour[] geomBehaviours;
        SpriteBehaviour[] spriteBehaviours;

        public void CreateLevel(LevelData levelData)
        {
            CreateGeom(levelData.geomData);
            CreateSprites(levelData.spriteData);
        }

        private void CreateGeom(GeomData[] geomData)
        {
            geomBehaviours = new GeomBehaviour[geomData.Length];
            for (int i = 0; i < geomData.Length; i++)
            {
                GameObject g = new GameObject();
                g.AddComponent<GeomBehaviour>();
                g.GetComponent<GeomBehaviour>().Create(geomData[i]);
                g.name = geomData[i].name;
                geomBehaviours[i] = g.GetComponent<GeomBehaviour>();
            }
        }

        private void CreateSprites(SpriteData[] spriteData)
        {
            spriteBehaviours = new SpriteBehaviour[spriteData.Length];
            for (int i = 0; i < spriteData.Length; i++)
            {
                GameObject g = new GameObject();
                g.AddComponent<SpriteBehaviour>();
                g.GetComponent<SpriteBehaviour>().Create(spriteData[i]);
                g.name = spriteData[i].name;
                spriteBehaviours[i] = g.GetComponent<SpriteBehaviour>();
            }
        }
    }
}