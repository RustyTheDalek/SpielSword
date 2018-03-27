using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestAura : Aura
{
    protected override void OnEnterAura(Collider2D coll)
    {
        base.OnEnterAura(coll);

        //Debug.Log("Entered Protection aura");
        coll.GetComponent<Villager>().shielded = true;
    }

    protected override void OnExitAura(Collider2D coll)
    {
        base.OnExitAura(coll);

        //Debug.Log("Exited Protection aura");
        coll.GetComponent<Villager>().shielded = false;
    }
}
