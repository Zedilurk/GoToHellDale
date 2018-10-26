using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRespawner : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            collision.gameObject.transform.position = GameObject.Find("Manager").GetComponent<LevelManager>().LastCheckpoint.transform.position;
        else
            Destroy(collision.gameObject);
    }
}
