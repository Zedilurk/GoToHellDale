using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject Spawn = null;
    public GameObject LastCheckpoint = null;
    public GameObject InputManagerPrefab = null;
    public GameObject DebugMenuPrefab = null;
    private GameObject InputManager;
    private GameObject DebugMenu;

    // Use this for initialization
    void Awake () {
        Spawn = GameObject.FindGameObjectWithTag("Spawn");
        LastCheckpoint = Spawn;
    }

    void Start()
    {
        InputManager = GameObject.Find("Input Manager");
        DebugMenu = GameObject.Find("DebugMenu");

        if (InputManager == null)
            InputManager = Instantiate(InputManagerPrefab);

        if (DebugMenu == null)
            DebugMenu = Instantiate(DebugMenuPrefab);

        if (InputManager != null)
            InputManager.name = "Input Manager";

        if (DebugMenu != null)
            DebugMenu.name = "DebugMenu";
    }

    // Update is called once per frame
    void Update () {
		
	}

    

}
