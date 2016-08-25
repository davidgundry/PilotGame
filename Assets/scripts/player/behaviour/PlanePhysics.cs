using UnityEngine;

namespace player.behaviour
{
    public class PlanePhysics
    {

        private const float minXSpeed = 0f; //0.5f
        private const float maxYVelocity = 100f;
        private const float minYVelocity = -100f;
        private const float maxSpeed = 14;

        // For aeroplane physics
        private const float density = 8;
        private const float angle = 3;

        public float SpeedMultiplier { get; set; }

        public PlanePhysics()
        {
            SpeedMultiplier = 1;
        }

        public Vector2 PassiveForce(Vector2 velocity, float deltaTime, float angle) //Vector2 position
        {
            float ad = 50;// (velocity.magnitude / SpeedMultiplier) * density;
            float vv = (velocity.magnitude / SpeedMultiplier) * angle;
            float lift = ad * vv;// * (1 / (position.y + 1));
            return (new Vector2(0, lift) * deltaTime);
        }

        public Vector2 BoundVelocity(Vector2 velocity)
        {
            if (velocity.magnitude > maxSpeed)
            {
                float scale = maxSpeed / velocity.magnitude;
                velocity.Scale(new Vector2(scale, scale));
            }
            return velocity;
            //return new Vector2(Mathf.Max(velocity.x, minXSpeed * SpeedMultiplier), Mathf.Max(Mathf.Min(velocity.y, maxYVelocity),minYVelocity));
        }

    }
}