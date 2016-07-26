using UnityEngine;
using System.Collections;
using level.data;
using player;

namespace level.behaviours
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class PickupBehaviour : MonoBehaviour
    {
        private PickupType pickupType;

        public void Create(PickupData pickupData)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = Resources.Load<Sprite>("pickups/" + pickupData.pickupType.ToString());

            transform.position = new Vector3(pickupData.position.x,pickupData.position.y,-0.2f);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, pickupData.rotation));
            transform.localScale = new Vector3(1.5f, 1.5f, 1);

            gameObject.name = pickupData.name;

            BoxCollider2D bc2d = gameObject.GetComponent<BoxCollider2D>();
            bc2d.isTrigger = true;
            bc2d.size = new Vector2(2f, 2f);

            gameObject.tag = "Pickup";
            pickupType = pickupData.pickupType;
        }

        public void OnTriggerEnter2D(Collider2D collider)
        {
            PlaneController pc = collider.GetComponent<PlaneController>();
            if (pc != null)
                pc.GetPickup(pickupType);
            Destroy(this.gameObject);
        }

    }
}