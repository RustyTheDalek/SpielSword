using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AuraVillager : Villager
{

    #region Private Variables

    bool AuraActive = false;

    Aura currentAura;

    #endregion

    protected override void Awake()
    {
        base.Awake();

        currentAura = Aura();
        currentAura.gameObject.SetActive(false);
        currentAura.creator = this;
    }

    public void Setup(VillagerManager vilManager)
    {

        currentAura.OnEnterAuraEvent += vilManager.IncCombosUsed;
    }

    public override void OnSpecial(bool _PlayerSpecial)
    {
        if (!AuraActive)
        {
            base.OnSpecial(_PlayerSpecial);
        }
    }

    public void SpawnAura()
    {
        if (villagerState == VillagerState.PresentVillager)
        {
            currentAura.gameObject.SetActive(true);
            currentAura.transform.position = transform.position;
            AuraActive = true;
            canSpecial = false;
        }
    }

    protected abstract Aura Aura();

    public void Unsubscribe(VillagerManager villagerManager)
    {
        currentAura.OnEnterAuraEvent -= villagerManager.IncCombosUsed;
    }
}