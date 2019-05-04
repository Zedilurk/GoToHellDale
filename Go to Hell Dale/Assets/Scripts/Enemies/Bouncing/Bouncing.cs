using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncing : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    public float BounceAmount;
    public float BounceFrequency;

    public bool BeginBounceOnSpawn = true;
    private bool _IsBouncing = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = this.GetComponent<Rigidbody2D>();
        if (BeginBounceOnSpawn)
        {
            _IsBouncing = true;
            StartCoroutine(BounceLoop(BounceFrequency));
        }
    }

    IEnumerator BounceLoop (float bounceInterval)
    {
        while (_IsBouncing)
        {
            Bounce();
            yield return new WaitForSeconds(bounceInterval);
        }        
    }

    private void Bounce ()
    {
        rigidbody2D.AddForce(new Vector2(0, 1 * BounceAmount), ForceMode2D.Impulse);
    }
}
