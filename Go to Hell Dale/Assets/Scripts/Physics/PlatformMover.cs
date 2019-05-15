using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    public List<GameObject> PivotPoints = new List<GameObject>();
    public List<float> PauseAtPoints = new List<float>();
    private GameObject CurrentPivotTarget;
    public float PlatformSpeed = 10f;
    public GameObject PlatformToMove;
    public float DistanceToPivotGrace = .1f;
    public bool BeginMovingOnSpawn = true;

    public float RotationAmount = 0;

    // Start is called before the first frame update
    void Start()
    {
        OnPlatformArrived += PlatformHasArrived;

        if (BeginMovingOnSpawn)
            CurrentPivotTarget = PivotPoints[0];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (PlatformToMove != null && CurrentPivotTarget != null)
        {
            float step = PlatformSpeed * Time.deltaTime; // calculate distance to move
            PlatformToMove.transform.position = Vector3.MoveTowards(PlatformToMove.transform.position, CurrentPivotTarget.transform.position, step);

            if (Vector3.Distance(PlatformToMove.transform.position, CurrentPivotTarget.transform.position) < DistanceToPivotGrace)
                OnPlatformArrived();
        }

        if (RotationAmount > 0 || RotationAmount < 0)
        {
            this.transform.Rotate(Vector3.forward, RotationAmount * Time.deltaTime, Space.Self);
        }
    }


    public delegate void PlatformArrived();
    public PlatformArrived OnPlatformArrived;
    private void PlatformHasArrived ()
    {
        int indexOfCurrent = PivotPoints.IndexOf(CurrentPivotTarget);

        if (indexOfCurrent + 1 >= PivotPoints.Count)
            indexOfCurrent = 0;
        else
            indexOfCurrent++;

        //CurrentPivotTarget = PivotPoints[indexOfCurrent];
        StartCoroutine(WaitBeforeDeparture(indexOfCurrent));
    }

    IEnumerator WaitBeforeDeparture (int index)
    {
        if (PauseAtPoints.Count > index)
            yield return new WaitForSeconds(PauseAtPoints[index]);
        else
            yield return null;

        CurrentPivotTarget = PivotPoints[index];
    }
}
