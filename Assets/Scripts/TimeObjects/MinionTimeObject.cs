using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionTimeObject : PlatformerTimeObject {

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();

        OnStartPlayback += ResetTracking;
    }

    private void ResetTracking()
    {
        bFrames.Clear();
        pFrames.Clear();
        sFrames.Clear();

        tObjectState = TimeObjectState.Present;

        finishFrame = 0;

        m_Sprite.color = new Color(m_Sprite.color.r, m_Sprite.color.g, m_Sprite.color.b, 1f);
    }

    private void OnDestroy()
    {
        OnStartPlayback -= ResetTracking;
    }


}
