using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base Class for enemy minions
/// </summary>
public abstract class Minion : Character
{
    #region Public Variables

    public MinionState state = MinionState.Patrolling;

    public LayerMask layerGround;

    public List<Villager> villagerInSight = new List<Villager>(5);
    public Villager closestVillager = null;

    float closetVillageDist = Mathf.Infinity;

    [Range(0, 100)]
    public float meleeAttackRange = 5;

    [Range(0, 100)]
    public float rangedAttackRange = 5;

    /// <summary>
    /// How much above or below attack range still constitutes an attack
    /// </summary>
    float rAVariance = 1f;

    public Projectile projectile;

    /// <summary>
    /// After a Rest/Celebrate how long does it for Minion to go back to fighting
    /// </summary>
    public int attackCooldownTime = 3;

    public string RangedAttackAnimName = "RangedAttack",
                  MeleeAttackAnimName  = "MeleeAttack";

    [Tooltip("If the player enters within Melee Range of a Ranged minion will they do a Melee attack?")]
    public bool meleePanic;

    #endregion

    #region Protected Variables

    protected MinionState startingState;

    protected RigidbodyConstraints2D startingConstraints;

    protected int startingLayer;

    protected bool canAttack = true;

    protected Vector2 prevDir = Vector2.zero;

    //For when Minions suceeds in killing a villager (In case we want a celebration
    //If not it can enforce the Minion goes back to patrolling
    protected readonly int  m_HashCelebrateParam = Animator.StringToHash("Celebrate"),
                            m_HashMeleeAttackParam,
                            m_HashRangedAttackParam;
    protected Color startingColor;

    /// <summary>
    /// How the minion moves during attacks
    /// </summary>
    protected Vector2 desiredAttackMoveDirection = Vector2.zero;

    #endregion

    #region Private Variables

    #endregion

    protected override void Awake()
    {
        base.Awake();

        startingState = state;
        startingConstraints = m_rigidbody.constraints;
        startingLayer = gameObject.layer;

        if (GetComponent<SpriteRenderer>())
            startingColor = GetComponent<SpriteRenderer>().color;

        if (projectile)
            projectile.CreatePool(10);
    }

    protected Minion()
    {
        m_HashMeleeAttackParam = Animator.StringToHash(MeleeAttackAnimName);
        m_HashRangedAttackParam = Animator.StringToHash(RangedAttackAnimName);
    }

    public virtual void OnEnable()
    { 
        StopAllCoroutines();

        gameObject.layer = startingLayer;
        m_Animator.SetBool(m_HashDeadParam, false);
        m_Animator.Play("Move");
        health = 1;
        m_rigidbody.velocity = Vector2.zero;
        m_rigidbody.constraints = startingConstraints;
        m_rigidbody.simulated = true;
        state = startingState;

        if (GetComponent<SpriteRenderer>())
            GetComponent<SpriteRenderer>().color = startingColor;

        canAttack = true;

        villagerInSight.Clear();

        SceneLinkedSMB<Minion>.Initialise(m_Animator, this);
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeAttackRange);

