using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormKing : BossManager{
    [HideInInspector]
    public List<Sprite> headStages, bodyStages, lArmStages, rArmStages,
                          utilityA, utilityB, utilityC;
    
    /// <summary>
    /// Causes next stage of Boss Damage
    /// TODO:Make this much more elegant
    /// </summary>
    protected override void DamageBoss(int num)
    {
        //bossLimbs[0].sprite = headStages[num];
        //bossLimbs[1].sprite = bodyStages[num];
        //bossLimbs[2].sprite = utilityA[num];
        //bossLimbs[3].sprite = utilityB[num];
        //bossLimbs[4].sprite = utilityC[num];
        //bossLimbs[5].sprite = rArmStages[num];
        //bossLimbs[6].sprite = lArmStages[num];
    }

    #region Stage One
    public override void OnStageOne()
    {
        attackCountStage = 6;
    }

    protected override void StageOneAttacks(int attack)
    {
        switch (attack)
        {
            case 0:

                //animator.SetTrigger("S1A1L");
                break;

            case 1:

                //animator.SetTrigger("S1A1R");
                break;

            case 2:

                //animator.SetTrigger("S1A2L");
                break;

            case 3:

                //animator.SetTrigger("S1A2R");
                break;

            case 4:

                //animator.SetTrigger("S1A3L");
                break;

            case 5:

               // animator.SetTrigger("S1A3R");
                break;
        }

        attacking = true;
    }// Selects the attack based on the given number
    #endregion
    #region Stage Two
    public override void OnStageTwo()
    {
        base.OnStageTwo();
        //Sets the amount of attacks possible this stage
        attackCountStage = 5;
    }

    protected override void StageTwoAttacks(int attack)
    {
        switch (attack)
        {
            case 0:

                //animator.SetTrigger("S2A1L");
                break;

            case 1:

                //animator.SetTrigger("S2A1R");
                break;

            case 2:

                //animator.SetTrigger("S2A2L");
                break;

            case 3:

                //animator.SetTrigger("S2A2R");
                break;

            case 4:

                //animator.SetTrigger("S2A3");
                break;
        }

        attacking = true;
    }// Selects the attack based on the given number
    #endregion
    #region Stage Three
    public override void OnStageThree()
    {
        base.OnStageThree();
        //Sets the amount of attacks possible this stage
        attackCountStage = 4;
    }

    protected override void StageThreeAttacks(int attack)
    {
        switch (attack)
        {
            case 0:

                //animator.SetTrigger("S3A1L");
                break;

            case 1:

                //animator.SetTrigger("S3A1R");
                break;

            case 2:

                //animator.SetTrigger("S3A2L");
                break;

            case 3:

                //animator.SetTrigger("S3A2R");
                break;
        }

        attacking = true;
    }// Selects the attack based on the given number
    #endregion
    #region Stage Four
    public override void OnStageFour()
    {
        base.OnStageFour();
        //Sets the amount of attacks possible this stage
        attackCountStage = 5;
    }

    protected override void StageFourAttacks(int attack)
    {
        switch (attack)
        {
            case 0:

                //animator.SetTrigger("S1A1L");
                break;

            case 1:

                //animator.SetTrigger("S1A1R");
                break;

            case 2:

                //animator.SetTrigger("S4A1L");
                break;

            case 3:

                //animator.SetTrigger("S4A1R");
                break;

            case 4:

                //animator.SetTrigger("S4A2L");
                break;
        }

        attacking = true;
    }// Selects the attack based on the given number
    #endregion
    #region Stage Five
    public override void OnStageFive()
    {
        base.OnStageFive();
        //Sets the amount of attacks possible this stage
        attackCountStage = 6;
    }

    protected override void StageFiveAttacks(int attack)
    {
        switch (attack)
        {
            case 0:

                //animator.SetTrigger("S5A1L");
                break;

            case 1:

               // animator.SetTrigger("S5A1R");
                break;

            case 2:

               // animator.SetTrigger("S5A2L");
                break;

            case 3:

               // animator.SetTrigger("S5A2R");
                break;

            case 4:
               // animator.SetTrigger("S5A3");
                break;

            case 5:
                break;

        }

        attacking = true;
    }// Selects the attack based on the given number
    #endregion
}
