using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject Spawn = null;
    public GameObject LastCheckpoint = null;
    public GameObject InputManagerPrefab = null;
    private GameObject InputManager;

	// Use this for initialization
	void Awake () {
        Spawn = GameObject.FindGameObjectWithTag("Spawn");
        LastCheckpoint = Spawn;
    }

    void Start()
    {
        InputManager = GameObject.Find("Input Manager");

        if (InputManager == null)
            InputManager = Instantiate(InputManagerPrefab);

        if (InputManager != null)
            InputManager.name = "Input Manager";
    }

    // Update is called once per frame
    void Update () {
		
	}

    

}
