using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject Spawn = null;
    public GameObject LastCheckpoint = null;

	// Use this for initialization
	void Awake () {
        Spawn = GameObject.FindGameObjectWithTag("Spawn");
        LastCheckpoint = Spawn;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    

}
