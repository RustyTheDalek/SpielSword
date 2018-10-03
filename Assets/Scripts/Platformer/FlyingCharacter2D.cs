using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingCharacter2D : PlatformerCharacter2D
{
    [SerializeField] float m_MaxVelocity = 7f; 

    public void Move(Vector2 moveDir, float _MaxVelocity = 7f)
    {
        m_MaxVelocity = _MaxVelocity;

        m_Rigidbody2D.velocity = new Vector3(moveDir.x, moveDir.y, 0) * m_MoveForce * Time.deltaTime;
        m_Rigidbody2D.velocity = Vector2.ClampMagnitude(m_Rigidbody2D.velocity, m_MaxVelocity);
    }

    internal void SetMaxVelocity(float _MaxVelocity)
    {
        m_MaxVelocity = _MaxVelocity;
    }
}
