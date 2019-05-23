using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackEnemyChild : MonoBehaviour
{
    public Vector3 StartPosition;
    public Vector3 JumpPosition;
    public float JumpSpeed;
    //public float GraceDistance = .1f;
    private float step = 0;

    private bool _BouncingUpward = false;
    private bool _BouncingDownward = false;
    Coroutine BounceCoroutine = null;

    private void FixedUpdate()
    {
        if (_BouncingUpward && BounceCoroutine == null)
        {
            BounceCoroutine = StartCoroutine(SmoothMove(StartPosition, JumpPosition, JumpSpeed));
        }
        else if (_BouncingUpward && BounceCoroutine != null)
        {
            if (Vector3.Distance(transform.localPosition, JumpPosition) <= 0)
            {
                _BouncingUpward = false;
                _BouncingDownward = true;
                StopCoroutine(BounceCoroutine);
                BounceCoroutine = null;
            }
        }

        if (_BouncingDownward && BounceCoroutine == null)
        {
            BounceCoroutine = StartCoroutine(SmoothMove(JumpPosition, StartPosition, JumpSpeed));
        }
        else if (_BouncingDownward && BounceCoroutine != null)
        {
            if (Vector3.Distance(StartPosition, transform.localPosition) <= 0)
            {
                _BouncingUpward = false;
                _BouncingDownward = false;
                StopCoroutine(BounceCoroutine);
                BounceCoroutine = null;
            }
        }


        /*
        if (step < 1 && _BouncingUpward)
        {
            step += Time.deltaTime * JumpSpeed;
            transform.localPosition = Vector3.Slerp(StartPosition, JumpPosition, step);
        }   
        else if (step >= 1 && _BouncingUpward)
        {
            step = 0;
            _BouncingUpward = false;
            _BouncingDownward = true;
        }

        if (step < 1 && _BouncingDownward)
        {
            step += Time.deltaTime * JumpSpeed;
            transform.localPosition = Vector3.Slerp(JumpPosition, StartPosition, step);
        }
        else if (step >= 1 && _BouncingDownward)
        {
            step = 0;
            _BouncingUpward = false;
            _BouncingDownward = false;
        }
    */
    }

    public void Bounce ()
    {
        //step = 0;
        BounceCoroutine = null;
        _BouncingUpward = true;
    }

    IEnumerator SmoothMove(Vector3 startpos, Vector3 endpos, float seconds)
    {
        float t = 0.0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            transform.localPosition = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0.0f, 1.0f, t));
            yield return null;
        }
    }
}
