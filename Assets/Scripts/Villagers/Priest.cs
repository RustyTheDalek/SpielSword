using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Priest : AuraVillager
{
    static GameObject _PriestAura;

    public static GameObject PriestAura
    {
        get
        {
            if (_PriestAura == null)
            {
                _PriestAura = new GameObject();

                UnityEngine.Object obj = Resources.Load("PriestAura");

                _PriestAura = (GameObject)obj;
                _PriestAura.CreatePool(25);
            }

            return _PriestAura;
        }
    }

    protected override Aura Aura()
    {
        return PriestAura.Spawn().GetComponent<Aura>();
    }
}