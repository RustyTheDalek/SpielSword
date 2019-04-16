using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Base Class for enemy minions
/// </summary>
public abstract class Minion : Character
{
    #region Public Variables

    public MinionState state = MinionState.Patrolling;

    public LayerMask layerGround;

    public List<Character> villagerInSight = new List<Character>(5);
    public Character closestVillager = null;

    float closetVillageDist = Mathf.Infinity;

    [Header("Attack Ranges")]
    [Range(0, 100)]
    public float meleeAttackRange = 5;

    [Range(0, 100)]
    public float rangedAttackRange = 5;

    [Header("State Speeds")]
    [Range(0, 100)]
    public int patrolSpeed = 7;

    [Range(0, 100)]
    public int engageSpeed = 10;

    [Range(0, 100)]
    public int attackMoveSpeed = 15;

    /// <summary>
    /// How much above or below attack range still constitutes an attack
    /// </summary>
    float rAVariance = 1f;

    [Tooltip("If the player enters within Melee Range of a Ranged minion will they do a Melee attack?")]
    public bool meleePanic;

    [Tooltip("Does the minion drop into indiviudal pieces when it dies?")]
    public bool minionGibs = false;

    #endregion

    #region Protected Variables

    protected MinionState startingState;

    protected RigidbodyConstraints2D startingConstraints;

    protected int startingLayer;

    //For when Minions suceeds in killing a villager (In case we want a celebration
    //If not it can enforce the Minion goes back to patrolling
    protected readonly int m_HashCelebrateParam = Animator.StringToHash("Celebrate"),
                            m_HashMeleeAttackParam = Animator.StringToHash("Melee"),
                            m_HashRangedAttackParam = Animator.StringToHash("Ranged");


    protected MinionGibTracking[] minionParts;  

    #endregion

    #region Private Variables

    #endregion

    protected override void Awake()
    {
        base.Awake();

        startingState = state;
        startingConstraints = m_rigidbody.constraints;
        startingLayer = gameObject.layer;
        pData.maxVelocity = patrolSpeed;

        minionParts = GetComponentsInChildren<MinionGibTracking>();
    }

    public virtual void OnEnable()
    {
        StopAllCoroutines();

        gameObject.layer = startingLayer;
        m_Animator.SetBool(m_HashDeadParam, false);
        m_Animator.Play("Move");
        health = MaxHealth;
        m_Animator.enabled = true;
        m_rigidbody.velocity = Vector2.zero;
        m_rigidbody.constraints = startingConstraints;
        m_rigidbody.simulated = true;
        state = startingState;

        villagerInSight.Clear();

        pData.maxVelocity = patrolSpeed;

        SceneLinkedSMB<Minion>.Initialise(m_Animator, this);
        SceneLinkedSMB<TimeObject>.Initialise(m_Animator, GetComponent<TimeObject>());
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (m_rigidbody)
            Gizmos.DrawWireSphere(m_rigidbody.position, meleeAttackRange);
        else
            Gizmos.DrawWireSphere(transform.position, meleeAttackRange);

        if (attackType == AttackType.Ranged)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(m_rigidbody.position, rangedAttackRange);
        }

