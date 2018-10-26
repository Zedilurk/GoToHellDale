using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public GameObject PlayerPrefab;
    public GameObject PlayerCameraPrefab;

    public GameObject LevelBottomRespawner;

    private GameObject Player;
    private GameObject PlayerCamera;

    public GameObject LastCheckpoint = null;

	// Use this for initialization
	void Start () {
        LastCheckpoint = GameObject.FindGameObjectWithTag("Spawn");
        SpawnAtCheckpoint(LastCheckpoint);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void SpawnAtCheckpoint (GameObject checkpoint)
    {
        if (checkpoint)
        {
            Player = Instantiate(PlayerPrefab, GameObject.FindGameObjectWithTag("Spawn").transform.position, new Quaternion());
            PlayerCamera = Instantiate(PlayerCameraPrefab, checkpoint.transform.position + new Vector3(0, 0, -5), new Quaternion());
            PlayerCamera.GetComponent<CameraFollow2D>().Target = Player.transform;
            Player.GetComponent<Player>().PlayerState = global::Player.PlayerStateEnum.Idle;
        }
        else
        {
            Player = Instantiate(PlayerPrefab, Vector3.zero, new Quaternion());
            PlayerCamera = Instantiate(PlayerCameraPrefab, new Vector3(0, 0, -5), new Quaternion());
            PlayerCamera.GetComponent<CameraFollow2D>().Target = Player.transform;
            Player.GetComponent<Player>().PlayerState = global::Player.PlayerStateEnum.Idle;
        }
    }

}
