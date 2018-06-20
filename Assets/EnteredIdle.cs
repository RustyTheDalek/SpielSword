using UnityEngine;

/// <summary>
/// Tracks When boss enters idle state to allow next Attacks
/// Created by : Ian Jones - 20/06/18
/// </summary>
public class EnteredIdle : StateMachineBehaviour {

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<BossManager>().attacking = false;
    }
}
