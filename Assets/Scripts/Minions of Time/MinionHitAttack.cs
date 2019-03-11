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
    public AudioSource EN;

    void OnCollisionEnter2D(Collision2D coll)
    {
        //Attack can only damage Villagers, has to be enabled and God mode off for 
        //obvious reasons
        switch (LayerMask.LayerToName(coll.gameObject.layer))
        {
            case "Villager":

                LivingObject character = coll.gameObject.GetComponentInParent<LivingObject>();

                if (!LevelManager.GodMode)
                {
                    character.OnHit(coll.transform.position.PointTo(transform.position));
                }

                break;
        }
    }

    public void PlayEffect()
    {
        EN.Stop();
        EN.Play();
    }
}
