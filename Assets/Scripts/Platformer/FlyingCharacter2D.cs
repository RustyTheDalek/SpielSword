using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingCharacter2D : PlatformerCharacter2D
{
    [SerializeField] float m_MaxVelocity = 7f; 

    public void Move(Vector2 moveDir, float _MaxVelocity = 7f)
    {
        base.Move(moveDir);

        m_MaxVelocity = _MaxVelocity;

        m_Rigidbody2D.AddForce(moveDir * m_MoveForce * Time.deltaTime, ForceMode2D.Impulse);

        m_Rigidbody2D.velocity = Vector2.ClampMagnitude(m_Rigidbody2D.velocity, m_MaxVelocity);
    }
}
