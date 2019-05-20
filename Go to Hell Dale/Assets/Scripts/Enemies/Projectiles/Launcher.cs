using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    public List<FiringGroup> FiringGroups = new List<FiringGroup>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class FiringGroup
{
    public List<Shot> Shots = new List<Shot>();

    public void AddShot ()
    {
        Shots.Add(new Shot());
    }
    public void RemoveShot (Shot shot)
    {
        if (Shots.Contains(shot))
            Shots.Remove(shot);
    }
}

public class Shot
{
    public Vector3 Direction { get; set; }
    public float Speed { get; set; }
    public ProjectileType ProjectileType { get; set; }
    public string TargetByTag { get; set; }
    public Transform Target { get; set; }
    public bool UseGravity { get; set; }

    public float Lifetime { get; set; }
    public bool OnlyImpactPlayer { get; set; }
}
