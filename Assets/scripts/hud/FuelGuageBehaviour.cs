using UnityEngine;
using System.Collections;

namespace hud
{
    public class FuelGuageBehaviour : MonoBehaviour
    {

        public RectTransform fuelLevel;

        void Start()
        {
            Fill();
        }

        private void SetLevel(float y)
        {
            fuelLevel.offsetMax = new Vector2(fuelLevel.offsetMax.x, y);
        }

        private float ProportionToLevel(float proportion)
        {
            return -40 - (1 - proportion) * 300;
        }

        public void Fill()
        {
            SetLevel(ProportionToLevel(1));
        }

        public void Empty()
        {
            SetLevel(ProportionToLevel(0));
        }

        public void SetProportion(float proportion)
        {
            SetLevel(ProportionToLevel(proportion));
        }

    }
}
