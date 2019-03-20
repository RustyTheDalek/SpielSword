using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionGibTracking : ObjectTrackBase
{
    Transform m_Parent;

    Vector3 m_StartPos;

    Rigidbody2D m_Rigidbody;

    Collider2D m_Collider;

    private void Awake()
    {
        m_Parent = transform.parent;
        m_StartPos = transform.localPosition;
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Collider = GetComponent<Collider2D>();
    }

    public override void ResetToPresent()
    {
        transform.SetParent(m_Parent);
        transform.localPosition = m_StartPos;

        m_Rigidbody.bodyType = RigidbodyType2D.Static;
        m_Collider.enabled = false;

        gameObject.layer = LayerMask.NameToLayer("Minion");
    }

    public void Throw(Vector2 direction)
    {
        m_Rigidbody.bodyType = RigidbodyType2D.Dynamic;

        Vector2 throwforce = (new Vector2(
                direction.x * Random.Range(2f, 10f),
                direction.y * Random.Range(1f, 5f)) * (m_Rigidbody.mass * m_Rigidbody.mass));

        Debug.DrawRay(transform.position, throwforce, Color.red, 5f);

        m_Rigidbody.AddForce(throwforce, ForceMode2D.Impulse);
        m_Collider.enabled = true;
        transform.SetParent(null);

        gameObject.layer = LayerMask.NameToLayer("Bits");
    }
}
