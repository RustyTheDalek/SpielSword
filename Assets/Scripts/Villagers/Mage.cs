using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : AuraVillager {

    static GameObject _MageAura;

    public static GameObject MageAura
    {
        get
        {
            if (_MageAura == null)
            {
                _MageAura = new GameObject();

                UnityEngine.Object obj = Resources.Load("MageAura");

                _MageAura = (GameObject)obj;
                _MageAura.CreatePool(25);
            }

            return _MageAura;
        }
    }

    // Use this for initialization
    public override void Start ()
    {
        base.Start();
        attackType = AttackType.Ranged;
	}

    protected override Aura Aura()
    {
        return MageAura.Spawn(transform.position).GetComponent<Aura>();
    }
}
