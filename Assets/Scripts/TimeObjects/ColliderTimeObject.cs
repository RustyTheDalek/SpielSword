using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTimeObject : TimeObject
{
    Collider2D m_Collider;

    private List<bool> collFrames = new List<bool>();

    protected override void Awake()
    {
        base.Awake();

        m_Collider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        OnPlayFrame += PlayCollider;
        OnTrackFrame += TrackCollider;
    }

    protected void TrackCollider()
    {
        collFrames.Add(m_Collider.enabled);
    }

    protected void PlayCollider()
    {
        m_Collider.enabled = (collFrames[(int)currentFrame]);
    }

    protected void OnDestroy()
    {
        OnPlayFrame -= PlayCollider;
        OnTrackFrame -= TrackCollider;
    }

}
