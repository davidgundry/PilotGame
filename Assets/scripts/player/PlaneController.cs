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

            CreateCamera();
        }

        private void CreateCamera()
        {
            GameObject g = new GameObject();
            g.AddComponent<FollowCamera>();
            FollowCamera camera = g.GetComponent<FollowCamera>();
            camera.Target = this.transform;
            camera.Offset = new Vector3(10, 2, -20);
            camera.MinY = 1.5f;
            camera.MaxY = 20;
            camera.MinX = 18.5f;
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
