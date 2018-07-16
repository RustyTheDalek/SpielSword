using UnityEngine;

/// <summary>
/// Tracks when the Villagers dies to accurately reverse it
/// Created on : Ian Jones      - 06/09/16
/// Updated by : Ian Jones      - 01/04/18
/// </summary>
public class VillagerDeath : StateMachineBehaviour {

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Tracks the in which the Villager dies so the Aninmation can be reversed when needed
        if (stateInfo.normalizedTime >= 1 && animator.GetComponent<Villager>().villagerState == VillagerState.PresentVillager 
            && animator.GetComponent<Villager>().deathEnd == false)
        {
            //Debug.Log("Death recorded");
            animator.GetComponent<Villager>().deathEnd = true;
        }
    }
}
