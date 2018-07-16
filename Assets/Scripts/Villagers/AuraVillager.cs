using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AuraVillager : Villager
{

    #region Private

    bool AuraActive;

    Aura currentAura;

    #endregion

    protected override void Awake()
    {
        base.Awake();

        specialType = SpecialType.Press;
        animData["PlayerSpecialIsTrigger"] = true;
    }

    public void Setup(VillagerManager vilManager)
    {
        currentAura = Aura();
        currentAura.gameObject.SetActive(false);
        currentAura.creator = this;
        currentAura.OnEnterAuraEvent += vilManager.IncCombosUsed;
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
            currentAura.gameObject.SetActive(true);
            currentAura.transform.position = transform.position;
            AuraActive = true;
            animData["CanSpecial"] = false;
        }
    }

    protected abstract Aura Aura();

    public void Unsubscribe(VillagerManager villagerManager)
    {
        currentAura.OnEnterAuraEvent -= villagerManager.IncCombosUsed;
    }
}