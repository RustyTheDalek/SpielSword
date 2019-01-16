using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestAura : Aura
{
    protected override void OnEnterAura(Villager villager)
    {
        base.OnEnterAura(villager);

        villager.shielded = true;
    }

    protected override void OnExitAura(Villager villager)
    {
        base.OnExitAura(villager);

        villager.shielded = false;
    }
}
