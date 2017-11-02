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
            if (shieldStrength > 0 && animData.playerSpecial)
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
        if (!m_Animator)
        {
            m_Animator.runtimeAnimatorController = VillagerManager.villagerAnimators[2];
        }

        villagerAttackType = AttackType.Melee;
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
                    animData.canSpecial = false;
                }
                else if (shieldStrength > 0)
                {
                    animData.canSpecial = true;
                }

                //When the player is trying to use the shield and the shield has 
                //strength detract power  
                if (animData.playerSpecial && shieldStrength > 0)
                {
                    shieldStrength -= Time.deltaTime;
                }
                //otherwise if the shield is not in use and needs charging charge it up
                else if (!animData.playerSpecial && shieldStrength < 1)
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

    public override void OnSpecial(bool playerSpecial)
    {
        animData.playerSpecial = playerSpecial;
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
