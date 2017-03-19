using UnityEngine;
using System.Collections;

/// <summary>
/// Script for VillagerRange
/// </summary>
public class VillagerAttack : MonoBehaviour
{

    public int damageMult = 1;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (this.name.Contains("Range"))
        {
            if (coll.gameObject.GetComponent<Head>())
            {
                coll.gameObject.GetComponent<Head>().OnHit(damageMult);
            }

            if (coll.tag != "Ethereal")
            {
                Destroy(this.gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (this.name.Contains("Range"))
        {
            Destroy(this.gameObject);
        }
    }
}
