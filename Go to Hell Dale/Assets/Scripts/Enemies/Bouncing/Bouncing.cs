﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bouncing : MonoBehaviour
{
    public Vector2 BounceDirection = new Vector2(-0.5f, 2f);
    public float BounceMultiplier = 8;
    public bool BeginBounceOnSpawn = true;
    public float DelayBeforeBouncing = 0f;
    public float DelayBetweenBounces = 0f;

    private Rigidbody2D rigidbody2D;
    public bool IsBouncing = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = this.GetComponent<Rigidbody2D>();
        if (BeginBounceOnSpawn)
            StartCoroutine(StartBouncing());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsBouncing)
            StartCoroutine(Bounce());         
    }

    private IEnumerator StartBouncing ()
    {
        yield return new WaitForSeconds(DelayBeforeBouncing);
        IsBouncing = true;

        if (DelayBeforeBouncing > 0)
            StartCoroutine(Bounce());
    }

    private IEnumerator Bounce ()
    {
        yield return new WaitForSeconds(DelayBetweenBounces);
        rigidbody2D.AddForce(BounceDirection * BounceMultiplier, ForceMode2D.Impulse);
    }
}
