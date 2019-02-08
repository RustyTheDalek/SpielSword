using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script summary
/// </summary>
public class MinionCelebrateSMB : SceneLinkedSMB<Minion> 
{

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateExit(animator, stateInfo, layerIndex);
        m_MonoBehaviour.StopCelebrate();
    }
}
