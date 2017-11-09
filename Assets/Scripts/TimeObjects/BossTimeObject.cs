using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTimeObject : SpriteTimeObject {

    private SpriteFrameData tempFrame;
    private List<SpriteFrameData> frames = new List<SpriteFrameData>();

    Animator _Animator;

    protected override void Start()
    {
        startFrame = Game.t;

        m_Sprite = GetComponent<SpriteRenderer>();

        if (GetComponent<VHSEffect>())
        {
            vhsEffect = GetComponent<VHSEffect>();
        }
        else
        {
            vhsEffect = gameObject.AddComponent<VHSEffect>();
        }

        _Animator = GetComponent<Animator>();

        TimeObjectManager.bossObjs.Add(this);
    }

    protected override void OnStartPlayback()
    {
        //Not called for some reason
        frames.Clear();
        tObjectState = TimeObjectState.Present;
        finishFrame = 0;
    }

    void OnAnimatorMove()
    {
        _Animator.speed = Game.PastTimeScale;

    }
}
