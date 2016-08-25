using UnityEngine;
using System.Collections;

using level;

namespace player.behaviour
{
    [RequireComponent(typeof(Camera))]
    public class FollowCamera : MonoBehaviour
    {

        private const float springConstantX = 2f;
        private const float springConstantY = 0.5f;
        private const float damping = 0.7f;

        public Transform Target {get; set;}
        public Vector3 Offset {get; set;}
        public float MinY {get; set;}
        public float MaxY {get; set;}
        public float MinX { get; set; }

        private float yVelocity = 0;
        private float xVelocity = 0;

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
            //Camera.orthographicSize = 15;

            Camera.orthographicSize = ((13f/10f)*16f / Screen.width) * Screen.height;

            Camera.clearFlags = CameraClearFlags.SolidColor;
            Camera.backgroundColor = new Color(0.75f,0.93f,0.98f,1);
            Offset = new Vector3(Camera.orthographicSize/1.5f, 0, -10);

            transform.position = Target.position + Offset;
        }

        public void SetCameraBounds(LevelBounds levelBounds)
        {
            MinY = levelBounds.BottomEdge + Camera.orthographicSize;
            MaxY = levelBounds.HardMaxHeight - Camera.orthographicSize + 2;
            MinX = levelBounds.LeftEdge + Camera.orthographicSize*Camera.aspect;
        }

        void FixedUpdate()
        {
            float currentX = transform.position.x;
            float currentY = transform.position.y;
            

            float dy = currentY - (Target.position.y + Offset.y);
            float force = -dy * springConstantY;
            yVelocity += force;
            yVelocity *= damping;

            float dx = currentX - (Target.position.x + Offset.x);
            force = -dx * springConstantX;
            xVelocity += force;
            xVelocity *= damping;

            float x = Mathf.Max(currentX + xVelocity * Time.deltaTime, MinX);
            float y = Mathf.Min(Mathf.Max(currentY + yVelocity * Time.deltaTime, MinY), MaxY);

            transform.position = new Vector3(x, y, Target.position.z + Offset.z);
        }
    }
}
