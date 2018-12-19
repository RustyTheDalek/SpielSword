using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for player character and NPCS
/// </summary>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{

    #region Public Variables

    public bool Alive
    {
        get
        {
            return health > 0;
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
    public Transform rangedTrans;

    private PlatformerCharacter2D m_Character;

    protected float health = 1;

    protected Animator m_Animator;
    protected Rigidbody2D m_rigidbody;

    protected readonly int m_HashDeadParam = Animator.StringToHash("Dead");

    #endregion

    #region Private Variables

    #endregion

    protected virtual void Awake()
    {
        m_Character = GetComponent<PlatformerCharacter2D>();
        m_Animator = GetComponent<Animator>();
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        if (!Alive)
            return;
    }

    /// <summary>
    /// What happens when character dies
    /// </summary>
    /// <param name="attackDirection"> What direction character was attacked from</param>
    public virtual void OnDeath(Vector2 attackDirection)
    {
        if(GetComponent<TimeObject>())
            GetComponent<TimeObject>().tObjectState = TimeObjectState.PresentDead;

        this.NamedLog("Dead");

        moveDir = Vector2.zero;
        m_rigidbody.velocity = Vector2.zero;

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

    public void RestoreHealth(float newHealth = 1)
    {
        health = newHealth;
    }
}
