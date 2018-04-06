using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTimeObject : SpriteTimeObject {

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

    //protected override void Start()
    //{
    //    TimeObjectManager.bossObjs.Add(this);
    //}

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
