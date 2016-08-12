using UnityEngine;
using System.Collections;
using level.data;
using player.behaviour;

namespace level.behaviours
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class HoopBehaviour : MonoBehaviour
    {
        private bool hitEdge = false;
        private bool throughHoop = false;
        public ParticleSystem particleSystem;

        public void Create(HoopData hoopData)
        {
            transform.parent.position = new Vector3(hoopData.position.x, hoopData.position.y, 0.2f);
            transform.parent.rotation = Quaternion.Euler(new Vector3(0, 0, hoopData.rotation));
            transform.parent.localScale = new Vector3(hoopData.scale.x, hoopData.scale.y, 1f);

            gameObject.name = hoopData.name;
        }

        public void OnTriggerEnter2D(Collider2D collider)
        {
            ThroughHoop(collider);
            particleSystem.Play();
        }

        private void ThroughHoop(Collider2D collider)
        {
            if ((!hitEdge) && (!throughHoop))
            {
                PlaneController pc = collider.GetComponent<PlaneController>();
                if (pc != null)
                {
                    pc.ThroughHoop();
                    throughHoop = true;
                }
            }
        }

        public void HitHoopEdge(Collider2D collider)
        {
            PlaneController pc = collider.GetComponent<PlaneController>();
            if (pc != null)
            {
                pc.HitHoopEdge();
                hitEdge = true;
            }
        }

    }
}