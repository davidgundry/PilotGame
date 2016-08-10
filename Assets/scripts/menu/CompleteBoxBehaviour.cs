using UnityEngine;
using UnityEngine.UI;

namespace menu
{
    public class CompleteBoxBehaviour : MonoBehaviour
    {
        public Text completeText;
        public Text numberText;
        public Text percentText;

        private int completePercentage;
        public int CompletePercentage
        {
            get
            {
                return completePercentage;
            }
            set
            {
                completePercentage = value;
                if (completePercentage > 100)
                    completePercentage = 100;
                if (completePercentage < 0)
                    completePercentage = 0;
                numberText.text = completePercentage.ToString();
            }
        }

        void Start()
        {
            GameController gameController = GameObject.FindObjectOfType<GameController>();
            if (gameController != null)
                CompletePercentage = gameController.PlayerGameProgress.CompletePercentage;
        }

    }
}