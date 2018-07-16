using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingCharacter2D : PlatformerCharacter2D
{
    [SerializeField] float m_MaxVelocity = 15; 

    public override void Move(Hashtable animData)
    {
        base.Move(animData);

        m_Rigidbody2D.AddForce(moveDir * m_MoveForce * Time.deltaTime, ForceMode2D.Impulse);

        m_Rigidbody2D.velocity = Vector2.ClampMagnitude(m_Rigidbody2D.velocity, m_MaxVelocity);
    }
}
