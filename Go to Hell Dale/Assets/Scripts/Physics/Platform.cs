using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Platform : MonoBehaviour {

    Controller2D Player;
    List<Enemy> Enemies = new List<Enemy>();

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
        if (other.gameObject.tag == "Enemy")
            if (!Enemies.Contains(other.gameObject.GetComponent<Enemy>()))
            {
                Enemies.Add(other.gameObject.GetComponent<Enemy>());
                Debug.Log("Enemy added");
            }
                
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
                if (Player.transform.parent == null)
                    Player.transform.parent = null;
            }
        }
        else if (other.gameObject.tag == "Enemy")
        {
            Enemy enemy = Enemies.FirstOrDefault(x => x.gameObject == other.gameObject);

            if (enemy.transform.parent == null)
                enemy.transform.parent = transform;            
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
        else if (other.gameObject.tag == "Enemy")
        {
            Enemy enemy = Enemies.FirstOrDefault(x => x.gameObject == other.gameObject);

            if (!enemy.StayOnPlatformsIndefinitely)
            {
                if (enemy.transform.parent != null)
                    enemy.transform.parent = null;

                Enemies.Remove(enemy);
            }
                
        }
    }
}
