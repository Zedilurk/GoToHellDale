using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum ProjectileType { Straight, Homing };
public class ProjectileLauncher : MonoBehaviour
{
    public GameObject Projectile;
    public bool AutoDeterineProjectilePoolSize = true;
    public int ProjectilePoolSize = 20;

    private int _CurrentPoolIndex = 0;
    private List<GameObject> _ProjectilePool = new List<GameObject>();

    public float ProjectileLifetime = 10f;

    public float InitialDelay = 0f;
    public float IntervalBetweenShotsInSeconds = 2f;
    public float ProjectileSpeed = 5f;
    public float Gravity = 0f;
    public bool PlayerImpactOnly = true;
    public List<Vector2> FireDirections = new List<Vector2>() { new Vector2(-1, 0) };
    public bool UseDegreesFiring = false;
    public List<float> FireDirectionsInDegrees = new List<float>() { 0 };
    public float DegreesOffset = 0;

    public bool TargetPlayerInsteadOfFireDirections = false;
    public int TargetPlayer_NumberOfShotsToFire = 1;
    public bool UseRandomProjectileTypes = true;

    public List<float> DelaysBetweenShots = new List<float>();
    public List<ProjectileType> ProjectileTypes = new List<ProjectileType>();
    public float HomingAccuracy = 5f;


    [System.NonSerialized]
    public bool IsFiring = false;
    public bool BeginFiringOnSpawn = true;

    // Start is called before the first frame update
    void Start()
    {
        //Our end projectile pool size is the amount we need rounded, plus an additional 10% as a safety net
        if (AutoDeterineProjectilePoolSize)
        {
            if (UseDegreesFiring)
            {
                float requiredBase = FireDirectionsInDegrees.Count * (IntervalBetweenShotsInSeconds * 2 / ProjectileLifetime);
                float bonusAmount = requiredBase * 1f;
                int requiredCount = Mathf.RoundToInt(requiredBase + bonusAmount);
                ProjectilePoolSize = requiredCount;
            }
            else
            {
                float requiredBase = FireDirections.Count * (IntervalBetweenShotsInSeconds * 2 / ProjectileLifetime);
                float bonusAmount = requiredBase * 1f;
                int requiredCount = Mathf.RoundToInt(requiredBase + bonusAmount);
                ProjectilePoolSize = requiredCount;
            }
            
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
        float delayUpToThisPoint = 0;
        ProjectileType lastType = ProjectileType.Straight;

        if (TargetPlayerInsteadOfFireDirections)
        {
            for (int x = 0; x < TargetPlayer_NumberOfShotsToFire; x++)
            {
                Vector3 directionToPlayer = GameObject.FindGameObjectWithTag("Player").transform.position - transform.position;
                directionToPlayer.Normalize();
                ProjectileType type = ProjectileType.Straight;

                if (UseRandomProjectileTypes)
                {
                    type = (ProjectileType)Random.Range(0, 2);
                    Debug.Log(type.ToString());
                }                    
                else
                    if (ProjectileTypes.Count > x)
                        type = ProjectileTypes[x];

                StartCoroutine(FireProjectile(directionToPlayer, delayUpToThisPoint + lastDelay, type));

                delayUpToThisPoint += lastDelay;

                float delay = GetNextAvailableDelay(x);
                if (delay != -1)
                    lastDelay = delay;
            }
            
        }
        else if (UseDegreesFiring)
        {
            for (int x = 0; x < FireDirectionsInDegrees.Count; x++)
            {
                if (ProjectileTypes.Count > x)
                    lastType = ProjectileTypes[x];

                StartCoroutine(FireProjectile((FireDirectionsInDegrees[x] + DegreesOffset).DegreeToVector2(), delayUpToThisPoint + lastDelay, lastType));

                delayUpToThisPoint += lastDelay;

                float delay = GetNextAvailableDelay(x);
                if (delay != -1)
                    lastDelay = delay;
            }
        }
        else
        {
            for (int x = 0; x < FireDirections.Count; x++)
            {
                if (ProjectileTypes.Count > x)
                    lastType = ProjectileTypes[x];

                StartCoroutine(FireProjectile(FireDirections[x], delayUpToThisPoint + lastDelay, lastType));

                delayUpToThisPoint += lastDelay;

                float delay = GetNextAvailableDelay(x);
                if (delay != -1)
                    lastDelay = delay;
            }
        }        
    }

    private float GetNextAvailableDelay (int index)
    {
        if (DelaysBetweenShots.Count > index)
            return DelaysBetweenShots[index];

        return -1;
    }


    private IEnumerator FireProjectile(Vector2 direction, float? delay, ProjectileType projectileType = ProjectileType.Straight)
    {
        if (delay != null)
            yield return new WaitForSeconds((float)delay);

        GameObject pObj = null;
        Projectile projectile = GetNextAvailableProjectile(out pObj);

        if (projectile == null || pObj == null)
            yield return null;

        if (!projectile.IsActive)
        {
            SetProjectileValues(projectile, direction, projectileType);
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
    private void SetProjectileValues(Projectile projectile, Vector2? direction = null, ProjectileType projectileType = ProjectileType.Straight)
    {
        projectile.Launcher = this;
        projectile.ProjectileSpeed = ProjectileSpeed;
        projectile.ProjectileLifetime = ProjectileLifetime;
        projectile.Gravity = Gravity;
        projectile.PlayerImpactOnly = PlayerImpactOnly;
        projectile.Type = projectileType;
        projectile.HomingAccuracy = HomingAccuracy;
        projectile.transform.rotation = Quaternion.identity;

        if (direction == null)
        {
            if (UseDegreesFiring)
                projectile.FireDirection = (FireDirectionsInDegrees[0] + DegreesOffset).DegreeToVector2();
            else
                projectile.FireDirection = FireDirections[0];
        }            
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
