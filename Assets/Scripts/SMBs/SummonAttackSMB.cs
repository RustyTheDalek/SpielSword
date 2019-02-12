using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonAttackSMB : SceneLinkedSMB<Summon>
{
    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateExit(animator, stateInfo, layerIndex);

        m_MonoBehaviour.Kill();
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);

        if(stateInfo.normalizedTime >= 1)
        {
            m_MonoBehaviour.Kill();
        }
    }
}
