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

        void Awake()
        {
            planePhysics = new PlanePhysics();
            playerInputManager = GetComponent<PlayerInputManager>();
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            PhysicalForces();
            PlayerForces();
            RotateToDirection();
        }

        private void PhysicalForces()
        {
            rb.AddForce(planePhysics.PassiveForce(rb.velocity, Time.deltaTime));
            rb.velocity = planePhysics.BoundVelocity(rb.velocity);
        }

        private void PlayerForces()
        {
            rb.AddForce(playerInputManager.PlayerForce(planePhysics,Time.deltaTime));
        }

        private void RotateToDirection()
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
            transform.Rotate(Vector3.up, -90);
        }

    }
}
