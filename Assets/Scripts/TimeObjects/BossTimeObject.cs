using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Maintains logic for Boss objects
/// Created by : Ian Jones - 26/04/17
/// Updated by : Ian Jones - 06/04/18
/// </summary>
public class BossTimeObject : RigidbodyTimeObject
{
    private SpriteFrameData tempFrame;
    private List<SpriteFrameData> frames = new List<SpriteFrameData>();

    protected override void Awake()
    {
        base.Awake();

        OnStartPlayback += OnStartBossTimeObjectPlayback;
    }

    protected void OnStartBossTimeObjectPlayback()
    {
        //Not called for some reason
        frames.Clear();
        bFrames.Clear();
        sFrames.Clear();
        tObjectState = TimeObjectState.Present;
        finishFrame = 0;
        m_Sprite.color = new Color(m_Sprite.color.r, m_Sprite.color.g, m_Sprite.color.b, 1f);
    }

    private void OnDestroy()
    {
        OnStartPlayback -= OnStartBossTimeObjectPlayback;
    }
}
