using UnityEngine;

/// <summary>
/// Tracks Villagers Marty point properly to allow rewind   
/// Created on : Ian Jones      - 27/10/17
/// Updated by : Ian Jones      - 01/04/18
/// </summary>
public class VillagerMarty : StateMachineBehaviour {

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Tracks the in which the Villager dies so the Aninmation can be reversed when needed
        //if (stateInfo.normalizedTime >= 1 && animator.GetComponent<Villager>().villagerState == VillagerState.PresentVillager 
        //    && animator.GetComponent<VillagerTimeObject>().endRecorded == false && 
        //    animator.GetComponent<VillagerTimeObject>().deathOrMarty == false)
        //{
        //    //Debug.Log("Death recorded");
        //    animator.GetComponent<VillagerTimeObject>().endFinish = true;
        //}
    }
}
