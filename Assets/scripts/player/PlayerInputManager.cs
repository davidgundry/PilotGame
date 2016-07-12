using UnityEngine;
using System.Collections;
using MicTools;

namespace player
{
    [RequireComponent(typeof(MicrophoneController))]
    [RequireComponent(typeof(MicrophoneInput))]
    public class PlayerInputManager : MonoBehaviour
    {

        MicrophoneInput microphoneInput;

        void Awake()
        {
            microphoneInput = GetComponent<MicrophoneInput>();
        }

        public Vector2 PlayerForce(PlanePhysics planePhysics, float deltaTime)
        {
            return KeyboardInput(planePhysics, deltaTime);
            //return MicrophoneLevelInput(planePhysics,deltaTime);
        }

        private Vector2 SyllableInput(PlanePhysics planePhysics, float forceTime, float deltaTime)
        {
            if (forceTime > 0)
            {
                if (forceTime > 0.1f)
                    return new Vector2(1f, 0) * 500 * planePhysics.SpeedMultiplier * deltaTime;
                else
                    return new Vector2(1f, 0) * 350 * planePhysics.SpeedMultiplier * deltaTime;
            }

            return new Vector2(0, 0);
        }

        private Vector2 KeyboardInput(PlanePhysics planePhysics, float deltaTime)
        {
            if (Input.GetKeyDown("space"))
                return (new Vector2(100f, 0) * 50 * deltaTime);
            return new Vector2(0, 0);
        }

        private Vector2 MicrophoneLevelInput(PlanePhysics planePhysics, float deltaTime)
        {
            if (microphoneInput.Level > 0.005f)
                return (new Vector2(Mathf.Sqrt(microphoneInput.Level * 10000), 0) * 50 * planePhysics.SpeedMultiplier * deltaTime);
            return new Vector2(0, 0);
        }
    }
}
