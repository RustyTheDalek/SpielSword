using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeObjectDeathSMB : SceneLinkedSMB<TimeObject>
{ 
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);

        if(stateInfo.normalizedTime >= 1 &&
            m_MonoBehaviour.tObjectState == TimeObjectState.Present &&
            m_MonoBehaviour.finished == false)
        {
            m_MonoBehaviour.FinishTracking();
        }
    }
}
