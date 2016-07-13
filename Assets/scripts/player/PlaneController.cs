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

        void Awake()
        {
            planePhysics = new PlanePhysics();
            playerInputManager = GetComponent<PlayerInputManager>();
            rb = GetComponent<Rigidbody2D>();
            
        }

        void Start()
        {
            weightedOldPosition = transform.position -  new Vector3(1f, 0f, 0f);
        }


        void Update()
        {
            RotateToDirection();
            PhysicalForces();
            PlayerForces();
        }

        private void PhysicalForces()
        {
            rb.AddForce(planePhysics.PassiveForce(rb.velocity, Time.deltaTime));
            rb.velocity = planePhysics.BoundVelocity(rb.velocity);
        }

        private void PlayerForces()
        {
            rb.AddForce(playerInputManager.PlayerForce(planePhysics,Time.deltaTime),ForceMode2D.Impulse);
        }

        private void RotateToDirection()
        {
            transform.rotation = Quaternion.LookRotation(transform.position - weightedOldPosition);
            transform.Rotate(Vector3.up, -90);

            weightedOldPosition = (weightedOldPosition * 6 + transform.position) / 7;
        }

    }
}
