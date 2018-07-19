using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpielSword : Villager
{
    //Rigidbody2D m_RigidBody;

    public BoxCollider2D spielsword;

    // Use this for initialization
    public void Start()
    {
        specialType = SpecialType.Hold;
    }

    public void Spielcrafice()
    {
        Debug.Log("Spielcrafice complete");
        this.Kill();
    }

    public void SetBodyType(RigidbodyType2D type)
    {
        m_rigidbody.bodyType = type;
        m_rigidbody.velocity = Vector3.zero;
    }

    public void EnableSpiel()
    {
        spielsword.enabled = true;

        //Debug.Break();
    }

    public void DisableSpiel()
    {
        //Debug.Break();
        canSpecial = false;
        spielsword.enabled = false;
    }
}
