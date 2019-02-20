using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Villager class that sacrfiices Speed(?) For Damage
/// </summary>
public class Berserker : Villager
{
    bool    sacrificing = false,
            rage = false;
    public void OnBerserkerRage()
    {
        if (villagerState == VillagerState.PresentVillager && !rage)
        {
            Debug.Log("I'm RAGING");
            rage = true;

            SetDamageMult((int)damageMult * 2);

            m_rigidbody.transform.localScale = new Vector3(2, 2, 1);
            m_Sprite.transform.localScale = new Vector3(2, 2, 1);
        }
    }

    public void StartSacrifice()
    {
        if (canSpecial)
        {
            Debug.Log("Sacrifice Starting");

            SetBodyType(RigidbodyType2D.Kinematic);
            canMove = false;
            sacrificing = true;
        }
    }

    public void FinishSacrifice()
    {
        Debug.Log("Sacrifice Complete");
        SetBodyType(RigidbodyType2D.Dynamic);
        sacrificing = false;
        canSpecial = false;
        special1 = false;
        special2 = false;
        canMove = true;
        Kill();
    }

    public void SetBodyType(RigidbodyType2D type)
    {
        m_rigidbody.bodyType = type;
        m_rigidbody.velocity = Vector3.zero;
        moveDir = Vector2.zero;
    }
}
