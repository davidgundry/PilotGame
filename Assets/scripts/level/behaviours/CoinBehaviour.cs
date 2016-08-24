using UnityEngine;
using System.Collections;
using level.data;
using player.behaviour;

namespace level.behaviours
{
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class CoinBehaviour : MonoBehaviour
    {
        public ParticleSystem particleSystem;
        private bool pickedUp = false;

        public void OnTriggerEnter2D(Collider2D collider)
        {
            if (!pickedUp)
            {
                PlaneController pc = collider.GetComponent<PlaneController>();
                if (pc != null)
                    pc.GetPickup(PickupType.Coin);
                Destroy(this.gameObject, 0.5f);
                GetComponent<SpriteRenderer>().enabled = false;
                particleSystem.emissionRate = 0;
                pickedUp = true;
            }
        }

    }
}