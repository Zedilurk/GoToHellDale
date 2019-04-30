using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float ProjectileSpeed = 5f;
    public float Gravity = 0f;
    public Vector2 FireDirection = new Vector2(1, 0);

    public float ProjectileLifetime = 10f;

    [HideInInspector]
    public ProjectileLauncher Launcher;

    private bool IsActive = false;
    private Rigidbody2D rigidbody2D;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        //if (IsActive)
            //rigidbody2D.AddForce(FireDirection * ProjectileSpeed);
    }

    private void OnEnable()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.gravityScale = Gravity;
        rigidbody2D.AddForce(FireDirection * ProjectileSpeed, ForceMode2D.Impulse);
        //IsActive = true;
        StartCoroutine(LifeCountdown());
    }

    private IEnumerator LifeCountdown ()
    {
        yield return new WaitForSeconds(ProjectileLifetime);
        OnLifetimeReached();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Hit");

        if (collision.gameObject.tag == "Player")
            collision.gameObject.GetComponent<Player>().Death();

        OnLifetimeReached();
    }

    private void OnLifetimeReached ()
    {
        IsActive = false;
        this.gameObject.SetActive(false);
        Launcher.AddProjectileBackToPool(this.gameObject);
    }
}
