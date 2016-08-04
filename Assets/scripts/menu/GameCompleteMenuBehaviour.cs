using UnityEngine;
using System.Collections;

public class GameCompleteMenuBehaviour : MonoBehaviour {

    public void ContinueButton()
    {
        Application.LoadLevel("main-menu");
    }
}
