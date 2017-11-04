using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : AuraVillager {

    // Use this for initialization
    public override void Start ()
    {
        base.Start();
        villagerAttackType = AttackType.Ranged;
	}

    protected override GameObject Aura()
    {
        return AssetManager.MageAura.Spawn(transform.position);
    }
}
