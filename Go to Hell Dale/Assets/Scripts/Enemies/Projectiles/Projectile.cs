using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float ProjectileSpeed = 5f;
    public float Gravity = 0f;
    public Vector2 FireDirection = new Vector2(1, 0);

    public float ProjectileLifetime = 10f;
    public bool PlayerImpactOnly = true;

    [HideInInspector]
    public ProjectileLauncher Launcher;

    public bool IsActive = false;
    private Rigidbody2D rigidbody2D;
    public ProjectileType Type;
    public float HomingAccuracy = 1f;

    private GameObject player;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        switch (Type)
        {
            case ProjectileType.Straight:

                break;
            case ProjectileType.Homing:
                if (player == null)
                    player = GameObject.Find("Player");

                if (player != null)
                {
                    //FireDirection
                    /*
                    rigidbody2D.velocity = transform.up * ProjectileSpeed * 100 * Time.deltaTime;
                    Vector3 targetVector = player.transform.position - transform.position;
                    float rotatingIndex = Vector3.Cross(targetVector, transform.up).z;
                    rigidbody2D.angularVelocity = -1 * rotatingIndex * HomingAccuracy * 100 * Time.deltaTime;
                    */

                    /*
                    float step = ProjectileSpeed * Time.deltaTime; // calculate distance to move
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
                    */
                }
                break;
        }
    }

    private void OnEnable()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

        if (PlayerImpactOnly)
            gameObject.layer = LayerMask.NameToLayer("PlayerOnlyProjectile");
        else
            gameObject.layer = LayerMask.NameToLayer("Projectile");

        if (Type == ProjectileType.Straight)
        {
            rigidbody2D.gravityScale = Gravity;
            rigidbody2D.AddForce(FireDirection * ProjectileSpeed, ForceMode2D.Impulse);
        }
        else if (Type == ProjectileType.Homing)
        {
            transform.eulerAngles = new Vector3(FireDirection.x, FireDirection.y, 0);
        }
        
        StartCoroutine(LifeCountdown());
    }

    private IEnumerator LifeCountdown ()
    {
        yield return new WaitForSeconds(ProjectileLifetime);
        OnLifetimeReached();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            collision.gameObject.GetComponent<Player>().Death();

        OnLifetimeReached();
    }

    private void OnLifetimeReached ()
    {
        StopAllCoroutines();
        IsActive = false;
        this.gameObject.SetActive(false);
    }
}
