using UnityEngine;
using System.Collections;

/// <summary>
/// Script for handling attacks against the boss
/// </summary>
public class VillagerAttack : MonoBehaviour
{
    public SpriteRenderer m_Sprite;
    public Rigidbody2D m_Rigidbody;
    public Collider2D m_Collider;
    public Animator anim;
    public AudioSource EffectNoise;

    public AttackType attackType;

    public float damage = 1;
    public float damageMult = 1;

    public float lifeTime = 1;

    protected void OnEnable()
    {
        if (EffectNoise)
            EffectNoise.Play();

        if (lifeTime == 0)
        {
            lifeTime = Mathf.Infinity;
        }
    }

    protected virtual void Update()
    {
        if (lifeTime < 0)
        {
            SetActive(false);
        }

        lifeTime -= Time.deltaTime;
    }

    public void Fire(Vector2 dir)
    {
        m_Rigidbody.AddForce(dir, ForceMode2D.Impulse);
    }

    public void OnDeath()
    {
        SetActive(false);
        GetComponent<TimeObject>().FinishTracking();
    }

    //protected virtual void OnTriggerEnter2D(Collider2D coll)
    //{
    //    if (attackType == AttackType.Ranged || name.Contains("Imp"))
    //    {
    //        if (coll.gameObject.GetComponent<Head>())
    //        {
    //            coll.gameObject.GetComponent<Head>().OnHit(damage * damageMult);
    //            anim.SetTrigger("Death");
    //        }
    //    }
    //}

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (attackType == AttackType.Ranged)
        {
            if (coll.gameObject.GetComponent<Head>())
            {
                coll.gameObject.GetComponent<Head>().OnHit(damage * damageMult);
            }
        }

        anim.SetTrigger("Death");
    }

    protected void SetActive(bool active)
    {
        m_Sprite.enabled = active;

        if (m_Collider)
            m_Collider.enabled = active;

        if (m_Rigidbody)
            m_Rigidbody.simulated = active;

        if (!active)
            enabled = false;
    }
}
