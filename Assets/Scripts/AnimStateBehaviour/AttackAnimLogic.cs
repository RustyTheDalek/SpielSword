using UnityEngine;
using System.Collections;

/// <summary>
/// Prevents Player from just holding down attack and also provides accurate tracking of attacks
/// Created on : Ian Jones      - 27/08/17
/// Updated by : Ian Jones      - 01/04/18
/// </summary>
public class AttackAnimLogic : StateMachineBehaviour {

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (layerIndex == 0 && animator.GetComponent<Villager>().villagerState == VillagerState.PresentVillager)
        {
            //Debug.Log("Start Attack");
            animator.GetComponent<VillagerTimeObject>().attackStart = true;
        }

        if (TimeObjectManager.timeState == TimeState.Forward)
        {
            animator.SetBool("CanAttack", false);
        }
        //animator.GetComponentInChildren<MeleeAttack>().GetComponent<CircleCollider2D>().enabled = true;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (TimeObjectManager.timeState == TimeState.Backward)
        {
            animator.SetBool("CanAttack", false);
        }
    }
}
