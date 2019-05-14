using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Stacked : MonoBehaviour
{
    public float BounceAmount = 5f;
    private Rigidbody2D rigidbody2D;

    [SerializeField]
    private bool _DoBounce = false;
    public bool DoBounce
    {
        get { return _DoBounce; }
        set
        {
            _DoBounce = value;

            if (_DoBounce)
                Bounce();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Bounce()
    {
        rigidbody2D.AddForce(Vector3.up * BounceAmount, ForceMode2D.Impulse);
        _DoBounce = false;
    }
}
