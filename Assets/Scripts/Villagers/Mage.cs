using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Villager {

    #region Protected

    #endregion

    #region Private

    bool AuraActive;

    #endregion

    public override void Awake()
    {
        base.Awake();

        specialType = SpecialType.Press;
        animData.playerSpecialIsTrigger = true;

    }

    // Use this for initialization
    public override void Start ()
    {
        m_Animator.runtimeAnimatorController = AssetManager.VillagerAnimators[0];
        villagerAttackType = AttackType.Ranged;
        base.Start();	
	}
	
	// Update is called once per frame
	public override void Update ()
    {
        base.Update();
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
            VillagerManager.auras.Add(AssetManager.Aura.Spawn(transform.position).GetComponent<MageAura>());

            AuraActive = true;
            animData.canSpecial = false;
        }
    }
}
