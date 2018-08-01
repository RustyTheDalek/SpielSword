using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Script to control the Ground based minions
/// </summary>
[RequireComponent(typeof(GroundMinionCharacter))]
public class GroundMinions : Minion
{
    #region Public Variables

    #endregion

    #region Protected Variables

    protected GroundMinionCharacter m_GroundMinion;

    protected readonly int m_HashAttackLeft = Animator.StringToHash("AttackLeft");
    protected readonly int m_HashAttackRight = Animator.StringToHash("AttackRight");

    #endregion

    #region Private Variables

    private float distanceToFloor = .8f;
    private float distanceFromWall = .4f;

    bool attackLeft = false,
        attackRight = false;

    #endregion

    protected override void Awake()
    {
        base.Awake();

        m_GroundMinion = GetComponent<GroundMinionCharacter>();
    }

    // Use this for initialization
    protected override void Start ()
    {
        base.Start();
        //Set a random start direction
        moveDir = (Random.Range(0, 2) > 0) ? Vector2.right : Vector2.left;
    }

    protected override void Patrol()
    {
        if(moveDir == Vector2.zero)
        {
            moveDir = (Random.Range(0, 2) > 0) ? Vector2.right : Vector2.left;
        }

        #region Find the floor

        if(!Physics2D.Raycast(m_GroundMinion.m_Front.position, Vector2.down, distanceToFloor, layerGroundOnly))
        {
            moveDir.x *= -1;
        }

        Debug.DrawRay(m_GroundMinion.m_Front.position, Vector2.down * distanceToFloor, Color.green);

        #endregion

        #region Find the wall

        Debug.DrawRay(m_GroundMinion.m_Front.position, moveDir * distanceFromWall, Color.red);

        if (Physics2D.Raycast(m_GroundMinion.m_Front.position, moveDir, distanceFromWall, layerGround))
        {
            moveDir.x *= -1;
        }

        #endregion

        m_GroundMinion.Move(moveDir);
    }

    protected override void Attack()
    {
        prevDir = moveDir;
        moveDir = Vector2.zero;

        if (moveDir.x > 1)
        {
            m_Animator.SetTrigger(m_HashAttackLeft);
        }
        else
        {
            m_Animator.SetTrigger(m_HashAttackRight);
        }

        StartRest();
    }

    protected override void StartRest()
    {
        base.StartRest();
    }
}
