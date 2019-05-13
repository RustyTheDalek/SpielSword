using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTrapperSpawnSMB : SceneLinkedSMB<WallTrapMinion>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateEnter(animator, stateInfo, layerIndex);

        m_MonoBehaviour.pData.moveDir = Vector2.zero;
        m_MonoBehaviour.pData.velocityDampen = 1;
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator,
    AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);
        m_MonoBehaviour.MinionSpawning();
    }
}