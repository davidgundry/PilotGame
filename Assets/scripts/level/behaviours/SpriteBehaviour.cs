using UnityEngine;
using System.Collections;
using level.data;
using player.behaviour;

namespace level.behaviours
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteBehaviour : MonoBehaviour
    {
        private Vector3 Movement { get; set; }
        private bool rock = false;

        public void Create(SpriteData spriteData)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = Resources.Load<Sprite>("sprites/" + spriteData.filePath);
            if (spriteData.filePath == "balloon")
                rock = true;

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
            gameObject.tag = "Sprite";

            if (rock)
                transform.Rotate(Vector3.forward, -7.5f);
        }

        void Update()
        {
            transform.position += Movement;
            if (rock)
                transform.Rotate(Vector3.forward, Mathf.Sin(Time.time*2)/6);
        }

        public void OnTriggerEnter2D(Collider2D collider)
        {
            Movement = new Vector3(0, 0.2f, 0);
            PlaneController pc = collider.GetComponent<PlaneController>();
            if (pc != null)
                pc.SpriteCrash();
        }
    }
}