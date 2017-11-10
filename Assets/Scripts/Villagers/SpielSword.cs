using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpielSword : Villager
{

    // Use this for initialization
    public override void Start()
    {
        RuntimeAnimatorController temp;
        AssetManager.VillagerAnimators.TryGetValue("Warlock", out temp);
        m_Animator.runtimeAnimatorController = temp;

        base.Start();

        specialType = SpecialType.Hold;
    }

    public void Spielcrafice()
    {
        Debug.Log("Spielcrafice complete");
    }
}
