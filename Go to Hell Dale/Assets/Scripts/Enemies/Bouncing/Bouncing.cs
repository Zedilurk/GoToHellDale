using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bouncing : MonoBehaviour
{
    public List<Vector2> BounceDirections = new List<Vector2>()
    {
        new Vector2(0, 2f)
    };
    public float BounceMultiplier = 8;
    public bool BeginBounceOnSpawn = true;
    public float DelayBeforeBouncing = 0f;
    public float DelayBetweenBounces = 0f;
    public bool ResetPhysicsBetweenBounces = true;

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

        if (ResetPhysicsBetweenBounces)
            rigidbody2D.velocity = new Vector2(0, 0);

        rigidbody2D.AddForce(GetNextBounceDirection() * BounceMultiplier, ForceMode2D.Impulse);
    }

    int currentBounceIndex = 0;
    private Vector2 GetNextBounceDirection ()
    {
        if (BounceDirections.Count <= currentBounceIndex)
            currentBounceIndex = 0;

        Vector2 bounceDir = BounceDirections[currentBounceIndex];
        currentBounceIndex++;
 
        return bounceDir;
    }
}
