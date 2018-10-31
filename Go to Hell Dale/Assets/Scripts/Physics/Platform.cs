using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    Controller2D Player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
            Player = other.gameObject.GetComponent<Controller2D>();
    }

    void OnTriggerStay2D (Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        { 
            if (Player.collisions.below)
            {
                Debug.Log("Collisions below");

                if (Player.transform.parent == null)
                    Player.transform.parent = transform;
            }
            else
            {
                if (Player.transform.parent != null)
                    Player.transform.parent = null;
            }
        }
    }

    void OnTriggerExit2D (Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Player.transform.parent != null)
                Player.transform.parent = null;

            Player = null;
        }
    }
}
