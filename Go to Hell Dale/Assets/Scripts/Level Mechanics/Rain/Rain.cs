using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain : MonoBehaviour {

    public Vector2 Direction = Vector2.zero;
    public float RainSpeed = 5f;

	void FixedUpdate ()
    {
        transform.Translate((Direction * RainSpeed) * Time.deltaTime);
    }
}
