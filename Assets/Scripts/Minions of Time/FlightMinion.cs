﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FlyingCharacter2D))]
public class FlightMinion : Minion
{
    #region Public Variables

    [Range(0, 100)]
    public int patrolSpeed = 7;

    [Range(0, 100)]
    public int attackMoveSpeed = 15;

    /// <summary>
    /// Distance Minion orbits from origin
    /// </summary>
    public int orbitDistance = 5;

    /// <summary>
    /// The strength of the force that maintains the distance
    /// </summary>
    public float distanceAmp = 2;

    /// <summary>
    /// Kill object once it has passed over the threshold for the level
    /// </summary>
    public float killZone;

    #endregion

    #region Protected Variables

    protected PolygonCollider2D[] parts;

    protected int moveSpeed;

    protected readonly int m_HashAttackParam = Animator.StringToHash("Attack");
    protected readonly int m_HashStuckParam = Animator.StringToHash("Stuck");

    #endregion

    #region Private Variables

    private Vector3 orginalPosition;

    private FlyingCharacter2D m_Flying;

    #endregion

    protected override void Awake()
    {
        base.Awake();

        m_Flying = GetComponent<FlyingCharacter2D>();
    }

    protected override void Start()
    {
        base.Start();

        parts = GetComponentsInChildren<PolygonCollider2D>();

        moveSpeed = patrolSpeed;

        orginalPosition = transform.position;

        //Move the Minion by the distance we want him to Orbit
        transform.position = transform.position + Vector3.right * orbitDistance;

        SceneLinkedSMB<FlightMinion>.Initialise(m_Animator, this);

    }

    protected override void FixedUpdate()
    {
        m_Animator.SetFloat("xSpeed", m_rigidbody.velocity.x);
        m_Animator.SetFloat("ySpeed", m_rigidbody.velocity.y);
        m_Animator.SetFloat("xSpeedAbs", Mathf.Abs(m_rigidbody.velocity.x));

        m_Flying.Move(moveDir, moveSpeed);
    }

    public override void Migrate()
    {
        //TODO: Make this appropriate in future
        if(transform.position.x < killZone)
        {
            gameObject.SetActive(false);
            return;
        }

        //TODO: improve this, sort of works but it takes them a while to get back
        moveDir = new Vector2(-1, transform.position.PointTo(orginalPosition).y);

        m_Flying.Move(moveDir, moveSpeed);
    }

    public override void Patrol()
    {
        //This code finds vector towards it's origin then finds the 
        //perpendicular vector with a clockwise bias so that it moves
        //in that direction
        Vector2 toOriginal = transform.position - orginalPosition;
        Vector2 forwardPoint = new Vector2(-toOriginal.y, toOriginal.x) /
            Mathf.Sqrt(toOriginal.x.Sqd() + toOriginal.y.Sqd());
        //We use the direction to the Origin and the desired distance to 
        //adjust the move direction to keep them the right distance away
        moveDir = forwardPoint.normalized + (toOriginal.normalized *
            (orbitDistance - toOriginal.magnitude) * distanceAmp);
    }

    protected override void StartAttack()
    {
        base.StartAttack();

        m_Animator.SetTrigger(m_HashAttackParam);
        moveSpeed = attackMoveSpeed;
    }

    public override void MoveToClosest()
    {
        moveDir = transform.position.PointTo(closestVillager.transform.position);
        m_Flying.Move(moveDir);
    }

    protected override void OnNoMoreTargets()
    {
        base.OnNoMoreTargets();

        m_Animator.SetBool(m_HashAttackParam, false);
    }

    public override void StopRest()
    {
        base.StopRest();

        moveSpeed = patrolSpeed;
    }

    protected void OnStuck()
    {
        //We want to keep, the direction the same in this case so we grab the current X &
        //Y Speed and hold them separately
        m_Animator.SetFloat("x", m_Animator.GetFloat("xSpeed"));
        m_Animator.SetFloat("y", m_Animator.GetFloat("ySpeed"));
        m_Animator.SetTrigger(m_HashStuckParam);
        m_rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        moveSpeed = patrolSpeed;
    }

    public void OnUnstuck()
    {
        this.NamedLog("I'm unstuck now");
        m_rigidbody.constraints = RigidbodyConstraints2D.None;
        state = startingState;
        StartCoroutine(AttackCooldown());
        moveDir = transform.position.PointTo(orginalPosition);
    }

    public override void OnDeath(Vector2 attackDirection)
    {
        this.NamedLog("Getting Yeeted in " + attackDirection);

        base.OnDeath(attackDirection);

        m_Animator.enabled = false;

        Rigidbody2D rb;

        foreach(PolygonCollider2D part in parts)
        {
            rb = part.GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;

            Vector2 throwforce = (new Vector2(
                attackDirection.x * Random.Range(2f, 10f),
                attackDirection.y * Random.Range(1f, 5f)) * (rb.mass * rb.mass));

            Debug.DrawRay(part.transform.position, throwforce, Color.red, 5f);

            rb.AddForce(throwforce, ForceMode2D.Impulse);
            part.enabled = true;

            if (part.name == "Head")
            {
                part.transform.GetChild(1).SetParent(null);
                part.transform.GetChild(1).SetParent(null);
            }
            else
            {
                part.gameObject.transform.DetachChildren();
            }

            part.transform.SetParent(null);
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        this.NamedLog("Hit a thing");

        if (state == MinionState.Attacking)
        {
            switch (LayerMask.LayerToName(collision.gameObject.layer))
            {
                case "Ground":

                    this.NamedLog("Ooof, Hit a Wall");

                    OnStuck();

                    break;
            }
        }
    }
}
