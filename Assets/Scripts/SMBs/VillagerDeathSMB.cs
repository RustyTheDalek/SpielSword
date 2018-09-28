using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerDeathSMB : SceneLinkedSMB<Villager>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateEnter(animator, stateInfo, layerIndex);

        if (m_MonoBehaviour.villagerState == VillagerState.PresentVillager)
        {
            m_MonoBehaviour.NamedLog("Dying, adjusting animator");

            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        }
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);

        if(stateInfo.normalizedTime >= 1 && 
            m_MonoBehaviour.villagerState == VillagerState.PresentVillager &&
            m_MonoBehaviour.deathEnd == false)
        {
            m_MonoBehaviour.vTO.finishFrame = (int)TimeObjectManager.t;
            m_MonoBehaviour.deathEnd = true;

            animator.updateMode = AnimatorUpdateMode.Normal;
        }
    }
}
