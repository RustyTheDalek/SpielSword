using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for player character and NPCS
/// </summary>
public class Character : MonoBehaviour {

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

    private PlatformerCharacter2D m_Character;

    protected float health = 1;

    protected Animator m_Animator;
    protected Rigidbody2D m_rigidbody;

    protected readonly int m_HashDeadPara = Animator.StringToHash("Dead");

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
        m_Animator.SetBool(m_HashDeadPara, !Alive);

        if (!Alive)
            return;
    }

    public virtual void OnDeath()
    {
        GetComponent<TimeObject>().tObjectState = TimeObjectState.PresentDead;
    }

    public virtual void FixedUpdate()
    {
        if (!Alive)
            return;

        m_Animator.SetFloat("xSpeed", m_rigidbody.velocity.x);
        m_Animator.SetFloat("ySpeed", m_rigidbody.velocity.y);
        m_Animator.SetFloat("xSpeedAbs", Mathf.Abs(m_rigidbody.velocity.x));
        m_Character.Move(moveDir);
    }
}
