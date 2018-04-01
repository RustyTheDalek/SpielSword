using UnityEngine;

/// <summary>
/// Place this scripts on the idle animations for boss and it keeps track of when that body part is not attacking
/// Created on : Ian Jones      - 27/10/17
/// Updated by : Ian Jones      - 01/04/18
/// </summary>
public class IdleCheck : StateMachineBehaviour {

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponentInParent<BossAttack>().attacking = false;
    }   
}
