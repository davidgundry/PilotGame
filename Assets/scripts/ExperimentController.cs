using UnityEngine;
using System.Collections;

public class ExperimentController : MonoBehaviour {

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {

    }

    public void ExperimentEnd()
    {

    }

    public string DataKey()
    {
        return null;
    }
}
