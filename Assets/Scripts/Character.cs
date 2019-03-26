﻿using UnityEngine;

/// <summary>
/// Base class for player character and NPCS
/// </summary>
[RequireComponent(typeof(Animator))]
public class Character : LivingObject
{

    #region Public Variables

    public Transform Rigidbody
    {
        get
        {
            return m_rigidbody.transform;
        }
    }

    public AttackType attackType;

    /// <summary>
    /// The direction for movement
    /// Left = -1
    /// None =  0
    /// Right=  1
    /// </summary>
    public Vector2 moveDir;

    #endregion

    #region Protected Variables

    /// <summary>
    /// Spawn position for Ranged projectiles
    /// </summary>
    public Transform rangedSpawn;


    protected Animator m_Animator;

    protected Rigidbody2D m_rigidbody;

    protected readonly int m_HashDeadParam = Animator.StringToHash("Dead");

    protected MeleeAttack classMeleeAttack;

    #endregion

    #region Private Variables

    private PlatformerCharacter2D m_Character;

    #endregion

    protected virtual void Awake()
    {
        m_Character = GetComponent<PlatformerCharacter2D>();
        m_Animator = GetComponentInChildren<Animator>();
        m_rigidbody = GetComponent<Rigidbody2D>();

        switch (attackType)
        {
            case AttackType.Ranged:

                rangedSpawn = GameObject.Find(this.name + "/Character/RangedTransform").transform;

                break;

            case AttackType.Melee:

                try
                {
                    classMeleeAttack = GetComponentInChildren<MeleeAttack>();
                }
                catch
                {
                    Debug.LogWarning("No Melee component on " + name);
                }

                break;
        }
    }

    /// <summary>
    /// What happens when character dies
    /// </summary>
    /// <param name="attackDirection"> What direction character was attacked from</param>
    protected override void OnDeath(Vector2 attackDirection)
    {
        base.OnDeath(attackDirection);

        moveDir = Vector2.zero;
        m_rigidbody.velocity = Vector2.zero;
        m_rigidbody.simulated = false;

        //gameObject.layer = LayerMask.NameToLayer("PastVillager");

        m_Animator.SetFloat("xSpeed", 0);
        m_Animator.SetFloat("ySpeed", 0);
        m_Animator.SetFloat("xSpeedAbs", 0);
        m_Animator.SetBool(m_HashDeadParam, true);
    }

    protected virtual void FixedUpdate()
    {
        if (!Alive)
            return;

        m_Animator.SetFloat("xSpeed", m_rigidbody.velocity.x);
        m_Animator.SetFloat("ySpeed", m_rigidbody.velocity.y);
        m_Animator.SetFloat("xSpeedAbs", Mathf.Abs(m_rigidbody.velocity.x));

        m_Character.Move(moveDir);
    }

    public void PlayMeleeEffect()
    {
        classMeleeAttack.PlayEffect();
    }

    /// <summary>
    /// Kills player
    /// </summary>
    [ContextMenu("Kill")]
    public override void Kill()
    {
        health = 0;
        m_rigidbody.velocity = Vector2.zero;
        m_rigidbody.angularVelocity = 0;
        OnDeath(Vector2.zero);
    }
}
