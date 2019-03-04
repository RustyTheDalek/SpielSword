using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tracking for objects that split from parent on Death but we want them to come back 
/// on rewind
/// </summary>
public class PieceTracking : ObjectTrackBase
{
    Transform parent;

    Vector3 startPos;

    Rigidbody2D m_Rigidbody;
    Collider2D m_Collider;

    protected void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Collider = GetComponent<Collider2D>();

        parent = transform.parent;
        startPos = transform.localPosition;
    }

    public override void ResetToPresent()
    {
        m_Rigidbody.bodyType = RigidbodyType2D.Static;

        m_Collider.enabled = false;
        transform.SetParent(parent);
        transform.localPosition = startPos;

        gameObject.layer = LayerMask.NameToLayer("Minion");
    }

    public override void OnStartPlayback(int startFrame)
    {

        m_Rigidbody.bodyType = RigidbodyType2D.Static;

        m_Collider.enabled = false;
        transform.SetParent(parent);
        transform.localPosition = startPos;

        gameObject.layer = LayerMask.NameToLayer("Minion");
    }

    public override void OnStartReverse()
    {
        transform.SetParent(null);
    }
}
