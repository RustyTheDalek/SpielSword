using UnityEngine;

public class BubbleSMB : SceneLinkedSMB<Minion>
{
    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateExit(animator, stateInfo, layerIndex);

        //After Bubbling is done go back to patrolling to allow minion retreat and attack
        m_MonoBehaviour.state = MinionState.Patrolling;
    }
}
