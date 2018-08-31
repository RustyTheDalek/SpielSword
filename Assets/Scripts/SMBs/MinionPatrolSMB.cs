using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script summary
/// </summary>
public class MinionPatrolSMB : SceneLinkedSMB<Minion>
{
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        switch (m_MonoBehaviour.state)
        {
            case MinionState.Patrolling:

                m_MonoBehaviour.Patrol();
                break;

            case MinionState.ClosingIn:

                m_MonoBehaviour.FindClosest();
                m_MonoBehaviour.MoveToClosest();
                m_MonoBehaviour.CheckAttackRange();
                break;

            case MinionState.Migrating:

                m_MonoBehaviour.Migrate();
                break;
        }
    }
}
