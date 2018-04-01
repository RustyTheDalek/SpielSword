using UnityEngine;

/// <summary>
/// Enable attacks for melee characters
/// TODO: Remove this and do it using animations this is clunky as it means the player is attacking for the whole animation
/// Created on : Ian Jones      - 13/09/16
/// Updated by : Ian Jones      - 01/04/18
/// </summary>
public class MeleeAttackAnim : StateMachineBehaviour {

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponentInChildren<MeleeAttack>().GetComponent<CircleCollider2D>().enabled = true;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponentInChildren<MeleeAttack>().GetComponent<CircleCollider2D>().enabled = false;
    }
}
