using UnityEngine;
using System.Collections;
using level.data;
using level;

public class LevelStarter : MonoBehaviour {


	// Use this for initialization
	void Start () {
	    CreateLevel(LoadLevel("levels/test"));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private LevelData LoadLevel(string filePath)
    {
        return new LevelData(new LevelFileLoader(filePath));
    }

    private void CreateLevel(LevelData levelData)
    {
        GameObject g = new GameObject();
        g.AddComponent<LevelBehaviour>();
        g.GetComponent<LevelBehaviour>().CreateLevel(levelData);
    }

}
