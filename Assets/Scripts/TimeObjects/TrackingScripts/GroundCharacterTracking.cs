using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCharacterTracking : ObjectTrackBase
{
    protected GroundCharacter2D m_Ground;

    protected void Awake()
    {
        m_Ground = GetComponent<GroundCharacter2D>();
    }

    public override void ResetToPresent()
    {
        m_Ground.enabled = true;
    }

    public override void OnStartReverse()
    {
        m_Ground.enabled = false;
    }
}
