using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerMotion2D : MonoBehaviour
{
    Rigidbody2D m_Rigidbody2D;

    protected void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //When player collides with Ground for first time then reduce xVelocity
        //if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        //{
        //    Debug.Log("hitting floor");
        //    m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
        //    m_Rigidbody2D.angularVelocity = 0;
        //}

        if (collision.collider.tag == "Wall")
        {
            Debug.Log("hitting Wall");
            m_Rigidbody2D.velocity = Vector2.zero;
            m_Rigidbody2D.angularVelocity = 0;
        }
    }
}
