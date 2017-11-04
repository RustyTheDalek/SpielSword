using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rogue : Villager {

    public float distance = 5;

	// Use this for initialization
	public override void Start ()
    {
        m_Animator.runtimeAnimatorController = AssetManager.VillagerAnimators[1];

        //Need to set Rogue animator here
        base.Start();

        villagerAttackType = AttackType.Melee;
        specialType = SpecialType.Press;
	}

    public override void OnSpecial(bool _PlayerSpecial)
    {
        animData.playerSpecial = _PlayerSpecial;
    }

    public void Blink()
    {
        float direction = rangedTrans.position.x - transform.position.x;

        Vector3 newPos = transform.position + Vector3.right * Mathf.Sign(direction) * distance;

        while (!GameManager.moveRequest(PlayerCollisions, newPos))
        {
            newPos = transform.position + Vector3.right * Mathf.Sin(direction) * distance --;
        }
        Debug.Log("Teleport allowed");

        transform.position = newPos;
    }
}
