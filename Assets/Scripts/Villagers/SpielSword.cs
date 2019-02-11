using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpielSword : Villager
{
    bool sacrificing = false;

    //public override void OnSpecial(bool _PlayerSpecial)
    //{
    //    if (sacrificing && canSpecial)
    //    {
    //        playerSpecial = true;
    //    }
    //    else
    //    {
    //        base.OnSpecial(_PlayerSpecial);
    //    }
    //}

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
