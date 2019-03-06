using System.Collections.Generic;
using UnityEngine;

public class ColliderTracking : ObjectTrackBase
{
    protected Collider2D m_Collider;

    private List<bool> collFrames = new List<bool>();

    protected void Awake()
    {
        m_Collider = GetComponent<Collider2D>();
    }

    public override void ResetToPresent()
    {
        collFrames.Clear();
    }

    public override void PlayFrame(int currentFrame)
    {
        if(collFrames.WithinRange(currentFrame))
            m_Collider.enabled = (collFrames[currentFrame]);
    }

    public override void TrackFrame()
    {
        collFrames.Add(m_Collider.enabled);
    }
}
