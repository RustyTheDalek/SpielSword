using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightMinionStuckSMB : SceneLinkedSMB<FlightMinion>
{
    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateExit(animator, stateInfo, layerIndex);

        m_MonoBehaviour.OnUnstuck();
    }
}
