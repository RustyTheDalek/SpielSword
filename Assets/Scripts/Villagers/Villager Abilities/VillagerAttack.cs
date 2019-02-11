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

    protected SpriteRenderer m_Sprite;
    protected Rigidbody2D m_Rigidbody;
    protected Collider2D m_Collider;

    protected void Start()
    {
        m_Sprite = GetComponent<SpriteRenderer>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Collider = GetComponent<Collider2D>();
    }

    protected void Update()
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
        m_Sprite.enabled = active;

        if (m_Collider)
            m_Collider.enabled = active;

        if (m_Rigidbody)
            m_Rigidbody.simulated = active;

        if (!active)
            enabled = false;
    }

    private void OnEnable()
    {
        EffectNoise.Play();
    }
}
