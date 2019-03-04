using UnityEngine;
using System.Collections;

/// <summary>
/// Paladin Class that protects themself with a shield and others with a protective Aura
/// </summary>
public class Paladin : Villager
{
    float shieldStrength = 2;

    float channelSpeed = 7.5f;

    public Aura m_Aura;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();

        if (!Alive)
            return;

        switch (villagerState)
        {
            case VillagerState.PresentVillager:

                //If the Shield has been used too long we need to disable the Players 
                //ability to special and renable when the shield is not in use
                canSpecial = shieldStrength <= 0 ? false : true;

                //When the player is trying to use the shield and the shield has 
                //strength detract power  
                if ((special1 || special2) && shieldStrength > 0)
                {
                    if(special1) //Paladins 1st special just protects themself
                    {
                        shielded = true;
                    }

                    m_Aura.m_Sprite.color = m_Aura.m_Sprite.color.SetAlpha(shieldStrength);

                    shieldStrength -= Time.deltaTime;

                    moveSpeed = channelSpeed;
                }
                else if ((special1 || special2) && shieldStrength < 1)
                {
                    shielded = false;
                }
                //otherwise if the shield is not in use and needs charging charge it up
                else if (!(special1 || special2) && shieldStrength < 1)
                {
                    shielded = false;

                    shieldStrength += Time.deltaTime;

                    moveSpeed = maxSpeed;
                }
                else if (shieldStrength > 1)
                {
                    shieldStrength = 1;
                }

                if(!canSpecial)
                {
                    shielded = false;
                }

                m_Aura.SetAura(special2 && canSpecial);
                
                break;
        }
    }

    public void Setup(VillagerManager vilManager)
    {
        m_Aura.OnEnterAuraEvent += vilManager.IncCombosUsed;
    }

    public void Unsubscribe(VillagerManager villagerManager)
    {
        m_Aura.OnEnterAuraEvent -= villagerManager.IncCombosUsed;
    }

    public override void OnDeath(Vector2 attackDirection)
    {
        base.OnDeath(attackDirection);

        //TODO:Detach protection aura if it active
        if(special2 && villagerState == VillagerState.PresentVillager)
        {
            m_Aura.Detach();
        }
    }
}
