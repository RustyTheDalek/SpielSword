using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Villager class that sacrfiices Speed(?) For Damage
/// </summary>
public class Berserker : Villager
{
    public void OnBerserkerRage()
    {
        if (villagerState == VillagerState.PresentVillager)
        {
            Debug.Log("I'm RAGING");
            canSpecial = false;

            SetDamageMult((int)damageMult * 2);

            transform.localScale = new Vector3(2, 2, 1);
        }
    }
}
