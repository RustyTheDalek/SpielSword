using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Script to control the Ground based minions
/// </summary>
[RequireComponent(typeof(GroundCharacter2D))]
public class GroundMinion : Minion
{
    #region Public Variables

    public GroundCharacter2D m_GroundCharacter;

    public bool randomStartDir;

    public Direction startDir;

    #endregion

    #region Protected Variables


    #endregion

    #region Private Variables

    private float distanceToFloor = 2;
    private float distanceFromWall = .4f;

    bool attackLeft = false,
        attackRight = false;

    private MinionHitAttack minionAttack;

    #endregion

    protected override void Awake()
    {
        base.Awake();

        m_GroundCharacter = GetComponent<GroundCharacter2D>();
        minionAttack = GetComponentInChildren<MinionHitAttack>();
        m_rigidbody = transform.Find("Motion").GetComponentInChildren<Rigidbody2D>();

        //Set a random start direction
        if (randomStartDir)
            startDir = (Random.Range(0, 2) > 0) ? Direction.Right : Direction.Left;

        moveDir = startDir.ToVector2();
    }

    public override void OnEnable()
    {
        base.OnEnable();

        moveDir = startDir.ToVector2();
    }

    protected override void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (m_GroundCharacter)
            Gizmos.DrawWireSphere(m_GroundCharacter.m_Rigidbody2D.position, meleeAttackRange);

        if (attackType == AttackType.Ranged)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(m_GroundCharacter.m_Rigidbody2D.position, rangedAttackRange);
        }

        Gizmos.color = Color.green;
        if (m_GroundCharacter)
            Gizmos.DrawRay(m_GroundCharacter.m_Rigidbody2D.position, moveDir);
    }

    protected override void FixedUpdate()
    {
        m_Animator.SetFloat("xDir", moveDir.x);
        m_Animator.SetFloat("yDir", moveDir.y);
        m_Animator.SetFloat("xSpeedAbs", Mathf.Abs(moveDir.x));
        m_Animator.SetFloat("xVel", m_rigidbody.velocity.x);

        if (m_GroundCharacter.m_ManualFaceDirection && closestVillager)
        {
            var vilDir = (int)Mathf.Sign((closestVillager.Rigidbody.position.XY() - m_rigidbody.position).x);
            m_GroundCharacter.Move(moveDir, false, vilDir);
            //m_Animator.SetFloat("Direction", vilDir);
        }
        else
        {
            m_GroundCharacter.Move(moveDir, false);
        }
    }

    public override void Patrol()
    {
        //if(moveDir == Vector2.zero)
        //{
        //    moveDir = (Random.Range(0, 2) > 0) ? Vector2.right : Vector2.left;
        //}

        #region Find the floor

        if(m_GroundCharacter.m_Grounded && 
            !Physics2D.Raycast( m_GroundCharacter.m_Front.position, 
                                -transform.up, distanceToFloor, 
                                LayerMask.GetMask("Ground")))
        {
            moveDir = -moveDir;
            Debug.Log("No floor swapping");
        }

        //Debug.DrawRay(m_GroundCharacter.m_Front.position, -transform.up * distanceToFloor, Color.green);

        #endregion

        #region Find the wall

        Debug.DrawRay(m_GroundCharacter.m_Front.position, transform.right * moveDir * distanceFromWall, Color.red);

        if (Physics2D.Raycast(m_GroundCharacter.m_Front.position, transform.right * moveDir, distanceFromWall, layerGround))
        {
            moveDir = -moveDir;

            Debug.Log("At a wall swapping");
        }

        #endregion
    }

    public override void OnDeath(Vector2 attackDirection)
    {
        base.OnDeath(attackDirection);

        gameObject.layer = LayerMask.NameToLayer("Bits");
    }

    protected override void OnFoundTarget()
    {
        base.OnFoundTarget();

        if(!m_GroundCharacter.m_AbsoluteMoveDirection)
            m_GroundCharacter.m_ManualFaceDirection = true;
    }

    protected override void OnNoMoreTargets()
    {
        base.OnNoMoreTargets();

        if (!m_GroundCharacter.m_AbsoluteMoveDirection)
        {
            m_GroundCharacter.m_ManualFaceDirection = false;
            //m_Animator.SetFloat("Direction", 1);
        }
    }

    public override void Attack()
    {
        moveDir = Vector2.zero;
    }

    public override void StartCelebrate()
    {
        base.StartCelebrate();

        moveDir = Vector2.zero;

        if (!m_GroundCharacter.m_AbsoluteMoveDirection)
            m_GroundCharacter.m_ManualFaceDirection = false;
    }

    public void PlayEffect ()
    {
        minionAttack.PlayEffect();
    }
}