        if (attackType == AttackType.Ranged)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, rangedAttackRange);
        }
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, moveDir);
    }

    public abstract void Patrol();

    public virtual void MoveToClosest()
    {
        moveDir = transform.position.PointTo(closestVillager.hat.transform.position);
    }

    /// <summary>
    /// Calculates the movement to the ideal Attack range for ranged minions
    /// </summary>
    public virtual void MoveToEngage()
    {
        moveDir = transform.position.PointTo(closestVillager.transform.position + Vector3.right * rangedAttackRange * Mathf.Sign(transform.position.sqrMagnitude - closestVillager.transform.position.sqrMagnitude));
    }

    protected virtual void StartAttack()
    {
        Debug.Log("Starting Attack");
        //Close enough to attack
        state = MinionState.Attacking;

        StartAttack(attackType);
    }

    protected virtual void StartAttack(AttackType attackToDo)
    {
        Debug.Log("Starting Attack");
        //Close enough to attack
        state = MinionState.Attacking;

        switch (attackToDo)
        {
            case AttackType.Melee:
                m_Animator.SetTrigger(m_HashMeleeAttackParam);
                break;
            case AttackType.Ranged:
                m_Animator.SetTrigger(m_HashRangedAttackParam);
                break;
        }
    }

    public virtual void Attack()
    {
        moveDir = desiredAttackMoveDirection;
    }

    public virtual void CelebrateAttack()
    { 
        state = MinionState.Celebrating;
        m_rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        m_Animator.SetTrigger(m_HashCelebrateParam);
    }

    public virtual void Migrate() { }

    public virtual void StartRest()
    {
        state = MinionState.Resting;
        m_rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public virtual void Rest()
    {
        m_rigidbody.velocity = Vector2.zero;
    }

    public virtual void StopRest()
    {
        m_rigidbody.constraints = startingConstraints;
        state = startingState;
        moveDir = prevDir;
        prevDir = Vector2.zero;
    }

    /// <summary>
    /// Find closest Villager
    /// </summary>
    public void FindClosest()
    {
        //Reset closest to max
        closetVillageDist = Mathf.Infinity;
        //Find the closest
        foreach (Villager villager in villagerInSight)
        {
            float sqrDist = (villager.transform.position - transform.position).sqrMagnitude;

            if ( sqrDist < (closetVillageDist * closetVillageDist))
            {
                closestVillager = villager;
                closetVillageDist = sqrDist;
            }
        }
    }

    /// <summary>
    /// See if closest villager is in attack range
    /// </summary>
    public void CheckMeleeAttackRange()
    {
        if ((closestVillager.transform.position - transform.position).sqrMagnitude < (meleeAttackRange * meleeAttackRange))
        {
            StartAttack(AttackType.Melee);
        }
    }

    public void CheckRangedAttackRange()
    {
        if ((closestVillager.transform.position - transform.position).sqrMagnitude < 
            ((rangedAttackRange + rAVariance) * (rangedAttackRange + rAVariance)) &&
            (closestVillager.transform.position - transform.position).sqrMagnitude > 
            ((rangedAttackRange - rAVariance) * (rangedAttackRange - rAVariance)))
        {
            //Debug.Break();
            Debug.Log("Within Engagement Range");
            StartAttack();
        }
    }

    public void FireProjectile(BallisticMotion objToSpawn)
    {
        Vector3 targetPos = closestVillager.GetComponent<Villager>().hat.transform.position;
        Vector3 diff = targetPos - rangedTrans.position;
        Vector3 diffGround = new Vector3(diff.x, 0f, diff.z);

        Vector3[] solutions = new Vector3[2];
        int numSolutions;

        //if (closestVillager.GetComponent<Rigidbody2D>().velocity.sqrMagnitude > 0)
        //{
        //    numSolutions = fts.solve_ballistic_arc(rangedTrans.position, 15f /*Velocity*/,
        //        targetPos, closestVillager.GetComponent<Rigidbody2D>().velocity,
        //        9.81f, out solutions[0], out solutions[1]);
        //}
        //else
        //{
            numSolutions = fts.solve_ballistic_arc(rangedTrans.position, 15f,
                targetPos, 9.81f, out solutions[0], out solutions[1]);
        //}

        BallisticMotion proj;

        if (numSolutions > 0)
        {

            proj = objToSpawn.Spawn(null, rangedTrans.position);

            proj.Initialize(rangedTrans.position, 9.81f);

            var impulse = solutions[0];

            proj.AddImpulse(impulse);
        }

        //Projectile proj = objToSpawn.Spawn(null, rangedTrans.position);

        //float throwAngle = Vector2.SignedAngle(transform.right * transform.localScale.x, transform.PointTo(closestVillager.transform));

        //Debug.Log("Extra " + throwAngle + "Being added");

        //proj.Throw((int)Mathf.Sign(transform.localScale.x), throwAngle);
    }

    public void SpawnObject(GameObject objToSpawn)
    {
        objToSpawn.Spawn(rangedTrans.transform.position);
    }

    protected virtual void OnFoundTarget()
    {
        state = MinionState.ClosingIn;
    }

    protected virtual void OnNoMoreTargets()
    {
        state = startingState;
    }

    public IEnumerator AttackCooldown()
    {
        float _h, _s, _v;
        //In future have something more elegant than changing colour?
        if (GetComponent<SpriteRenderer>())
        {
            Color.RGBToHSV(startingColor, out _h, out _s, out _v);
            GetComponent<SpriteRenderer>().color = Color.HSVToRGB(_h, _s, _v * .5f);
        }
        canAttack = false;

        yield return new WaitForSeconds(attackCooldownTime);

        if (GetComponent<SpriteRenderer>())
            GetComponent<SpriteRenderer>().color = startingColor;
        canAttack = true;
    }

    public override void OnDeath(Vector2 attackDirection)
    {
        base.OnDeath(attackDirection);
        StopAllCoroutines();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        switch (LayerMask.LayerToName(collision.gameObject.layer))
        {
            case "Weapon":

                //Don't want Minions to be killed by their attacks being hit.
                if (Alive && 
                    collision.otherCollider.gameObject.layer != 
                    LayerMask.NameToLayer("EnemyAttacks"))
                {
                    health--;
                    OnDeath(collision.transform.position.PointTo(transform.position));
                }
                break;

            case "Villager":
            case "PastVillager":

                if (state == MinionState.Attacking && 
                    collision.gameObject == closestVillager &&
                    collision.otherCollider.gameObject.layer != LayerMask.NameToLayer("Minion"))
                {
                    Debug.Log(collision.otherCollider.gameObject.name);
                    //Attack successful start resting
                    this.NamedLog("Hit my target Villager");
                    villagerInSight.Remove(collision.gameObject.GetComponent<Villager>());
                    CelebrateAttack();
                }
                break;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (LayerMask.LayerToName(collision.gameObject.layer))
        {
            case "Villager":
            case "PastVillager":

                if (villagerInSight.Count < villagerInSight.Capacity &&
                !villagerInSight.Contains(collision.GetComponent<Villager>()) &&
                collision.gameObject.GetComponent<Character>().Alive) // don't want them to attack dead villagers
                {
                    villagerInSight.Add(collision.GetComponent<Villager>());

                    if (canAttack &&
                        state == MinionState.Patrolling || state == MinionState.Migrating)
                    {
                        OnFoundTarget();
                    }
                }
                break;

            //TODO:Figure out why this was here and removed if not needed
            case "Weapon":

                if (Alive)
                {
                    health--;
                    OnDeath(collision.transform.position.PointTo(transform.position));
                }
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (LayerMask.LayerToName(collision.gameObject.layer))
        {
            case "Villager":
            case "PastVillager":

                if (villagerInSight.Contains(collision.gameObject.GetComponent<Villager>()))
                {
                    if (canAttack &&
                        state == MinionState.Patrolling || state == MinionState.Migrating)
                    {
                        state = MinionState.ClosingIn;
                    }
                }
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (LayerMask.LayerToName(collision.gameObject.layer))
        {
            case "Villager":
            case "PastVillager":

                if (villagerInSight.Contains(collision.gameObject.GetComponent<Villager>()))
                {
                    villagerInSight.Remove(collision.gameObject.GetComponent<Villager>());

                    if (villagerInSight.Count == 0)
                    {
                        OnNoMoreTargets();
                    }
                }

                break;
        }
    }

    /// <summary>
    /// Sets the state of the Minion
    /// </summary>
    /// <param name="_State"> What state to set.</param>
    /// <param name="setStartState"> Whether this adjust default state.</param>
    public void SetState(MinionState _State, bool setStartState = false)
    {
        state = _State;

        if(setStartState)
        {
            startingState = _State;
        }
    }
}
