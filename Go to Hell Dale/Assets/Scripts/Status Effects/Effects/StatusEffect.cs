using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StatusEffectEnum { Slow };
public class StatusEffect : MonoBehaviour
{
    public string Name;
    public StatusEffectEnum Effect;
    public float Length;
    public float RemainingLength;
    private BoxCollider2D trigger;

    private void Start()
    {
        if (this.GetComponent<BoxCollider2D>() != null)
            trigger = this.GetComponent<BoxCollider2D>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (trigger != null)
        {
            if (collision.gameObject.tag == "Player")
            {
                Player player = collision.gameObject.GetComponent<Player>();
                player.ApplyStatusEffect(this);
            }
        }
    }
}
