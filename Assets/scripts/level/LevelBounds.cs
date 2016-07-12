using UnityEngine;

namespace level
{
    public class LevelBounds
    {

        private float levelLength;
        private float softMaxHeight;
        private float hardMaxHeight;

        public LevelBounds(float levelLength, float softMaxHeight, float hardMaxHeight)
        {
            this.levelLength = levelLength;
            this.softMaxHeight = softMaxHeight;
            this.hardMaxHeight = hardMaxHeight;
        }

        public Vector2 ApplyHardHeightLimit(Vector2 position)
        {
            return new Vector2(position.x, Mathf.Min(hardMaxHeight, position.y));
        }

        public void ApplySoftHeightLimit(Vector2 position, Rigidbody2D rigidbody2D, float deltaTime)
        {
            if (position.y > softMaxHeight)
                rigidbody2D.AddForce(new Vector2(0, -1*deltaTime));
        }

        public bool CheckLevelComplete(Vector2 position)
        {
            return (position.x >= levelLength);
        }
    }
}