using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public LevelManager LevelManager;
    public bool HasBeenFlagged = false;

    // Start is called before the first frame update
    void Start()
    {
        LevelManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<LevelManager>();
    }

    public void OnTriggerEnter2D (Collider2D other)
    {
        if (!HasBeenFlagged)
            if (other.gameObject.tag == "Player")
            {
                LevelManager.LastCheckpoint = this.gameObject;
                HasBeenFlagged = true;
            }
    }
}
