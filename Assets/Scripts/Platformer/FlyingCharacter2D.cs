using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingCharacter2D : PlatformerCharacter2D
{
    public override void Move(Hashtable animData)
    {
        base.Move();

        //m_Rigidbody2D.AddForce((Vector2)animData["MoveDir"] * m_MoveForce * Time.deltaTime, ForceMode2D.Impulse);
    }
}
