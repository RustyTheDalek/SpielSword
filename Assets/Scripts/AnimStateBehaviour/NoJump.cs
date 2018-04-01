using UnityEngine;

/// <summary>
/// Prevents jump from overriding other animations
/// TODO: Change this as it's a janky ass logic, may want improve this by having login 
/// in PlatformerCharacter2D that overrides attack 
/// Created on : Ian Jones      - 04/11/17
/// Updated by : Ian Jones      - 01/04/18
/// </summary>
public class NoJump : StateMachineBehaviour {

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("NoJump", true);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("NoJump", false);
    }
}
