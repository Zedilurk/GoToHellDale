using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISlideIn : MonoBehaviour
{
    public Vector3 StartPosition;
    public Vector3 OvershootPosition;
    public Vector3 EndPosition;
    public float Delay;
    public float TimeToReachTarget = 1;

    public bool UseOvershoot = false;
    public float ReturnToEndPositionTime = .2f;

    public bool SlideInImmediately = false;

    private float t;
    private float _timeToReachTarget;
    private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = this.GetComponent<RectTransform>();
        _startPosition = _endPosition = rectTransform.localPosition;

        if (SlideInImmediately)
            InitialSlideIn();
    }

    private void InitialSlideIn()
    {
        StartCoroutine(SlideUI(true, false, Delay));
    }

    public void BeginSlideIn()
    {
        StartCoroutine(SlideUI(true, false, 0));
    }

    public void BeginSlideOut()
    {
        StartCoroutine(SlideUI(false, false, 0));
    }

    void FixedUpdate()
    {
        t += Time.deltaTime / _timeToReachTarget;
        rectTransform.localPosition = Vector3.Lerp(_startPosition, _endPosition, t);

        if (rectTransform.localPosition == OvershootPosition && UseOvershoot)
            SetDestination(EndPosition, ReturnToEndPositionTime);
    }

    IEnumerator SlideUI(bool slideIn, bool overshoot, float pause)
    {
        yield return new WaitForSeconds(pause);

        if (slideIn)
        {
            if (UseOvershoot)
                SetDestination(OvershootPosition, TimeToReachTarget);
            else
                SetDestination(EndPosition, TimeToReachTarget);
        }
        else
            SetDestination(StartPosition, TimeToReachTarget);        
    }

    private Vector3 _startPosition;
    private Vector3 _endPosition;
    public void SetDestination(Vector3 destination, float time)
    {
        t = 0;
        _timeToReachTarget = time;
        _startPosition = rectTransform.localPosition;
        _endPosition = destination;        
    }
}
