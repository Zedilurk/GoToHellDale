using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LevelManager))]
public class PlayerManager : MonoBehaviour {

    public GameObject PlayerPrefab;
    public GameObject PlayerCameraPrefab;

    private GameObject Player;
    private GameObject PlayerCamera;

    private LevelManager LevelManager;

    public int PlayersToSpawn = 1;
    public List<Player> Players = new List<Player>();

    private bool spawn1InUse = false;
    private bool spawn2InUse = false;

    // Use this for initialization
    void Awake () {
        LevelManager = GetComponent<LevelManager>();

        while (Players.Count < PlayersToSpawn)
            SpawnPlayer(LevelManager.Spawn);
	}
	
	// Update is called once per frame
	void Update ()
    {
        HandleSpawnLogic();
    }

    private void HandleSpawnLogic ()
    {
        if (Input.GetAxisRaw("Jump_1") == 1)
            if (!spawn1InUse)
                if (Players.Count == 0)
                {
                    SpawnPlayer(LevelManager.Spawn);
                    spawn1InUse = true;
                }

        if (Input.GetAxisRaw("Jump_1") == 0)
            spawn1InUse = false;


        if (Input.GetAxisRaw("Jump_2") == 1)
            if (!spawn2InUse)
                if (Players.Count == 0)
                {
                    SpawnPlayer(LevelManager.Spawn);
                    spawn2InUse = true;
                }

        if (Input.GetAxisRaw("Jump_2") == 0)
            spawn2InUse = false;
    }

    private void SpawnPlayer(GameObject checkpoint)
    {
        if (checkpoint)
        {
            Player = Instantiate(PlayerPrefab, GameObject.FindGameObjectWithTag("Spawn").transform.position, new Quaternion());

            if (!PlayerCamera)
            {
                PlayerCamera = Instantiate(PlayerCameraPrefab, checkpoint.transform.position + new Vector3(0, 0, -5), new Quaternion());
                //PlayerCamera.GetComponent<CameraFollow2D>().Targets.Add(Player.transform);
                PlayerCamera.GetComponent<CameraSystem>().Player = Player.transform;
                PlayerCamera.name = "PlayerCamera";
            }

            Player.GetComponent<Player>().PlayerState = global::Player.PlayerStateEnum.Idle;
            Players.Add(Player.GetComponent<Player>());
            Player.GetComponent<PlayerInput>().SetPlayerNumber(Players.Count);
            Player.name = "Player";
        }
        else
        {
            Player = Instantiate(PlayerPrefab, Vector3.zero, new Quaternion());

            if (!PlayerCamera)
            {
                PlayerCamera = Instantiate(PlayerCameraPrefab, new Vector3(0, 0, -5), new Quaternion());
                //PlayerCamera.GetComponent<CameraFollow2D>().Targets.Add(Player.transform);
                PlayerCamera.GetComponent<CameraSystem>().Player = Player.transform;
                PlayerCamera.name = "PlayerCamera";
            }

            Player.GetComponent<Player>().PlayerState = global::Player.PlayerStateEnum.Idle;
            Players.Add(Player.GetComponent<Player>());
            Player.GetComponent<PlayerInput>().SetPlayerNumber(Players.Count);
            Player.name = "Player";
        }
    }
}
