using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script summary
/// </summary>
public class MinionAttackSMB : SceneLinkedSMB<Minion> 
{
    public override void OnSLStateNoTransitionUpdate(Animator animator,
        AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);

        m_MonoBehaviour.Attack();
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateExit(animator, stateInfo, layerIndex);

        //m_MonoBehaviour.StopAttack();
    }
}
