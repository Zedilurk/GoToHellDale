using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    private Vector3 Velocity = Vector3.zero;
    [SerializeField]
    private Vector3 _Offset = new Vector3(0, 4, 0);

    public float DampingTime = 0.15f;
    public float MaxLeadDistance = 2f;

    public List<Transform> Targets = new List<Transform>();

    public Vector3 Offset
    {
        get { return _Offset; }
        set { _Offset = value; }
    }

    Player _TargetPlayer;

    private void Start()
    {
        _TargetPlayer = Targets.FirstOrDefault().GetComponent<Player>();
    }

    private void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (Targets.Count > 0)
        {
            Vector3 centerPoint = FindCenterPoint(Targets);

            float leadDistance = _TargetPlayer.Velocity.x;
            float leadHeight = _TargetPlayer.Velocity.y;

            if (leadDistance > MaxLeadDistance)
                leadDistance = MaxLeadDistance;
            else if (leadDistance < (MaxLeadDistance * -1))
                leadDistance = (MaxLeadDistance * -1);

            
            if (leadHeight > MaxLeadDistance)
                leadHeight = MaxLeadDistance;
            else if (leadHeight < (MaxLeadDistance * -1))
                leadHeight = (MaxLeadDistance * -1);

            Vector3 aheadPoint = centerPoint + Offset + new Vector3(leadDistance, leadHeight, 0);
            Vector3 point = Camera.main.WorldToViewportPoint(aheadPoint);
            Vector3 delta = aheadPoint - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref Velocity, DampingTime);
        }
    }

    private Vector3 FindCenterPoint(List<Transform> objects)
    {
        Vector3 center = new Vector3(0, 0, 0);
        float count = 0;

        foreach (Transform t in objects)
        {
            center += t.position;
            count++;
        }

        return center / count;
    }
}