using UnityEngine;

namespace menu.experiment
{
    public class ExperimentMenuBehaviour : MonoBehaviour
    {

        public void StartExperiment()
        {

        }

        public void SkipToGame()
        {
            Application.LoadLevel("main-menu");
        }
    }
}