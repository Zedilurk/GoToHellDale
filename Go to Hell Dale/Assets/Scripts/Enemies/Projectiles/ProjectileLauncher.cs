using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public GameObject Projectile;
    public bool AutoDeterineProjectilePoolSize = false;
    public int ProjectilePoolSize = 20;

    private int _CurrentPoolIndex = 0;
    public List<GameObject> _ProjectilePool = new List<GameObject>();

    public float ProjectileLifetime = 10f;

    public float InitialDelay = 0f;
    public float IntervalBetweenShotsInSeconds = 2f;
    public float ProjectilesPerShot = 1;
    public float ProjectileSpeed = 5f;
    public float Gravity = 0f;
    public bool PlayerImpactOnly = true;
    public List<Vector2> FireDirections = new List<Vector2>() { new Vector2(-1, 0) };
    public List<float> DelaysBetweenShots = new List<float>();

    [System.NonSerialized]
    public bool IsFiring = false;
    public bool BeginFiringOnSpawn = true;

    // Start is called before the first frame update
    void Start()
    {
        //Our end projectile pool size is the amount we need rounded, plus an additional 10% as a safety net
        if (AutoDeterineProjectilePoolSize)
        {
            float initialSize = ProjectileLifetime / IntervalBetweenShotsInSeconds;
            ProjectilePoolSize = Mathf.RoundToInt(initialSize) + Mathf.RoundToInt(initialSize * .10f);
        }

        PopulatePool();

        lastIntervalBetweenShots = IntervalBetweenShotsInSeconds;

        if (BeginFiringOnSpawn)
            BeginFiring();
    }


    #region Update Intervals from Inspector at Runtime

    //Enables us to adjust fire rate on the fly by reflecting changes made in the debug window
    private float lastIntervalBetweenShots = 0f;
    private void Update()
    {
        if (IntervalBetweenShotsInSeconds != lastIntervalBetweenShots)
        {
            StopFiring();
            lastIntervalBetweenShots = IntervalBetweenShotsInSeconds;
            BeginFiring();
        }
    }

    #endregion

    #region Object Pooling

    private void PopulatePool()
    {
        for (int x = 0; x < ProjectilePoolSize; x++)
        {
            GameObject instance = Instantiate(Projectile);
            Projectile projectile = instance.GetComponent<Projectile>();
            SetProjectileValues(projectile);
            instance.SetActive(false);
            _ProjectilePool.Add(instance);
        }
    }

    #endregion

    #region Firing Controls

    public void BeginFiring()
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
            StopAllCoroutines();
            IsFiring = false;
        }
    }

    #endregion

    #region Firing Action

    private void CalculateFiringGroup ()
    {
        float lastDelay = 0;

        for (int x = 0; x < FireDirections.Count; x++)
        {
            StartCoroutine(FireProjectile(FireDirections[x], lastDelay * x));
            float delay = GetNextAvailableDelay(x);
            if (delay != -1)
                lastDelay = delay;
        }        
    }

    private float GetNextAvailableDelay (int index)
    {
        if (DelaysBetweenShots.Count > index)
            return DelaysBetweenShots[index];

        return -1;
    }


    private IEnumerator FireProjectile(Vector2 direction, float? delay)
    {
        if (delay != null)
            yield return new WaitForSeconds((float)delay);

        GameObject pObj = null;
        Projectile projectile = GetNextAvailableProjectile(out pObj);

        if (projectile == null || pObj == null)
            yield return null;

        if (!projectile.IsActive)
        {
            SetProjectileValues(projectile, direction);
            pObj.transform.position = this.transform.position;
            pObj.SetActive(true);
            projectile.IsActive = true;
        }
    }
    private Projectile GetNextAvailableProjectile(out GameObject pObj)
    {
        int MaxAttempts = 10;
        int CurrentAttempts = 0;
        Projectile projectile = null;
        pObj = null;

        if (_ProjectilePool.Count > 0)
        {
            if (_CurrentPoolIndex >= _ProjectilePool.Count)
                _CurrentPoolIndex = 0;

            pObj = _ProjectilePool[_CurrentPoolIndex];
            projectile = pObj.GetComponent<Projectile>();

            while (projectile.IsActive && CurrentAttempts <= MaxAttempts)
            {
                _CurrentPoolIndex++;

                if (_CurrentPoolIndex >= _ProjectilePool.Count)
                    _CurrentPoolIndex = 0;

                pObj = _ProjectilePool[_CurrentPoolIndex];
                projectile = pObj.GetComponent<Projectile>();
                CurrentAttempts++;
            }
        }

        return projectile;
    }
    private void SetProjectileValues(Projectile projectile, Vector2? direction = null)
    {
        projectile.Launcher = this;
        projectile.ProjectileSpeed = ProjectileSpeed;
        projectile.ProjectileLifetime = ProjectileLifetime;
        projectile.Gravity = Gravity;
        projectile.PlayerImpactOnly = PlayerImpactOnly;

        if (direction == null)
            projectile.FireDirection = FireDirections[0];
        else
            projectile.FireDirection = (Vector2)direction;
    }

    #endregion

    #region Firing Loop

    private IEnumerator FiringLoop()
    {
        yield return new WaitForSeconds(InitialDelay);

        while (IsFiring)
        {
            CalculateFiringGroup();
            yield return new WaitForSeconds(IntervalBetweenShotsInSeconds);
        }
    }

    #endregion
}
