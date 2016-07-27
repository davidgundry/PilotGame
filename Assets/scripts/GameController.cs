using UnityEngine;

public class GameController : MonoBehaviour
{

    public int levelID { get; set; }

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void StartGame()
    {
        levelID = 0;
        Application.LoadLevel("load");
    }

}