        Gizmos.color = Color.green;
    }

    public abstract void Patrol();

    public virtual void CheckForTrap() { }

    public virtual void MoveToClosest()
    {
        pData.moveDir = m_rigidbody.position.PointTo(closestVillager.Rigidbody.position);
    }

    /// <summary>
    /// Calculates the movement to the ideal Attack range for ranged minions
    /// </summary>
    public virtual void MoveToEngage()
    {
        pData.moveDir = m_rigidbody.position.PointTo(closestVillager.Rigidbody.position
            + Vector3.right * rangedAttackRange * Mathf.Sign(transform.position.magnitude
            - closestVillager.Rigidbody.position.magnitude));
    }

    protected virtual void StartAttack()
    {
        Debug.Log("Starting Attack");
        //Close enough to attack
        StartAttack(attackType);
    }

    protected virtual void StartAttack(AttackType attackToDo)
    {
        Debug.Log("Starting Attack");
        //Close enough to attack
        state = MinionState.Attacking;
        pData.maxVelocity = attackMoveSpeed;
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
        if(attackMoveSpeed == 0)
        {
            m_rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    //public virtual void StopAttack()
    //{
    //    m_rigidbody.constraints = startingConstraints;
    //    pData.maxVelocity = patrolSpeed;
    //}

    public virtual void StartCelebrate()
    {
        pData.maxVelocity = attackMoveSpeed;
        state = MinionState.Celebrating;
        m_rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public virtual void Celebrate()
    {
        pData.maxVelocity = attackMoveSpeed;
        state = MinionState.Celebrating;
        m_rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public virtual void StopCelebrate()
    {
        pData.maxVelocity = patrolSpeed;
        m_rigidbody.constraints = startingConstraints;
        state = startingState;
    }

    public virtual void Migrate() { }

    public virtual void StartRest()
    {
        state = MinionState.Resting;
        pData.maxVelocity = patrolSpeed;
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
        pData.maxVelocity = patrolSpeed;
    }

    /// <summary>
    /// Find closest Villager
    /// </summary>
    public void FindClosest()
    {
        //Reset closest to max
        closetVillageDist = Mathf.Infinity;
        //Find the closest
        foreach (Character villager in villagerInSight)
        {
            float dist = (villager.Rigidbody.position.XY() - m_rigidbody.position).magnitude;

            if (dist < closetVillageDist)
            {
                closestVillager = villager;
                closetVillageDist = dist;
            }
        }
    }

    /// <summary>
    /// See if closest villager is in attack range
    /// </summary>
    public void CheckMeleeAttackRange()
    {
        float distToClosest = (closestVillager.Rigidbody.position.XY() - 
            m_rigidbody.position).magnitude;

        if (distToClosest < meleeAttackRange)
        {
            StartAttack(AttackType.Melee);
        }
    }

    public void CheckRangedAttackRange()
    {
        if ((closestVillager.Rigidbody.position.XY() - 
            m_rigidbody.position).magnitude < rangedAttackRange + rAVariance &&
            (closestVillager.Rigidbody.position.XY() - 
            m_rigidbody.position).magnitude > rangedAttackRange - rAVariance)
        {
            //Debug.Break();
            Debug.Log("Within Engagement Range");
            StartAttack();
        }
    }

    public void FireProjectile(BallisticMotion objToSpawn)
    {
        //Change this to not use hat in future
        Vector3 targetPos = closestVillager.Rigidbody.transform.position;
        Vector3 diff = targetPos - rangedSpawn.position;
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
            numSolutions = fts.solve_ballistic_arc(rangedSpawn.position, 15f,
                targetPos, 9.81f, out solutions[0], out solutions[1]);
        //}

        BallisticMotion proj;

        if (numSolutions > 0)
        {

            proj = objToSpawn.Spawn(null, rangedSpawn.position);

            proj.Initialize(rangedSpawn.position, 9.81f);

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
        objToSpawn.Spawn(rangedSpawn.transform.position);
    }

    protected virtual void OnFoundTarget()
    {
        state = MinionState.ClosingIn;
        pData.maxVelocity = engageSpeed;
    }

    protected virtual void OnNoMoreTargets()
    {
        state = startingState;
        pData.maxVelocity = patrolSpeed;
    }

    protected override void OnDeath(Vector2 attackDirection)
    {
        base.OnDeath(attackDirection);

        StopAllCoroutines();
        //TODO:Re-add this by the separating from TimeObject code
        if (minionGibs)
        {
            m_Animator.enabled = false;
            m_rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

            foreach (MinionGibTracking part in minionParts)
            {
                part.Throw(attackDirection);
            }

            StartCoroutine(DelayToDeath());
        }
    }

    public IEnumerator DelayToDeath()
    {
        yield return new WaitForSeconds(5f);

        GetComponent<TimeObject>().tObjectState = TimeObjectState.PresentDead;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name + " Collided with me" + LayerMask.LayerToName(collision.gameObject.layer));

        switch (LayerMask.LayerToName(collision.gameObject.layer))
        {
            case "Villager":
            case "PastVillager":

                Character villager = collision.gameObject.GetComponentInParent<Character>();

                if (state == MinionState.Attacking && 
                    collision.gameObject.transform.root.gameObject == closestVillager.gameObject &&
                    collision.otherCollider.gameObject.layer != LayerMask.NameToLayer("Minion") &&
                    !villager.shielded)
                {
                    Debug.Log(collision.otherCollider.gameObject.name);
                    this.NamedLog("Hit my target Villager");
                    villagerInSight.Remove(villager);
                    m_Animator.SetTrigger(m_HashCelebrateParam);
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

                Character character = collision.GetComponentInParent<Character>();

                if (villagerInSight.Count < villagerInSight.Capacity &&
                !villagerInSight.Contains(character) &&
                character.Alive) // don't want them to attack dead villagers
                {
                    villagerInSight.Add(character);

                    if (state == MinionState.Patrolling || state == MinionState.Migrating)
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

                if (villagerInSight.Contains(collision.gameObject.GetComponentInParent<Villager>()))
                {
                    if (state == MinionState.Patrolling || state == MinionState.Migrating)
                    {
                        OnFoundTarget();
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

                Character character = collision.GetComponentInParent<Character>();

                if (villagerInSight.Contains(character))
                {
                    villagerInSight.Remove(character);

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
