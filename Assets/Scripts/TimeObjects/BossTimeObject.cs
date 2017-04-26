using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTimeObject : SpriteTimeObject {

    protected override void OnStartPlayback()
    {
        //Not called for some reason
        frames.Clear();
        tObjectState = TimeObjectState.Present;
        finishFrame = 0;
    }
}
