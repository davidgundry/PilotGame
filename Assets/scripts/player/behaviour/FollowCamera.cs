using UnityEngine;
using System.Collections;

using level;

namespace player.behaviour
{
    [RequireComponent(typeof(Camera))]
    public class FollowCamera : MonoBehaviour
    {

        public Transform Target {get; set;}
        public Vector3 Offset {get; set;}
        public float MinY {get; set;}
        public float MaxY {get; set;}
        public float MinX { get; set; }

        public Camera Camera { get; private set; }

        void Awake()
        { 
            Camera = GetComponent<Camera>();
        }

        public void Create(Transform target)
        {
            gameObject.name = "Camera";
            Target = target;
            Camera.orthographic = true;
            Camera.orthographicSize = 15;
            Camera.clearFlags = CameraClearFlags.SolidColor;
            Camera.backgroundColor = new Color(0.75f,0.93f,0.98f,1);
            Offset = new Vector3(Camera.orthographicSize/1.5f, 0, -10);
        }

        public void SetCameraBounds(LevelBounds levelBounds)
        {
            MinY = levelBounds.BottomEdge;
            MaxY = levelBounds.HardMaxHeight;
            MinX = levelBounds.LeftEdge + Camera.orthographicSize*Camera.aspect;
        }

        void Update()
        {
            transform.position = new Vector3(Mathf.Max(Target.position.x + Offset.x,MinX), Mathf.Min(Mathf.Max(Target.position.y + Offset.y, MinY),MaxY), Target.position.z + Offset.z);
        }
    }
}
