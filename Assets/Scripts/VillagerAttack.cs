using UnityEngine;
using System.Collections;

/// <summary>
/// Script for handling attacks against the boss
/// </summary>
public class VillagerAttack : MonoBehaviour
{
    public int damage = 1;
    public int damageMult = 1;

    public float lifeTime = 1;

    protected void Update()
    {
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

            //TODO: add logic so projectile does not collide with self 
            //(Could be done with Tags)
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

    protected void SetActive(bool active)
    {
        GetComponent<SpriteRenderer>().enabled = active;

        if (GetComponent<Collider2D>())
            GetComponent<Collider2D>().enabled = active;

        if (GetComponent<Rigidbody2D>())
            GetComponent<Rigidbody2D>().simulated = active;

        if (!active)
        {
            this.enabled = false;
        }
    }
}
