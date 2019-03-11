using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base Class for any living characters in the game that can be hit or attacked
/// </summary>
public class LivingObject : MonoBehaviour
{
    protected float health = 1;
    public bool Alive
    {
        get
        {
            return health > 0;
        }
        set
        {
            if (value)
                health = 1;
            else
                health = 0;
        }
    }

    /// <summary>
    /// If a Villager is shielded they are unable to take damage from attacks
    /// </summary>
    public bool shielded = false;

    public virtual void OnHit(Vector2 attackDirection, float damage = 1)
    {
        if (!shielded && Alive)
        {
            health -= damage;

            if (health <= 0)
            {
                OnDeath(attackDirection);
            }
        }
    }

    /// <summary>
    /// What happens when character dies
    /// </summary>
    /// <param name="attackDirection"> What direction character was attacked from</param>
    protected virtual void OnDeath(Vector2 attackDirection)
    {
        GetComponent<TimeObject>().tObjectState = TimeObjectState.PresentDead;

        Debug.Log(name + " has Died");
    }

    [ContextMenu("RestoreHelth")]
    public void RestoreHealth()
    {
        health = 1;
    }

    /// <summary>
    /// Kills player
    /// </summary>
    [ContextMenu("Kill")]
    public virtual void Kill()
    {
        health = 0;

        OnDeath(Vector2.zero);
    }

    //protected virtual void OnCollisionEnter2D(Collision2D coll)
    //{
    //    Debug.Log(coll.gameObject.name + "Collider Hit: " + name);

    //    switch (LayerMask.LayerToName(coll.gameObject.layer))
    //    {
    //        case "Weapon":

    //            MeleeAttack attack = coll.gameObject.GetComponent<MeleeAttack>();
    //            OnHit(coll.transform.PointTo(transform), attack.Damage);
    //            break;

    //        case "Boss":
    //            OnHit(coll.transform.PointTo(transform));
    //            break;
    //    }
    //}

    //protected virtual void OnTriggerEnter2D(Collision2D coll)
    //{
    //    Debug.Log(coll.gameObject.name + "Trigger Hit: " + name);

    //    switch (LayerMask.LayerToName(coll.gameObject.layer))
    //    {
    //        case "Weapon":

    //            MeleeAttack attack = coll.gameObject.GetComponent<MeleeAttack>();
    //            OnHit(coll.transform.PointTo(transform), attack.Damage);

    //            break;

    //        case "Boss":
    //            OnHit(coll.transform.PointTo(transform));
    //            break;
    //    }
    //}

    //protected virtual void OnTriggerStay2D(Collision2D coll)
    //{
    //    Debug.Log(coll.gameObject.name + "Trigger Stayed: " + name);

    //    switch (LayerMask.LayerToName(coll.gameObject.layer))
    //    {
    //        case "Weapon":

    //            MeleeAttack attack = coll.gameObject.GetComponent<MeleeAttack>();
    //            OnHit(coll.transform.PointTo(transform), attack.Damage);

    //            break;

    //        case "Boss":
    //            OnHit(coll.transform.PointTo(transform));
    //            break;
    //    }
    //}
}
