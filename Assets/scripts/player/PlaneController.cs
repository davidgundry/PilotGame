using UnityEngine;
using System.Collections;

namespace player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerInputManager))]
    public class PlaneController : MonoBehaviour
    {

        PlanePhysics planePhysics;
        PlayerInputManager playerInputManager;
        Rigidbody2D rb;

        Vector3 weightedOldPosition;
        public bool AutoPilot { get; set; }
        private float angle;
        private bool inOcean;

        void Awake()
        {
            planePhysics = new PlanePhysics();
            playerInputManager = GetComponent<PlayerInputManager>();
            rb = GetComponent<Rigidbody2D>();
            AutoPilot = false;
        }

        void Start()
        {
            weightedOldPosition = transform.position -  new Vector3(1f, 0f, 0f);
        }


        void Update()
        {
            if (!HasCrashed())
            {
                RotateToDirection();
                ApplyPlayerTurning();
                PhysicalForces();
                if (!AutoPilot)
                    PlayerForces();
                else
                    rb.AddForce((new Vector2(30000f, 0) * Time.deltaTime));
            }
        }

        public bool HasCrashed()
        {
            if (inOcean)
                return true;
            if (rb.velocity.x < 0)
                return true;
            return false;
        }

        private void PhysicalForces()
        {
            Vector2 force = planePhysics.PassiveForce(rb.velocity, Time.deltaTime, angle);
            rb.AddForce(new Vector2(transform.up.x * force.x, transform.up.y * force.y));
            rb.velocity = planePhysics.BoundVelocity(rb.velocity);
            rb.AddForce(new Vector2(-rb.velocity.x*2, 0));
        }

        private void PlayerForces()
        {
            Vector2 force = playerInputManager.PlayerForce(planePhysics,Time.deltaTime);
            rb.AddForce(new Vector2(transform.right.x * force.x, transform.right.y * force.y));
        }

        private void ApplyPlayerTurning()
        {
           angle = playerInputManager.PlayerTurning();

        }

        private void RotateToDirection()
        {
            transform.rotation = Quaternion.LookRotation(transform.position - weightedOldPosition);
            transform.Rotate(Vector3.up, -90);

            weightedOldPosition = (weightedOldPosition * 6 + transform.position) / 7;
        }

        public void OceanCrash()
        {
            inOcean = true;
            rb.drag = 4;
        }

    }
}
