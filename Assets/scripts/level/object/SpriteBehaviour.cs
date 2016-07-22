using UnityEngine;
using System.Collections;
using level.data;
using player;

namespace level
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteBehaviour : MonoBehaviour
    {

        private Vector3 Movement { get; set; }

        public void Create(SpriteData spriteData)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = Resources.Load<Sprite>("sprites/" + spriteData.filePath);

            transform.position = spriteData.position;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, spriteData.rotation));
            transform.localScale = spriteData.scale;

            gameObject.name = spriteData.name;

            if (spriteData.collision != null)
            {
                gameObject.AddComponent<PolygonCollider2D>();
                gameObject.GetComponent<PolygonCollider2D>().points = spriteData.collision;
                gameObject.GetComponent<PolygonCollider2D>().isTrigger = true;
            }
        }

        void Update()
        {
            transform.position += Movement;
        }

        public void OnTriggerEnter2D(Collider2D collider)
        {
            Debug.Log("Trigger");
            Movement = new Vector3(0, 0.1f, 0);
            PlaneController pc = collider.GetComponent<PlaneController>();
            if (pc != null)
                pc.SpriteCrash();
        }
    }
}