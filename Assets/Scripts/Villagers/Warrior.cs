using UnityEngine;
using System.Collections;

/// <summary>
/// Warrior Class that allows player to deflect damage
/// </summary>
public class Warrior : Villager
{
    public float shieldStrength = 1;

    /// <summary>
    /// Whether the Villager is shielded from damage
    /// </summary>
    public bool Shielded
    {
        get
        {
            if (shieldStrength > 0 && (bool)animData["PlayerSpecial"])
                return true;
            else
                return false;
        }
    }

    public override void Awake()
    {
        base.Awake();

        specialType = SpecialType.Hold;
    }

    // Use this for initialization
    public override void Start()
    {
        RuntimeAnimatorController temp;
        AssetManager.VillagerAnimators.TryGetValue("Warlock", out temp);
        m_Animator.runtimeAnimatorController = temp;

        attackType = AttackType.Melee;
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        switch (villagerState)
        {
            case VillagerState.PresentVillager:

                //If the Shield has been used too long we need to disable the Players 
                //ability to special and renable when the shield is not in use
                if (shieldStrength <= 0)
                {
                    animData["CanSpecial"]= false;
                }
                else if (shieldStrength > 0)
                {
                    animData["CanSpecial"] = true;
                }

                //When the player is trying to use the shield and the shield has 
                //strength detract power  
                if ((bool)animData["PlayerSpecial"] && shieldStrength > 0)
                {
                    shieldStrength -= Time.deltaTime;
                }
                //otherwise if the shield is not in use and needs charging charge it up
                else if (!(bool)animData["PlayerSpecial"] && shieldStrength < 1)
                {
                    shieldStrength += Time.deltaTime;
                }
                else if (shieldStrength > 1)
                {
                    shieldStrength = 1;
                }
                
                break;
        }
    }

    public override void OnHit()
    {
        if (!Shielded)
        {
            base.OnHit();
        }
    }

    public override void OnPastHit(Collider2D collider)
    {
        if (!Shielded)
        {
            base.OnPastHit(collider);
        }
    }
}
