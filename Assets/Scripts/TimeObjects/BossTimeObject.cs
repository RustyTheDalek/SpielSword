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

    Animator _Animator;

    protected override void Awake()
    {
        base.Awake();

        startFrame = Game.t;

        _Animator = GetComponent<Animator>();

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

    void OnAnimatorMove()
    {
        _Animator.speed = Game.PastTimeScale;
    }
}
