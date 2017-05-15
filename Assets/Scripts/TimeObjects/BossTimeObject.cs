using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTimeObject : SpriteTimeObject {

    Animator _Animator;

    protected override void Start()
    {
        base.Start();
        _Animator = GetComponent<Animator>();
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
