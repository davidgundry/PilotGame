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
            spriteRenderer.sprite = Resources.Load<Sprite>("sprites/cloud" + Random.Range(1,3).ToString());

            transform.position = RandomCloudPosition(levelBounds);
            transform.localScale = RandomCloudScale();

            gameObject.name = "Cloud";
            Movement = new Vector3(environmentData.wind * Random.Range(1,3), 0, 0);
        }

        void Update()
        {
            this.transform.position += Movement*Time.deltaTime;
        }

        private Vector3 RandomCloudPosition(LevelBounds levelBounds)
        {
            return new Vector3(-10+(Random.value * (levelBounds.LevelLength+10)), ((Random.value+0.2f) * levelBounds.HardMaxHeight) + levelBounds.BottomEdge, 20);
        }

        private Vector3 RandomCloudScale()
        {
            float scale = (Random.value+0.8f) * 1.5f;
            return new Vector3(scale, scale, 1);
        }
    }
}