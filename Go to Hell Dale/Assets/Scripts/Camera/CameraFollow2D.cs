using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    private Vector3 Velocity = Vector3.zero;
    [SerializeField]
    private Vector3 _Offset = new Vector3(0, 0, 0);

    public float DampingTime = 0.15f;
    public float MaxLeadDistance = 2f;
    public Transform Target;
    public Vector3 Offset
    {
        get { return _Offset; }
        set { _Offset = value; }
    }

    Player _TargetPlayer;

    private void Start()
    {
        _TargetPlayer = Target.GetComponent<Player>();
    }

    void FixedUpdate()
    {
        if (Target)
        {
            float leadDistance = _TargetPlayer.Velocity.x;

            if (leadDistance > MaxLeadDistance)
                leadDistance = MaxLeadDistance;
            else if (leadDistance < (MaxLeadDistance * -1))
                leadDistance = (MaxLeadDistance * -1);

            Vector3 aheadPoint = Target.position + Offset + new Vector3(leadDistance, 0, 0);
            Vector3 point = Camera.main.WorldToViewportPoint(aheadPoint);
            Vector3 delta = aheadPoint - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref Velocity, DampingTime);
        }
    }
}