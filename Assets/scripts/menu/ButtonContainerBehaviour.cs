using UnityEngine;
using System.Collections;

namespace menu
{
    public class ButtonContainerBehaviour : MonoBehaviour
    {

        public void Retry()
        {
            Application.LoadLevel(Application.loadedLevel);
        }

        public void Next()
        {

        }

        public void Resume()
        {

        }

        public void Menu()
        {

        }

    }
}