using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMinion : GroundMinion {

    #region Protected variables

    protected readonly int m_HashAttackLeft = Animator.StringToHash("AttackLeft");
    protected readonly int m_HashAttackRight = Animator.StringToHash("AttackRight");

    #endregion

    protected override void StartAttack()
    {
        state = MinionState.Attacking;

        prevDir = moveDir;

        if (moveDir.x > 1)
        {
            m_Animator.SetTrigger(m_HashAttackLeft);
        }
        else
        {
            m_Animator.SetTrigger(m_HashAttackRight);
        }
    }

    protected override void StartAttack(AttackType attackType)
    {
        state = MinionState.Attacking;

        prevDir = moveDir;

        switch (attackType)
        {
            case AttackType.Melee:

                if (moveDir.x > 1)
                {
                    m_Animator.SetTrigger(m_HashAttackLeft);
                }
                else
                {
                    m_Animator.SetTrigger(m_HashAttackRight);
                }
                break;

            case AttackType.Ranged:

                //TODO: add ranged logic for Rock minion 'yeeting' rocks
                break;
        }
    }
}
