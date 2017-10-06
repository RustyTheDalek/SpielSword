using UnityEngine;
using System.Collections;

/// <summary>
/// Script for handling attacks against the boss
/// </summary>
public class VillagerAttack : SpawnableSpriteTimeObject
{
    public int damage = 1;
    public int damageMult = 1;

    public float lifeTime = 1;

    protected override void Update()
    {
        base.Update();

        if (lifeTime < 0)
        {
            SetActive(false);
        }

        lifeTime -= Time.deltaTime;

    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (this.name.Contains("Range") || this.name.Contains("Imp"))
        {
            if (coll.gameObject.GetComponent<Head>())
            {
                coll.gameObject.GetComponent<Head>().OnHit(damage * damageMult);
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
