using UnityEngine;
using System.Collections;

namespace level
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class CloudBehaviour : MonoBehaviour
    {

        public void Create(LevelBounds levelBounds)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = Resources.Load<Sprite>("sprites/cloud");

            transform.position = RandomCloudPosition(levelBounds);
            transform.localScale = RandomCloudScale();

            gameObject.name = "Cloud";
        }

        private Vector3 RandomCloudPosition(LevelBounds levelBounds)
        {
            return new Vector3(Random.value*levelBounds.LevelLength,(Random.value*levelBounds.HardMaxHeight)+levelBounds.BottomEdge,2);
        }

        private Vector3 RandomCloudScale()
        {
            float scale = Random.value * 3;
            return new Vector3(scale, scale, 1);
        }
    }
}