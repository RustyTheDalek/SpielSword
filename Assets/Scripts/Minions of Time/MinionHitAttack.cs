﻿using System.Collections;
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

    public delegate void AttackEvent(MinionHitAttack projectile, bool hitPlayer);
    public event AttackEvent OnAttack;

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
                    if(OnAttack != null)
                        OnAttack(this, true);
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
