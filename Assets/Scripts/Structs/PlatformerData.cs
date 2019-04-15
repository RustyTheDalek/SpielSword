using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlatformerData
{
    public Vector2 moveDir;

    /// <summary>
    /// Maximum total Velocity
    /// </summary>
    public float maxVelocity;

    /// <summary>
    /// Dampening force Max of 1 for 100%reduction
    /// </summary>
    public float velocityDampen
    {
        get
        {
            return m_VelocityDampen;
        }
        set
        {
            m_VelocityDampen = Mathf.Clamp(value, 0, 1);
        }
    }
    private float m_VelocityDampen;
}