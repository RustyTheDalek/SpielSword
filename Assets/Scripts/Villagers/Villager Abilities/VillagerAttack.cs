using UnityEngine;
using System.Collections;

/// <summary>
/// Script for handling attacks against the boss
/// </summary>
public class VillagerAttack : MonoBehaviour
{
    public float damage = 1;
    public float damageMult = 1;

    public float lifeTime = 1;

    public AudioSource EffectNoise;

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
        if (name.Contains("Range") || name.Contains("Imp"))
        {
            if (coll.gameObject.GetComponent<Head>())
            {
                coll.gameObject.GetComponent<Head>().OnHit(damage * damageMult);
                SetActive(false);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (name.Contains("Range"))
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
            enabled = false;
        }
    }

    private void OnEnable()
    {
        EffectNoise.Play();
    }
}
