using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossZoneCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject camera = Camera.main.gameObject;
            camera.GetComponent<CameraSystem>().SetZoneTarget(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject camera = Camera.main.gameObject;
            camera.GetComponent<CameraSystem>().ClearZoneTarget();
        }
    }
}
