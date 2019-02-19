using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Villager
{
    [Tooltip("How many charges Mage has")]
    [Range(1, 5)]
    public int charges = 5;

    public float magicMissileChargeTime = 1,
                 fireballChargeTime = 2.5f;

    public List<SpriteRenderer> m_ChargeAuras;

    float   timer = 0,
            rechargeTime;

    protected override void Update()
    {
        base.Update();

        if(!special1 && charges < 5)
        {
            timer += Time.deltaTime;

            if (timer >= rechargeTime)
            {
                charges++;
                m_ChargeAuras[charges - 1].enabled = true;
                timer = 0;

                if(charges == 5 && rechargeTime == fireballChargeTime)
                {
                    rechargeTime = magicMissileChargeTime;
                    canSpecial = true;
                }
            }
        }
    }

    public void FireMagicMissile()
    {
        if (villagerState == VillagerState.PresentVillager)
        {
            timer = 0;
            MagicMissile mMissile;

            for (int i = 0; i < charges; i++)
            {
                GameObject gObject = abilities.LoadAsset<GameObject>("MagicMissile").Spawn(m_ChargeAuras[i].transform.position);
                mMissile = gObject.GetComponent<MagicMissile>();
                mMissile.damageMult = damageMult;
            }

            if (charges > 1)
            {
                m_ChargeAuras[charges - 1].enabled = false;
                charges--;
            }

            rechargeTime = magicMissileChargeTime;
        }
    }

    public void SummonFireball()
    {
        if (charges == 5)
        {
            FireNamedProjectile("Fireball");
            charges = 0;

            foreach (SpriteRenderer chargeAura in m_ChargeAuras)
            {
                chargeAura.enabled = false;
            }

            rechargeTime = fireballChargeTime;
            canSpecial = false;
        }
    }
}
