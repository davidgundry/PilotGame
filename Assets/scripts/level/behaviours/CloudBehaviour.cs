using UnityEngine;
using System.Collections;
using level.data;
namespace level.behaviours
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class CloudBehaviour : MonoBehaviour
    {

        private Vector3 Movement { get; set; }

        public void Create(LevelBounds levelBounds, EnvironmentData environmentData)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = Resources.Load<Sprite>("sprites/cloud");

            transform.position = RandomCloudPosition(levelBounds);
            transform.localScale = RandomCloudScale();

            gameObject.name = "Cloud";
            Movement = new Vector3(environmentData.wind, 0, 0);
        }

        void Update()
        {
            this.transform.position += Movement*Time.deltaTime;
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