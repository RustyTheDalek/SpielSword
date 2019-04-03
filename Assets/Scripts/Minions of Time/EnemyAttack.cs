using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for registering Boss attacks
/// Created     : Sean Taylor - 24/04/18
/// </summary>
public class EnemyAttack : MonoBehaviour
{
    public bool spawnable = false;

    //public bool attacking = false;
    [Header("References")]
    public AudioSource EN;
    public Animator m_Anim;
    public Collider2D m_Coll;
    public SpriteRenderer m_Sprite;
    public Rigidbody2D m_Rigidbody;

    public delegate void AttackEvent(EnemyAttack projectile, bool hitPlayer);
    public event AttackEvent OnHit;

    void OnCollisionEnter2D(Collision2D coll)
    {
        //Attack can only damage Villagers, has to be enabled and God mode off for 
        //obvious reasons
        switch (LayerMask.LayerToName(coll.gameObject.layer))
        {
            case "Villager":

                LivingObject character = coll.gameObject.GetComponentInParent<LivingObject>();

                if (!LevelManager.GodMode)
                {
                    character.OnHit(coll.transform.position.PointTo(transform.position));

                    if(OnHit != null)
                        OnHit(this, true);

                    TriggerDeath();

                }

                break;

            case "Ground":

                if (OnHit != null)
                    OnHit(this, false);

                TriggerDeath();

                break;
        }
    }

    void TriggerDeath()
    {
        if (spawnable)
        {
            if (m_Anim)
                m_Anim.SetTrigger("Death");

            m_Coll.enabled = false;
            m_Sprite.enabled = false;
            m_Rigidbody.simulated = false;
            enabled = false;
        }
    }

    public void PlayEffect()
    {
        EN.Stop();
        EN.Play();
    }
}
