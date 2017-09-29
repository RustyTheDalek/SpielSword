using UnityEngine;
using System.Collections;

/// <summary>
/// Script for handling attacks against the boss
/// </summary>
public class VillagerAttack : SpawnableSpriteTimeObject
{
    public int damageMult = 1;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (this.name.Contains("Range") || this.name.Contains("Imp"))
        {
            if (coll.gameObject.GetComponent<Head>())
            {
                coll.gameObject.GetComponent<Head>().OnHit(damageMult);
            }

            if (coll.tag != "Ethereal")
            {
                SetActive(false);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (this.name.Contains("Range"))
        {
            SetActive(false);
        }
    }
}
