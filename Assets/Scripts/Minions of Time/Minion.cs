using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base Class for enemy minions
/// </summary>
public class Minion : Character
{
    #region Public Variables

    public MinionState state = MinionState.Patrolling;

    public LayerMask layerGround;
    public LayerMask layerGroundOnly;
    public LayerMask layerVillagerOnly;

    public ContactFilter2D contactFilter;

    public List<GameObject> villagerInSight = new List<GameObject>(5);
    public GameObject closestVillager = null;

    public float attackRange = 5;

    #endregion

    #region Protected Variables

    protected Timer restTimer;

    protected Vector2 prevDir = Vector2.zero;

    #endregion

    #region Private Variables

    #endregion

    protected virtual void Start()
    {
        restTimer = gameObject.AddComponent<Timer>();
        restTimer.Setup("Rest", 5f);

        contactFilter.layerMask = layerGroundOnly;
    }

    protected override void Update()
    {
        base.Update();

        if(Alive)
        {
            MinionUpdate();
        }
    }

    protected virtual void MinionUpdate()
    {
        switch(state)
        {
            case MinionState.Patrolling:

                Patrol();
                break;

            case MinionState.ClosingIn:

                FindClosest();
                MoveToClosest();
                break;

            case MinionState.Attacking:

                Attack();
                break;

            case MinionState.Resting:

                if(restTimer.complete)
                {
                    state = MinionState.Patrolling;
                    moveDir = prevDir;
                    restTimer.Reset();
                }
                break;

            case MinionState.Migrating:

                Migrate();
                break;
        }
    }

    protected virtual void Patrol() { }
    protected virtual void MoveToClosest() { }
    protected virtual void Attack() { }
    protected virtual void Migrate() { }

    protected void FindClosest()
    {
        //Reset closest to max
        float closest = Mathf.Infinity;
        //Find the closest
        foreach (GameObject villager in villagerInSight)
        {
            float villagerDist = Vector2.Distance(transform.position, villager.transform.position);

            if (villagerDist < attackRange)
            {
                //Close enough to attack
                state = MinionState.Attacking;
                closestVillager = villager;
                break;
            }
            else if (villagerDist < closest)
            {
                closestVillager = villager;
                closest = villagerDist;
            }
        }
    }

    protected virtual void StartRest()
    {
        state = MinionState.Resting;
        restTimer.StartTimer();
    }

    protected virtual void OnNoMoreTargets()
    {
        state = MinionState.Patrolling;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == (LayerMask.NameToLayer("Weapon")))
        {
            health--;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (LayerMask.NameToLayer("Villager")))
        {
            if (villagerInSight.Count < villagerInSight.Capacity &&
                !villagerInSight.Contains(collision.gameObject))
            {
                villagerInSight.Add(collision.gameObject);

                state = MinionState.ClosingIn;
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
