using UnityEngine;

//TODO:Change this to be something better not sure I like it

/// <summary>
/// Keeps track of when boss is ready to start fight
/// Created on : Ian Jones      - 27/08/17
/// Updated by : Ian Jones      - 01/04/18
/// </summary>
public class BossReadiness : StateMachineBehaviour {

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Game.bossReady = false;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Game.bossReady = true;
    }
}
