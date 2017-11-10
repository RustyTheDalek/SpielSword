using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AuraVillager : Villager
{

    #region Private

    bool AuraActive;

    #endregion

    public override void Start()
    {
        RuntimeAnimatorController temp;
        AssetManager.VillagerAnimators.TryGetValue("Aura", out temp);
        m_Animator.runtimeAnimatorController = temp;

        base.Start();
    }

    public override void Awake()
    {
        base.Awake();

        specialType = SpecialType.Press;
        animData.playerSpecialIsTrigger = true;
    }

    public override void OnSpecial(bool _PlayerSpecial)
    {
        if (!AuraActive)
        {
            animData.playerSpecial = _PlayerSpecial;
        }
    }

    public void SpawnAura()
    {
        if (villagerState == VillagerState.PresentVillager)
        {
            VillagerManager.auras.Add(Aura().GetComponent<MageAura>());

            AuraActive = true;
            animData.canSpecial = false;
        }
    }

    protected abstract GameObject Aura();
}