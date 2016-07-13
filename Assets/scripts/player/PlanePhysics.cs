using UnityEngine;

namespace player
{
    public class PlanePhysics
    {

        private const float minXSpeed = 1f; //0.5f
        private const float maxYVelocity = 3.5f;
        private const float minYVelocity = -100f;

        // For aeroplane physics
        private const float density = 5;
        private const float angle = 5;

        public float SpeedMultiplier { get; set; }

        public PlanePhysics()
        {
            SpeedMultiplier = 1;
        }

        public Vector2 PassiveForce(Vector2 velocity, float deltaTime) //Vector2 position
        {
            float ad = (velocity.magnitude / SpeedMultiplier) * density;
            float vv = (velocity.magnitude / SpeedMultiplier) * angle;
            float lift = ad * vv;// * (1 / (position.y + 1));
            return (new Vector2(0, lift) * deltaTime);
        }

        public Vector2 BoundVelocity(Vector2 velocity)
        {
            return new Vector2(Mathf.Max(velocity.x, minXSpeed * SpeedMultiplier), Mathf.Max(Mathf.Min(velocity.y, maxYVelocity),minYVelocity));
        }

    }
}