using UnityEngine;
using System.Collections;

/// <summary>
/// Script for handling attacks against the boss
/// </summary>
//TODO: Make more generic so can be used by minions too
public class VillagerAttack : MonoBehaviour
{
    [Header("References")]
    public SpriteRenderer m_Sprite;
    public Rigidbody2D m_Rigidbody;
    public Collider2D m_Collider;
    public Animator anim;
    public AudioSource EffectNoise;

    [Header("Settings")]
    public AttackType attackType;

    public float damage = 1;
    public float damageMult = 1;

    /// <summary>
    /// Total damage including multiplier
    /// </summary>
    public float Damage
    {
        get
        {
            return damage * damageMult;
        }
    }

    public float lifeTime = 1;

    private void Awake()
    {
        if (EffectNoise == null)
        {
            Debug.LogWarning("EffectNoise not set, attempting to find it");
            EffectNoise = GetComponent<AudioSource>();
        }
    }

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
            switch (LayerMask.LayerToName(coll.gameObject.layer))
            {
                case "Boss":
                case "Minion":

                    coll.gameObject.GetComponent<LivingObject>().OnHit(
                        transform.PointTo(coll.transform), damage * damageMult);
                    break;
            }
        }

        if(anim)
            anim.SetTrigger("Death");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (TimeObjectManager.timeState == TimeState.Forward)
        {
            switch (LayerMask.LayerToName(collision.gameObject.layer))
            {
                case "Boss":
                case "Minion":
                    collision.GetComponentInParent<LivingObject>().OnHit(
                        collision.transform.PointTo(transform), Damage);
                    break;
            }
        }
    }

    public void PlayEffect()
    {
        if (EffectNoise)
        {
            EffectNoise.Stop();
            EffectNoise.Play();
        }
        else
        {
            Debug.LogWarning("No Noise assigned");
        }
    }

    protected void SetActive(bool active)
    {
        if (m_Sprite)
            m_Sprite.enabled = active;

        if (m_Collider)
            m_Collider.enabled = active;

        if (m_Rigidbody)
            m_Rigidbody.simulated = active;

        if (!active)
            enabled = false;
    }
}
