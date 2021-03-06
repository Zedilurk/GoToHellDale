﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRespawner : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            collision.GetComponent<Player>().Death();
        else if (collision.gameObject.tag == "Enemy")
            collision.GetComponent<Enemy>().Death();
        //else
        //Destroy(collision.gameObject);
    }
}
