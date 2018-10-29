using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectRainer : MonoBehaviour
{
    public float SpawnRate = 0.1f; //spawn every 0.5 seconds
    private float nextSpawn = 0f;

    public bool UseRandomSpawnDirection = false;
    public Vector2 SpawnDirection;

    public GameObject rainObject;
    public Transform GeneratorBounds;

    public int PoolSize = 100;
    private List<GameObject> RainObjectPool = new List<GameObject>();



    // Use this for initialization
    void Start()
    {
        while (RainObjectPool.Count < PoolSize)
        {
            GameObject rainObj = Instantiate(rainObject, Vector3.zero, transform.rotation);
            rainObj.transform.parent = transform;
            RainObjectPool.Add(rainObj);
            rainObj.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextSpawn)
        {
            nextSpawn = Time.time + SpawnRate;
            Vector3 spawnBox = GeneratorBounds.localScale;
            Vector3 position = new Vector3(Random.value * spawnBox.x + GeneratorBounds.position.x, Random.value * spawnBox.y + GeneratorBounds.position.y, GeneratorBounds.position.z);
            position = transform.TransformPoint(position - spawnBox / 2);
            GameObject obj = GetObjectFromPool();
            obj.transform.position = position;

            if (!UseRandomSpawnDirection)
                obj.GetComponent<Rain>().Direction = SpawnDirection;
            else
                obj.GetComponent<Rain>().Direction = new Vector2(Random.Range(-1f,1f), -1);

        }
    }

    private GameObject GetObjectFromPool()
    {
        GameObject spawn = RainObjectPool.FirstOrDefault();
        RainObjectPool.Remove(spawn);
        RainObjectPool.Add(spawn);
        spawn.SetActive(true);
        return spawn;
    }
}
