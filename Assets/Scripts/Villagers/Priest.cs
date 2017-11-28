﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Priest : AuraVillager
{
    public override void Start()
    {
        base.Start();
        attackType = AttackType.Melee;
    }

    protected override GameObject Aura()
    {
        return AssetManager.PriestAura.Spawn(transform.position);
    }
}