using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpielSword : Villager
{
    Rigidbody2D m_RigidBody;

    public BoxCollider2D spielsword;

    // Use this for initialization
    public override void Start()
    {
        //base.Start();

        specialType = SpecialType.Hold;

        m_RigidBody = GetComponent<Rigidbody2D>();
    }

    public void Spielcrafice()
    {
        Debug.Log("Spielcrafice complete");
        this.Kill();
    }

    public void SetBodyType(RigidbodyType2D type)
    {
        m_RigidBody.bodyType = type;
        m_RigidBody.velocity = Vector3.zero;
    }

    public void EnableSpiel()
    {
        spielsword.enabled = true;

        //Debug.Break();
    }

    public void DisableSpiel()
    {
        //Debug.Break();
        spielsword.enabled = false;
    }
}
