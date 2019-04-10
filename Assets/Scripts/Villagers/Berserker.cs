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

    protected override void OnEnable()
    {
        base.OnEnable();

        SceneLinkedSMB<Berserker>.Initialise(m_Animator, this);
    }

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

    /// <summary>
    /// This is called sacrifice is stopped, early or when it reaches its conclusion
    /// </summary>
    public void StopSacrifice()
    {
        SetBodyType(RigidbodyType2D.Dynamic);
        canMove = true;
        sacrificing = false;
    }

    /// <summary>
    /// Only called at the end of the animation when sacrifice is compelte
    /// </summary>
    public void FinishSacrifice()
    {
        Debug.Log("Sacrifice Complete");
        StopSacrifice();

        canSpecial = false;
        special1 = false;
        special2 = false;
        
        Kill();
    }

    public void SetBodyType(RigidbodyType2D type)
    {
        m_rigidbody.bodyType = type;
        m_rigidbody.velocity = Vector3.zero;
        pData.moveDir = Vector2.zero;
    }
}
