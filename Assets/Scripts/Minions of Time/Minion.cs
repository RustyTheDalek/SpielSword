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
    public LayerMask layerGroundOnly;
    public LayerMask layerVillagerOnly;

    public ContactFilter2D contactFilter;

    public List<GameObject> villagerInSight = new List<GameObject>(5);
    public GameObject closestVillager = null;

    [Range(0, 100)]
    public float attackRange = 5;

    /// <summary>
    /// After a Rest/Celebrate how long does it for Minion to go back to fighting
    /// </summary>
    public int attackCooldownTime = 3;

    #endregion

    #region Protected Variables

    protected MinionState startingState;

    protected RigidbodyConstraints2D startingConstraints;

    protected bool canAttack = true;

    protected Vector2 prevDir = Vector2.zero;

    //For when Minions suceeds in killing a villager (In case we want a celebration
    //If not it can enforce the Minion goes back to patrolling
    protected readonly int m_HashCelebrateParam = Animator.StringToHash("Celebrate");

    #endregion

    #region Private Variables

    #endregion

    protected virtual void Start()
    {
        contactFilter.layerMask = layerGroundOnly;

        startingState = state;
        startingConstraints = m_rigidbody.constraints;

        SceneLinkedSMB<Minion>.Initialise(m_Animator, this);
    }

    private void OnEnable()
    {
        gameObject.layer = LayerMask.NameToLayer("Minion");
        m_Animator.SetBool(m_HashDeadParam, false);
        m_Animator.Play("Move");
        health = 1;
    }

    public abstract void Patrol();

    public virtual void MoveToClosest() { }

    protected virtual void StartAttack()
    {
        //Close enough to attack
        state = MinionState.Attacking;
    }

    public virtual void Attack() { }

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
        float closest = Mathf.Infinity;
        //Find the closest
        foreach (GameObject villager in villagerInSight)
        {

            float sqrDist = (villager.transform.position - transform.position).sqrMagnitude;

            if ( sqrDist < (closest * closest))
            {
                closestVillager = villager;
                closest = sqrDist;
            }
        }
    }

    /// <summary>
    /// See if closest villager is in attack range
    /// </summary>
    public void CheckAttackRange()
    {
        if ((closestVillager.transform.position - transform.position).sqrMagnitude < (attackRange *attackRange))
        {
            StartAttack();
        }
    }

    protected virtual void OnNoMoreTargets()
    {
        state = startingState;
    }

    public IEnumerator AttackCooldown()
    {
        //In future have something more elegant than changing colour?
        if(GetComponent<SpriteRenderer>())
            GetComponent<SpriteRenderer>().color = Color.HSVToRGB(0, 1, .5f);
        canAttack = false;

        yield return new WaitForSeconds(attackCooldownTime);

        if (GetComponent<SpriteRenderer>())
            GetComponent<SpriteRenderer>().color = Color.HSVToRGB(0, 1, 1);
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

                if (Alive)
                {
                    health--;
                    OnDeath(collision.transform.position.PointTo(transform.position));
                }
                break;

            case "Villager":

                if (state == MinionState.Attacking && 
                    collision.gameObject == closestVillager)
                {
                    //Attack successful start resting
                    this.NamedLog("Hit my target Villager");
                    villagerInSight.Remove(collision.gameObject);
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

                if (villagerInSight.Count < villagerInSight.Capacity &&
                !villagerInSight.Contains(collision.gameObject) &&
                collision.gameObject.GetComponent<Character>().Alive) // don't want them to attack dead villagers
                {
                    villagerInSight.Add(collision.gameObject);

                    if (canAttack &&
                        state == MinionState.Patrolling || state == MinionState.Migrating)
                    {
                        state = MinionState.ClosingIn;
                    }
                }
                break;

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
        if (collision.gameObject.layer == (LayerMask.NameToLayer("Villager")))
        {
            if (villagerInSight.Contains(collision.gameObject))
            {
                if (canAttack &&
                    state == MinionState.Patrolling || state == MinionState.Migrating)
                {
                    state = MinionState.ClosingIn;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (LayerMask.NameToLayer("Villager")))
        {
            if (villagerInSight.Contains(collision.gameObject))
            {
                villagerInSight.Remove(collision.gameObject);

                if (villagerInSight.Count == 0)
                {
                    OnNoMoreTargets();
                }
            }
        }
    }
}
