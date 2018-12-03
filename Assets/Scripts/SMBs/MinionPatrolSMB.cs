using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script summary
/// </summary>
public class MinionPatrolSMB : SceneLinkedSMB<Minion>
{
    public override void OnSLStateNoTransitionUpdate(Animator animator, 
        AnimatorStateInfo stateInfo, int layerIndex)
    {
        switch (m_MonoBehaviour.state)
        {
            case MinionState.Patrolling:

                m_MonoBehaviour.Patrol();
                break;

            case MinionState.ClosingIn:

                m_MonoBehaviour.FindClosest();
                switch(m_MonoBehaviour.attackType)
                {
                    case AttackType.Melee:
                        m_MonoBehaviour.MoveToClosest();
                        m_MonoBehaviour.CheckMeleeAttackRange();
                        break;

                    case AttackType.Ranged:
                        m_MonoBehaviour.MoveToEngage();
                        m_MonoBehaviour.CheckRangedAttackRange();

                        if (m_MonoBehaviour.meleePanic)
                        {
                            m_MonoBehaviour.CheckMeleeAttackRange();
                        }
                        break;
                }
                break;

            case MinionState.Migrating:

                m_MonoBehaviour.Migrate();
                break;
        }
    }
}
