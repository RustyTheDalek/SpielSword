using UnityEngine;
using System.Collections;

public class VillagerDeath : StateMachineBehaviour {

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Tracks the in which the Villager dies so the Aninmation can be reversed when needed
        if (stateInfo.normalizedTime >= 1 && animator.GetComponent<Villager>().villagerState == VillagerState.PresentVillager 
            && animator.GetComponent<VillagerTimeObject>().deathRecorded == false)
        {
            Debug.Log("Death recorded");
            animator.GetComponent<VillagerTimeObject>().deathFinish = true;
        }
        ////If Time is rewinding and the Villager is at the start of his death Animation, allow him to reverse out
        //if (stateInfo.normalizedTime <= 0 && Game.timeState == TimeState.Backward)
        //{
        //    Debug.Log("Villager Undying");
        //    animator.SetTrigger("ExitDeath");
        //}
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if (layerIndex == 0 && animator.GetComponent<Villager>().villagerState == VillagerState.PresentVillager)
        //{
        //    Debug.Log("Death finished");
        //    animator.GetComponent<VillagerTimeObject>().deathFinish = true;
        //}
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
