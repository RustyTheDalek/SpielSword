using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Script to control the Ground based minions
/// </summary>
[RequireComponent(typeof(GroundMinionCharacter))]
public class GroundMinion : Minion
{
    #region Public Variables

    public bool randomStartDir;

    public Direction startDir;
    #endregion

    #region Protected Variables

    protected GroundMinionCharacter m_GroundMinion;

    #endregion

    #region Private Variables

    private float distanceToFloor = .8f;
    private float distanceFromWall = .4f;

    bool attackLeft = false,
        attackRight = false;

    private MinionHitAttack minionAttack;

    #endregion

    protected override void Awake()
    {
        base.Awake();

        m_GroundMinion = GetComponent<GroundMinionCharacter>();
        minionAttack = GetComponentInChildren<MinionHitAttack>();

        //Set a random start direction
        if(randomStartDir)
            startDir = (Random.Range(0, 2) > 0) ? Direction.Right : Direction.Left;

        moveDir = startDir.ToVector2();
    }

    public override void OnEnable()
    {
        base.OnEnable();

        moveDir = startDir.ToVector2();
    }

    protected override void FixedUpdate()
    {
        m_Animator.SetFloat("xSpeed", moveDir.x);
        m_Animator.SetFloat("ySpeed", moveDir.y);
        m_Animator.SetFloat("xSpeedAbs", Mathf.Abs(moveDir.x));

        m_GroundMinion.Move(moveDir);
    }

    public override void Patrol()
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
    }

    public override void OnDeath(Vector2 attackDirection)
    {
        base.OnDeath(attackDirection);

        gameObject.layer = LayerMask.NameToLayer("Bits");
    }

    public override void Attack()
    {
        moveDir = Vector2.zero;
    }

    public void PlayEffect ()
    {
        minionAttack.PlayEffect();
    }
}
