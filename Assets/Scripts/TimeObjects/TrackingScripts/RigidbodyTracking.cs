using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyTracking : ObjectTrackBase
{
    Rigidbody2D m_Rigidbody;

    protected void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    public override void ResetToPresent()
    {
        if (objectTrackType.HasFlag(ObjectTrackType.EventTracking))
            m_Rigidbody.simulated = true;
    }

    public override void OnStartPlayback(int startFrame)
    {
        m_Rigidbody.simulated = true;
    }

    public override void OnFinishPlayback()
    {
        m_Rigidbody.simulated = false;
    }

    public override void OnStartReverse()
    {
        m_Rigidbody.simulated = false;
    }
}
