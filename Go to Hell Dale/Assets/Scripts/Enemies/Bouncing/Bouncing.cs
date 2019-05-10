using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Bouncing : MonoBehaviour
{
    public Vector2 BounceDirection = new Vector2(-0.5f, 2f);
    public float BounceMultiplier = 8;
    public bool BeginBounceOnSpawn = true;


    private Rigidbody2D rigidbody2D;
    public bool IsBouncing = false;
    

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = this.GetComponent<Rigidbody2D>();
        if (BeginBounceOnSpawn)
            IsBouncing = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsBouncing)
            Bounce();
    }

    private void Bounce ()
    {
        rigidbody2D.AddForce(BounceDirection * BounceMultiplier, ForceMode2D.Impulse);
    }
}
