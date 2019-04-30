using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public GameObject Projectile;
    public bool AutoDeterineProjectilePoolSize = false;
    public int ProjectilePoolSize = 20;
    public List<GameObject> _ProjectilePool = new List<GameObject>();

    public float ProjectileLifetime = 10f;

    public float InitialDelay = 0f;
    public float IntervalBetweenShotsInSeconds = 2f;
    public float ProjectilesPerShot = 1;
    public float ProjectileSpeed = 5f;
    public float Gravity = 0f;
    public bool PlayerImpactOnly = true;
    public List<Vector2> FireDirections = new List<Vector2>() { new Vector2(-1, 0) };

    public bool IsFiring = false;
    public bool BeginFiringOnSpawn = true;

    // Start is called before the first frame update
    void Start()
    {
        if (AutoDeterineProjectilePoolSize)
        {
            float initialSize = ProjectileLifetime / IntervalBetweenShotsInSeconds;
            //Our end projectile pool size is the exact amount we need rounded, plus an additional 10% for as a safety net
            ProjectilePoolSize = Mathf.RoundToInt(initialSize) + Mathf.RoundToInt(initialSize * .10f);
        }

        PopulatePool();

        if (BeginFiringOnSpawn)
            BeginFiring();
    }

    private void PopulatePool()
    {
        for (int x = 0; x < ProjectilePoolSize; x++)
        {
            GameObject instance = Instantiate(Projectile);
            Projectile projectile = instance.GetComponent<Projectile>();

            projectile.Launcher = this;
            projectile.ProjectileSpeed = ProjectileSpeed;
            projectile.ProjectileLifetime = ProjectileLifetime;
            projectile.Gravity = Gravity;
            projectile.PlayerImpactOnly = PlayerImpactOnly;

            if (FireDirections.Count == 1)
                projectile.FireDirection = FireDirections[0];
            else
            {
                //implement multiple directions stuff later
            }

            instance.SetActive(false);
            _ProjectilePool.Add(instance);
        }
    }
    public void AddProjectileBackToPool (GameObject objectToAddToPool)
    {
        _ProjectilePool.Add(objectToAddToPool);
    }

    public void BeginFiring ()
    {
        if (!IsFiring)
        {
            IsFiring = true;
            StartCoroutine(FiringLoop());            
        }
    }
    public void StopFiring()
    {
        if (IsFiring)
        {

            IsFiring = false;
        }
    }


    public void FireProjectile ()
    {
        if (_ProjectilePool.Count > 0)
        {
            GameObject pObj = _ProjectilePool.Pop(0);
            pObj.transform.position = this.transform.position;
            pObj.SetActive(true);
        }
    }

    private IEnumerator FiringLoop ()
    {
        yield return new WaitForSeconds(InitialDelay);       

        while (IsFiring)
        {
            yield return new WaitForSeconds(IntervalBetweenShotsInSeconds);
            FireProjectile();
        }        
    }

}
