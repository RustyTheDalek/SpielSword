using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageAura : Aura
{
    protected override void OnEnterAura(Villager villager)
    {
        base.OnEnterAura(villager);

        villager.SetDamageMult(((int)health + 1) / 2);
    }

    protected override void OnExitAura(Villager villager)
    {
        base.OnExitAura(villager);
        villager.SetDamageMult(1);
    }
}
