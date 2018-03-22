using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Villager class that sacrfiices Speed(?) For Damage
/// </summary>
public class Berserker : Villager
{
    public override void Start()
    {
        base.Start();

        specialType = SpecialType.Hold;
    }

    public void OnBerserkerRage()
    {
        if (villagerState == VillagerState.PresentVillager)
        {
            Debug.Log("I'm RAGING");
            animData["CanSpecial"] = false;

            SetDamageMult((int)damageMult * 2);

            transform.localScale *= 2;
        }
    }
}
