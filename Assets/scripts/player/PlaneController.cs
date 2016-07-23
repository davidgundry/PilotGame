using UnityEngine;
using System.Collections;
using hud;

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
        private bool destroyed;
        private bool controlBlocked;

        private float damage;
        private float Damage { get { return damage; } set { damage = value; if (damageGuage != null) damageGuage.SetProportion(1-(damage / maxDamage)); } }
        private const float maxDamage = 3;

        private float fuel;
        private float Fuel { get { return fuel; } set { fuel = value; if (fuelGuage != null) fuelGuage.SetProportion(value); } }
        private const float fuelLossRate = 0.04f;

        private float oldVelocity;
        private Coroutine interruptControlCoroutine;
        private Coroutine flashPlaneCoroutine;

        private bool upsideDown;

        public PlayerLevelData PlayerLevelData { get; set; }

        FuelGuageBehaviour fuelGuage;
        DamageGuageBehaviour damageGuage;

        void Awake()
        {
            planePhysics = new PlanePhysics();
            playerInputManager = GetComponent<PlayerInputManager>();
            rb = GetComponent<Rigidbody2D>();
            AutoPilot = false;
            fuelGuage = GameObject.FindObjectOfType<FuelGuageBehaviour>();
            damageGuage = GameObject.FindObjectOfType<DamageGuageBehaviour>();
            Fuel = 1;
        }

        void Start()
        {
            weightedOldPosition = transform.position -  new Vector3(1f, 0f, 0f);
            oldVelocity = rb.velocity.magnitude;
        }


        void Update()
        {
            CheckForGeomCrash();

            UpdatePlanePhysics();

            UpdateFuelUsed();

            UpdatePlayerLevelData();

            oldVelocity = rb.velocity.magnitude;
        }

        private void UpdateFuelUsed()
        {
            if (playerInputManager.IsInput())
            {
                float fuelUsed = Time.deltaTime * fuelLossRate;
                Fuel -= fuelUsed;
                PlayerLevelData.FuelUsed += fuelUsed;
            }
        }

        private void UpdatePlayerLevelData()
        {
            PlayerLevelData.Distance = transform.position.x;
        }

        private void UpdatePlanePhysics()
        {
            RotateToDirection();
            PhysicalForces();

            if (!HasCrashed() && (!controlBlocked))
            {
                ApplyPlayerTurning();

                if (!AutoPilot)
                    PlayerForces();
                else
                    rb.AddForce((new Vector2(30000f, 0) * Time.deltaTime));
            }
        }

        private void CheckForGeomCrash()
        {
            if (oldVelocity - rb.velocity.magnitude > 2f)
            {
                GeomCrash();
            }
        }

        public bool HasCrashed()
        {
            if (destroyed)
                return true;
            if (inOcean)
                return true;
            //if (rb.velocity.x < 0)
            //    return true;
            return false;
        }

        private void PhysicalForces()
        {
            Vector2 force = planePhysics.PassiveForce(rb.velocity, Time.deltaTime, angle);
            rb.AddRelativeForce(new Vector2(force.x, force.y));
            rb.velocity = planePhysics.BoundVelocity(rb.velocity);
            rb.AddForce(new Vector2(-rb.velocity.x*10, 0));
        }

        private void PlayerForces()
        {
            Vector2 force = playerInputManager.PlayerForce(planePhysics,Time.deltaTime);
            rb.AddRelativeForce(new Vector2(force.x, force.y));
        }

        private void ApplyPlayerTurning()
        {
           angle = playerInputManager.PlayerTurning();
        }

        private void RotateToDirection()
        {
            if ((destroyed) && (upsideDown))
                upsideDown = true;

            transform.rotation = Quaternion.LookRotation(transform.position - weightedOldPosition);
            transform.Rotate(Vector3.up, -90);

            if (rb.velocity.x < 0f)
                upsideDown = true;
            if (rb.velocity.x > 0f)
                upsideDown = false;

            if (upsideDown)
                transform.Rotate(Vector3.right, 180);

            weightedOldPosition = (weightedOldPosition * 6 + transform.position) / 7;
        }

        public void OceanCrash()
        {
            inOcean = true;
            rb.drag = 4;
        }

        public void SpriteCrash()
        {
            interruptControlCoroutine = StartCoroutine(InterruptControl(0.5f));
            TakeDamage(1);
            PlayerLevelData.Crashes++;
            rb.velocity = new Vector2(rb.velocity.x / 2, rb.velocity.y);
            oldVelocity = rb.velocity.magnitude;
        }

        public void GeomCrash()
        {
            interruptControlCoroutine = StartCoroutine(InterruptControl(0.5f));
            TakeDamage(1);
            PlayerLevelData.Crashes++;
        }

        private void TakeDamage(float damage)
        {
            Damage += damage;
            flashPlaneCoroutine = StartCoroutine(FlashPlane(1f));
            if (Damage >= maxDamage)
                PlaneDestroyed();
            PlayerLevelData.DamageTaken += damage;
        }

        private void PlaneDestroyed()
        {
            destroyed = true;
        }

        private IEnumerator InterruptControl(float seconds)
        {
            controlBlocked = true;
            yield return new WaitForSeconds(seconds);
            controlBlocked = false;
        }

        private IEnumerator FlashPlane(float duration)
        {
            float startTime = Time.time;
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            Color original = Color.white;
            Color changed = new Color(original.r,original.g,original.b,0.6f);

            while (Time.time < startTime + duration)
            {
                if (spriteRenderer.color == original)
                    spriteRenderer.color = changed;
                else
                    spriteRenderer.color = original;
                yield return new WaitForSeconds(0.1f);
            }

            spriteRenderer.color = original;
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if ((collider.gameObject.tag != "Sprite") && (collider.gameObject.tag != "Ocean"))
                if (upsideDown)
                    PlaneDestroyed();
        }

    }
}
