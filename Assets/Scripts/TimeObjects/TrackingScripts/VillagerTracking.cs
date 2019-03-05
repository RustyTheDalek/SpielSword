using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerTracking : ObjectTrackBase
{
    /// <summary>
    /// Frames to track whether the Villager is alive
    /// </summary>
    protected List<bool> vFrames = new List<bool>();

    protected Villager m_Villager;

    public void Awake()
    {
        m_Villager = GetComponent<Villager>();
    }

    public override void PlayFrame(int currentFrame)
    {
        m_Villager.Alive = vFrames[currentFrame];
    }

    public override void ResetToPresent()
    {
        vFrames.Clear();
        m_Villager.enabled = true;
    }

    public override void TrackFrame()
    {
        vFrames.Add(m_Villager.Alive);
    }
}
