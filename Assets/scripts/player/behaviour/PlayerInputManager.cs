using UnityEngine;
using System.Collections;
using MicTools;
using experiment;
using experiment.level;

namespace player.behaviour
{
    [RequireComponent(typeof(MicrophoneController))]
    [RequireComponent(typeof(MicrophoneInput))]
    public class PlayerInputManager : MonoBehaviour
    {

        MicrophoneInput microphoneInput;
        ExperimentController experimentController;
        GameController gameController;

        void Awake()
        {
            experimentController = GameObject.FindObjectOfType<ExperimentController>();
            microphoneInput = GetComponent<MicrophoneInput>();
            
            gameController = GameObject.FindObjectOfType<GameController>();
            MicrophoneController microphoneController = GetComponent<MicrophoneController>();
            if (gameController.UsingMicrophone == false)
            {
                microphoneController.microphoneActive = false;
                microphoneController.enabled = false;
                microphoneInput.enabled = false;
            }
            else
                microphoneController.microphoneActive = true;
        }

        public Vector2 PlayerForce(PlanePhysics planePhysics, float deltaTime)
        {
            if (gameController.UsingMicrophone == true)
                return MicrophoneLevelInput(planePhysics, deltaTime);
            else if (gameController.UsingMicrophone == false)
                return KeyboardInput(planePhysics, deltaTime);
            return MicrophoneLevelInput(planePhysics, deltaTime) + KeyboardInput(planePhysics, deltaTime);
        }

        public bool IsInput()
        {
            if (gameController.UsingMicrophone == true)
                return (microphoneInput.Level > 0.003f);
            else if (gameController.UsingMicrophone == false)
                return (Input.GetKeyDown("space"));
            return (microphoneInput.Level > 0.003f) || (Input.GetKeyDown("space"));
        }

        public float PlayerTurning()
        {
            if (Input.GetKey("up"))
                return 26f;
            else if (Input.GetKey("down"))
                return -10f;

            return Input.acceleration.x*10 + 4;
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
                return (new Vector2(80000f, 0) * deltaTime);
            return new Vector2(0, 0);
        }

        private Vector2 MicrophoneLevelInput(PlanePhysics planePhysics, float deltaTime)
        {
            if (microphoneInput.Level > 0.003f)
                return (new Vector2(30000f, 0) * deltaTime);
            return new Vector2(0, 0);
        }
    }
}
