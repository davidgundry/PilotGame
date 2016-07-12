using UnityEngine;
using System.Collections;
using level;
using level.data;

public class LevelBehaviour : MonoBehaviour {

    LevelData levelData;

    private void LoadLevel(string filePath)
    {
        levelData = new LevelData(new LevelFileLoader(filePath));
    }

    private void InstantaiteLevel()
    {

    }

}
