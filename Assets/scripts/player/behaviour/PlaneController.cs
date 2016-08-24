using UnityEngine;
using System.Collections;
using hud;
using player.data;

namespace player.behaviour
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerInputManager))]
    public class PlaneController : MonoBehaviour
    {
        public ParticleSystem smokeParticles;
        public ParticleSystem particleSystem;

        PlanePhysics planePhysics;
        PlayerInputManager playerInputManager;
        Rigidbody2D rb;
        Rigidbody2D RB { get { if (rb == null) rb = GetComponent<Rigidbody2D>(); return rb; } }

        Vector3 weightedOldPosition;
        public bool AutoPilot { get; set; }
        private float angle;
        public bool InOcean { get; private set; }
        private bool destroyed;
        private bool controlBlocked;

        private float damage;
        private float Damage { get { return damage; } set { damage = value; if (damageGuage != null) damageGuage.SetProportion(1-(damage / maxDamage)); } }
        private const float maxDamage = 1;

        private float fuel;
        private float Fuel { get { return fuel; } set { fuel = value; if (fuelGuage != null) fuelGuage.SetProportion(value); } }
        private const float fuelLossRate = 0.05f;

        private float oldSpeed;
        private Coroutine interruptControlCoroutine;
        private Coroutine flashPlaneCoroutine;

        private bool upsideDown;

        public PlayerLevelData PlayerLevelData { get; set; }

        FuelGuageBehaviour fuelGuage;
        DamageGuageBehaviour damageGuage;

        private Vector2 pauseVelocityHolder;
        private float speedBoostTimer;
        private const float speedBoostTimerMax = 2;

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
            oldSpeed = RB.velocity.magnitude;
            smokeParticles.Clear();
            smokeParticles.Stop();
        }


        void Update()
        {
            if (Input.GetKey("s"))
                speedBoostTimer = speedBoostTimerMax;

            CheckForGeomCrash();

            UpdatePlanePhysics();

            UpdateFuelUsed();

            UpdatePlayerLevelData();

            oldSpeed = RB.velocity.magnitude;
            if (speedBoostTimer > 0 )
                speedBoostTimer -= Time.deltaTime;
        }

        private void UpdateFuelUsed()
        {
            if (playerInputManager.IsInput() && (Fuel > 0))
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
                    RB.AddForce((new Vector2(30000f, 0) * Time.deltaTime));
            }
        }

        private void CheckForGeomCrash()
        {
            if (oldSpeed - RB.velocity.magnitude > 2f)
            {
                GeomCrash();
            }
        }

        public bool HasCrashed()
        {
            if (destroyed)
                return true;
            if (InOcean)
                return true;
            //if (rb.velocity.x < 0)
            //    return true;
            return false;
        }

        private void PhysicalForces()
        {
            Vector2 force = planePhysics.PassiveForce(RB.velocity, Time.deltaTime, angle);
            RB.AddRelativeForce(new Vector2(force.x, force.y));
            RB.velocity = planePhysics.BoundVelocity(RB.velocity);
            RB.AddForce(new Vector2(-RB.velocity.x*4, 0));
        }

        private void PlayerForces()
        {
            float boostMultiplier = 1;
            if (speedBoostTimer > 0.2)
                boostMultiplier = 20;
            else if (speedBoostTimer > 0)
                boostMultiplier = (1+speedBoostTimer) * 20;
            Vector2 force = playerInputManager.PlayerForce(planePhysics,Time.deltaTime);
            RB.AddRelativeForce(new Vector2(force.x * boostMultiplier, force.y));
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

            if (RB.velocity.x < 0f)
                upsideDown = true;
            if (RB.velocity.x > 0f)
                upsideDown = false;

            if (upsideDown)
                transform.Rotate(Vector3.right, 180);

            weightedOldPosition = (weightedOldPosition * 6 + transform.position) / 7;
        }

        public void OceanCrash()
        {
            InOcean = true;
            RB.drag = 4;
        }

        public void SpriteCrash()
        {
            interruptControlCoroutine = StartCoroutine(InterruptControl(0.5f));
            TakeDamage(1);
            PlayerLevelData.Crashes++;
            RB.velocity = new Vector2(RB.velocity.x / 2, RB.velocity.y);
            oldSpeed = RB.velocity.magnitude;
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
            smokeParticles.Play();
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
            if ((collider.gameObject.tag != "Sprite") && (collider.gameObject.tag != "Ocean") && (collider.gameObject.tag != "Hoop"))
                if (upsideDown)
                    PlaneDestroyed();
        }

        public void ThroughHoop()
        {
            PlayerLevelData.Hoops++;
        }

        public void HitHoopEdge()
        {
            TakeDamage(1);
            //flashPlaneCoroutine = StartCoroutine(FlashPlane(1f));
        }

        public void GetPickup(PickupType pickupType)
        {
            switch (pickupType)
            {
                case PickupType.Fuel:
                    PickedUpFuel();
                    break;
                case PickupType.Repair:
                    PickedUpRepair();
                    break;
                case PickupType.Speed:
                    PickedUpSpeed();
                    break;
                case PickupType.Coin:
                    PickedUpCoin();
                    break;
            }
        }


        private void PickedUpFuel()
        {
            PlayerLevelData.Pickups++;
            Fuel = 1;
        }

        private void PickedUpRepair()
        {
            PlayerLevelData.Pickups++;
            Damage = 0;
        }

        private void PickedUpSpeed()
        {
            PlayerLevelData.Pickups++;
            speedBoostTimer = speedBoostTimerMax;
        }

        private void PickedUpCoin()
        {
            PlayerLevelData.Coins++;
        }

        public void Freeze()
        {
            pauseVelocityHolder = RB.velocity;
            RB.Sleep();
            RB.freezeRotation = true;
            enabled = false;
            smokeParticles.Pause();
            particleSystem.Pause();
        }

        public void Unfreeze()
        {
            RB.WakeUp();
            RB.freezeRotation = false;
            enabled = true;
            RB.velocity = pauseVelocityHolder;
            particleSystem.Play();
        }
    }
}
