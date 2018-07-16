using UnityEngine;
using System.Collections;

/// <summary>
/// Base Controller for 2D characters
/// Created : GGJ16
/// Updated by : Ian Jones  - 06/04/18
/// </summary>
public class PlatformerCharacter2D : MonoBehaviour
{
    #region Public Variables
    #endregion

    #region Protected Variables

    protected Rigidbody2D m_Rigidbody2D;
    protected Animator m_Anim;

    protected bool dead;
    protected bool meleeAttack;
    protected bool rangedAttack;

    protected Vector2 moveDir = Vector2.zero,
                    deltaMoveDir = Vector2.zero;

    [SerializeField] protected float m_MoveForce = 10f;                  // Strength of force that moves Character

    #endregion

    #region Private Variables
    #endregion

    protected virtual void Awake()
    {
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate()
    {
        if (m_Anim.runtimeAnimatorController)
        {
            // Set the speed for animation
            m_Anim.SetFloat("xSpeed", m_Rigidbody2D.velocity.x);
            m_Anim.SetFloat("ySpeed", m_Rigidbody2D.velocity.y);
            m_Anim.SetFloat("xSpeedAbs", Mathf.Abs(m_Rigidbody2D.velocity.x));
        }
    }

    private void LateUpdate()
    {
        deltaMoveDir = moveDir;
    }

    public virtual void Move(Hashtable animData)
    {
        dead = (bool)animData["Dead"];
        meleeAttack = (bool)animData["MeleeAttack"];
        rangedAttack = (bool)animData["RangedAttack"];
        moveDir = (Vector2)animData["Move"];

        Debug.DrawRay(transform.position, moveDir, Color.green);

        m_Anim.SetBool("Dead", dead);

        if(meleeAttack)
            m_Anim.SetTrigger("MeleeAttack");

        if(rangedAttack)
            m_Anim.SetTrigger("RangedAttack");
    }

    //Reset for animator
    public void Move()
    {
        m_Anim.SetFloat("xSpeed", 0);
        m_Anim.SetBool("Dead", false);
    }
}
