using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rogue : Villager {

    public float distance = 5;

	// Use this for initialization
	public override void Start ()
    {

        //Need to set Rogue animator here
        base.Start();

        attackType = AttackType.Melee;
        specialType = SpecialType.Press;
	}

    public void Blink()
    {
        float direction = melee.transform.position.x - transform.position.x;

        Vector3 newPos = transform.position + Vector3.right * Mathf.Sign(direction) * distance;

        while (!GameManager.MoveRequest(PlayerCollisions, newPos))
        {
            newPos = transform.position + Vector3.right * Mathf.Sin(direction) * distance --;
        }
        Debug.Log("Teleport allowed");

        transform.position = newPos;
    }
}
