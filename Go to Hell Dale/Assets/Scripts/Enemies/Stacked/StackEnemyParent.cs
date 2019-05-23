using Luminosity.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackEnemyParent : MonoBehaviour
{
    public float BounceAmount = 5f;
    public float GapSpacesFromBottom = 1;
    public float GapSize = 2;
    public float JumpSpeed = 2;

    public float TimeBetweenBounces = 4f;

    private bool _IsBouncing = false;
    public bool BeginBouncingOnSpawn = true;

    public List<StackEnemyChild> EnemiesWithin = new List<StackEnemyChild>();



    // Start is called before the first frame update
    void Start()
    {
        /// Add all of our sub items that will bounce at start
        foreach (Transform child in transform)
            if (child.gameObject.tag == "Enemy")
                EnemiesWithin.Add(child.GetComponent<StackEnemyChild>());

        if (BeginBouncingOnSpawn)
        {
            _IsBouncing = true;
            StartCoroutine(BounceLoop());
        }      
    }


    IEnumerator BounceLoop ()
    {
        while (_IsBouncing)
        {
            Bounce();
            yield return new WaitForSeconds(TimeBetweenBounces);
        }
    }

    public void Bounce()
    {
        int x = 0;
        foreach (StackEnemyChild enemy in EnemiesWithin)
        {
            float _BounceDistance = BounceAmount;
            if (x >= GapSpacesFromBottom)
                _BounceDistance += GapSize;

            enemy.JumpSpeed = JumpSpeed;
            enemy.StartPosition = enemy.transform.localPosition;
            enemy.JumpPosition = enemy.transform.localPosition + new Vector3(0, _BounceDistance, 0);
            enemy.Bounce();

            x++;
        }
    }
}
