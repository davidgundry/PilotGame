using UnityEngine;
using player.behaviour;

namespace level.behaviours
{
    public class HoopEdgeBehaviour : MonoBehaviour {

        HoopBehaviour parentHoop;

        void Start()
        {
            parentHoop = transform.parent.GetComponentInChildren<HoopBehaviour>();
        }

        public void OnTriggerEnter2D(Collider2D collider)
        {
            parentHoop.HitHoopEdge(collider);
        }
    }
}
