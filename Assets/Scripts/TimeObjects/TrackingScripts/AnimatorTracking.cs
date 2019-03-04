using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorTracking : ObjectTrackBase
{
    protected Animator m_Anim;

    public void Awake()
    {
        m_Anim = GetComponent<Animator>();
    }

    public override void ResetToPresent()
    {
        m_Anim.enabled = true;
    }

    public override void OnStartReverse()
    {
        m_Anim.enabled = false;
    }
}
