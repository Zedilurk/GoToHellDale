using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Runner : MonoBehaviour
{
    public List<GameObject> Waypoints = new List<GameObject>();
    private GameObject _CurrentWaypoint = null;
    public Enemy RunnerEnemy;

    public float MoveSpeed = 5f;
    public float GraceDistanceToWaypoint = .1f;

    public bool StartRunningImmediately = true;

    // Start is called before the first frame update
    void Start()
    {
        RunnerEnemy = GetComponentInChildren<Enemy>();

        if (StartRunningImmediately)
            _CurrentWaypoint = GetNextWaypoint();
    }

    void FixedUpdate()
    {
        if (_CurrentWaypoint != null)
        {
            if (Vector3.Distance(_CurrentWaypoint.transform.position, RunnerEnemy.transform.position) < GraceDistanceToWaypoint)
            {
                _CurrentWaypoint = GetNextWaypoint();
            }
            else
            {
                float step = MoveSpeed * Time.deltaTime;
                RunnerEnemy.transform.position = Vector3.MoveTowards(RunnerEnemy.transform.position, _CurrentWaypoint.transform.position, step);
            }
        }
    }

    GameObject GetNextWaypoint ()
    {
        if (_CurrentWaypoint == null)
            if (Waypoints.Count > 0)
                return Waypoints.FirstOrDefault();
            else
                return null;

        int index = Waypoints.IndexOf(_CurrentWaypoint);

        if (index < Waypoints.Count-1)
            return Waypoints[index + 1];
        else
        {
            if (Waypoints.Count > 0)
                return Waypoints.FirstOrDefault();
            else
                return null;
        }            
    }

    void RunToNextPoint ()
    {

    }
}
