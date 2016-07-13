using UnityEngine;

namespace level
{
    public class LevelBounds
    {

        public float LevelLength { get; private set; }
        public float SoftMaxHeight { get; private set; }
        public float HardMaxHeight { get; private set; }
        private float HalfCameraHeight { get; set; }
        private float HalfCameraWidth { get; set; }

        public float BottomEdge { get; private set; }
        public float LeftEdge { get; private set; }

        public float GeomTopEdge { get { return HardMaxHeight + HalfCameraHeight; } }
        public float GeomBottomEdge { get { return 0 - HalfCameraHeight; } }


        public LevelBounds(float levelLength, float bottomEdge, float leftEdge, float softMaxHeight, float hardMaxHeight, float halfCameraHeight, float halfCameraWidth)
        {
            this.LevelLength = levelLength;
            this.SoftMaxHeight = softMaxHeight;
            this.HardMaxHeight = hardMaxHeight;
            this.HalfCameraHeight = halfCameraHeight;
            this.HalfCameraWidth = halfCameraWidth;
            this.BottomEdge = bottomEdge;
            this.LeftEdge = leftEdge;
        }
        

        public Vector2 ApplyHardHeightLimit(Vector2 position)
        {
            return new Vector2(position.x, Mathf.Min(HardMaxHeight, position.y));
        }

        public void ApplySoftHeightLimit(Vector2 position, Rigidbody2D rigidbody2D, float deltaTime)
        {
            if (position.y > SoftMaxHeight)
                rigidbody2D.AddForce(new Vector2(0, -1*deltaTime));
        }

        public bool CheckLevelComplete(Vector2 position)
        {
            return (position.x >= LevelLength);
        }
    }
}