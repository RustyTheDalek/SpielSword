using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for registering Boss attacks
/// Created     : Sean Taylor - 24/04/18
/// </summary>
public class MinionHitAttack : MonoBehaviour
{
    //public bool attacking = false;

    void OnCollisionEnter2D(Collision2D coll)
    {
        //Attack can only damage Villagers, has to be enabled and God mode off for 
        //obvious reasons
        if (coll.gameObject.layer == (LayerMask.NameToLayer("Villager")) && !LevelManager.GodMode)
        {
            coll.gameObject.GetComponent<Villager>().OnHit();
        }
    }
}
