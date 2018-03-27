using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AuraVillager : Villager
{

    #region Private

    bool AuraActive;

    #endregion

    public override void Awake()
    {
        base.Awake();

        specialType = SpecialType.Press;
        animData["PlayerSpecialIsTrigger"] = true;
    }

    public override void OnSpecial(bool _PlayerSpecial)
    {
        if (!AuraActive)
        {
            animData["PlayerSpecial"] = _PlayerSpecial;
        }
    }

    public void SpawnAura()
    {
        if (villagerState == VillagerState.PresentVillager)
        {
            GameObject temp = Aura();
            temp.GetComponent<Aura>().creator = gameObject;

            AuraActive = true;
            animData["CanSpecial"] = false;
        }
    }

    protected abstract GameObject Aura();
}