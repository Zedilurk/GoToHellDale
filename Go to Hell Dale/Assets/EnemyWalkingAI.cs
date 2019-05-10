using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Seeker))]
public class EnemyWalkingAI : MonoBehaviour
{
    public List<Vector3> Waypoints = new List<Vector3>();
    private Path path;
    private AIPath pather;
    private Seeker seeker;
    public Transform target;

    public bool PathOperationHasBegun = false;

    // Start is called before the first frame update
    void Start()
    {
        seeker = this.GetComponent<Seeker>();
        seeker.pathCallback += OnPathComplete;
        pather = this.GetComponent<AIPath>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            target = GameObject.Find("Player").transform;

        if (!PathOperationHasBegun && target != null)
        {
            BeginPathToPlayer();
            PathOperationHasBegun = true;
        }
    }

    public void BeginPathToPlayer ()
    {
        path = seeker.StartPath(transform.position, target.position);
    }

    public void OnPathComplete(Path p)
    {
        Debug.Log("Yay, we got a path back. Did it have an error? " + p.error);
    }
}
