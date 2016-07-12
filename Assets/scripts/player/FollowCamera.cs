using UnityEngine;
using System.Collections;

namespace player
{
    [RequireComponent(typeof(Camera))]
    public class FollowCamera : MonoBehaviour
    {

        public Transform Target {get; set;}
        public Vector3 Offset {get; set;}
        public float MinY {get; set;}
        public float MaxY {get; set;}
        public float MinX { get; set; }

        void Update()
        {
            transform.position = new Vector3(Mathf.Max(Target.position.x + Offset.x,MinX), Mathf.Min(Mathf.Max(Target.position.y + Offset.y, MinY),MaxY), Target.position.z + Offset.z);
        }
    }
}
