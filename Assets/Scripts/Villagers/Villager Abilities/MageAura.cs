using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageAura : Aura
{
    protected override void OnEnterAura(Collider2D coll)
    {
        base.OnEnterAura(coll);

        Debug.Log("Entered Buff aura");
        coll.GetComponent<Villager>().SetDamageMult(((int)health + 1) / 2);
    }

    protected override void OnExitAura(Collider2D coll)
    {
        base.OnExitAura(coll);

        Debug.Log("No Buff aura");
        coll.GetComponent<Villager>().SetDamageMult(1);
    }
}
