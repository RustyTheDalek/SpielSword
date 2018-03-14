using UnityEngine;
using System.Collections.Generic;
using System;
/// <summary>
/// Script to control the Golem Boss
/// </summary>
public class Golem : BossManager {

    [HideInInspector]
	public Animator lArmRock1, lArmRock2, lArmRock3, lArmRock4,
	rArmRock1, rArmRock2, rArmRock3, headAnim, leftArm, rightArm,
	leftCrystal, rightCrystal;

    [HideInInspector]
	public GameObject rockPileLeft, rockPileRight, lArm, rArm;

    [HideInInspector]
	public List<Sprite> headStages, bodyStages, lArmStages, rArmStages,
						utilityA, utilityB, utilityC;

    /// <summary>
    /// Checks whether the the Boss is attacking
    /// </summary>
    public override bool Attacking
    {
        get
        {
            if (leftArm.GetComponent<BossAttack>().attacking ||
                rightArm.GetComponent<BossAttack>().attacking ||
                lArmRock1.GetComponent<BossAttack>().attacking ||
                lArmRock2.GetComponent<BossAttack>().attacking ||
                lArmRock3.GetComponent<BossAttack>().attacking ||
                //lArmRock4.GetComponent<BossAttack>().attacking ||
                rArmRock2.GetComponent<BossAttack>().attacking ||
                rArmRock3.GetComponent<BossAttack>().attacking ||
                rightArm.GetComponent<BossAttack>().attacking ||
                leftCrystal.GetComponent<BossAttack>().attacking ||
                rightCrystal.GetComponent<BossAttack>().attacking )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    #region Stage One
    public override void OnStageOne()
    {
        attackCountStage = 6;
    }

	protected override void StageOneAttacks(int attack)
	{
		if (attack == 0)
		{
			leftArm.SetBool("S1Attack1", true);
            leftArm.GetComponent<BossAttack>().attacking = true;
		}
		if (attack == 1)
		{
			rightArm.SetBool("S1Attack1", true);
            rightArm.GetComponent<BossAttack>().attacking = true;
        }
        if (attack == 2)
		{
			leftArm.SetBool("S1Attack2", true);
            leftArm.GetComponent<BossAttack>().attacking = true;
        }
        if (attack == 3)
		{
			rightArm.SetBool("S1Attack2", true);
            rightArm.GetComponent<BossAttack>().attacking = true;
        }
        if (attack == 4)
		{
			leftArm.SetBool("S1Attack3", true);
            leftArm.GetComponent<BossAttack>().attacking = true;
        }
        if (attack == 5)
		{
			rightArm.SetBool("S1Attack3", true);
            rightArm.GetComponent<BossAttack>().attacking = true;
        }
    }// Selects the attack based on the given number
    #endregion

    #region Stage Two
    public override void OnStageTwo()
    {
        bossParts[0].sprite = headStages[0];
        bossParts[1].sprite = bodyStages[0];
        bossParts[2].sprite = utilityA[0];
        bossParts[3].sprite = utilityB[0];
        bossParts[4].sprite = utilityC[0];
        bossParts[5].sprite = rArmStages[0];
        bossParts[6].sprite = lArmStages[0];

        //Sets the amount of attacks possible this stage
        attackCountStage = 5;
    }

    protected override void StageTwoAttacks(int attack)
	{
		if (attack == 0)
		{
			leftArm.SetBool("S1Attack1", true);
            leftArm.GetComponent<BossAttack>().attacking = true;
        }
        if (attack == 1)
		{
			rightArm.SetBool("S1Attack1", true);
            rightArm.GetComponent<BossAttack>().attacking = true;
        }
        if (attack == 2)
		{
			leftArm.SetBool("S2Attack1", true);
            leftArm.GetComponent<BossAttack>().attacking = true;
        }
        if (attack == 3)
		{
			rightArm.SetBool("S2Attack1", true);
            rightArm.GetComponent<BossAttack>().attacking = true;
        }
        if (attack == 4)
		{
			rightArm.SetBool("S2Attack2", true);
            rightArm.GetComponent<BossAttack>().attacking = true;

            leftArm.SetBool("S2Attack2", true);
            leftArm.GetComponent<BossAttack>().attacking = true;
        }

    }// Selects the attack based on the given number
    #endregion

    #region Stage Three
    public override void OnStageThree()
    {
        //Sets the amount of attacks possible this stage
        attackCountStage = 4;
    }

    protected override void StageThreeAttacks(int attack)
	{
		if (attack == 0)
		{
			leftArm.SetBool("S3Attack1", true);
            leftArm.GetComponent<BossAttack>().attacking = true;
        }
        if (attack == 1)
		{
			rightArm.SetBool("S3Attack1", true);
            rightArm.GetComponent<BossAttack>().attacking = true;
        }
        if (attack == 2)
		{
			leftArm.SetBool("S3Attack2", true);
            leftArm.GetComponent<BossAttack>().attacking = true;
        }
        if (attack == 3)
		{
			rightArm.SetBool("S3Attack2", true);
            rightArm.GetComponent<BossAttack>().attacking = true;
        }
    }// Selects the attack based on the given number
    #endregion

    #region Stage Four
    public override void OnStageFour()
    {
        bossParts[0].sprite = headStages[1];
        bossParts[1].sprite = bodyStages[1];
        bossParts[2].sprite = utilityA[1];
        bossParts[3].sprite = utilityB[1];
        bossParts[4].sprite = utilityC[1];
        bossParts[5].sprite = rArmStages[1];
        bossParts[6].sprite = lArmStages[1];

        //Sets the amount of attacks possible this stage
        attackCountStage = 6;
    }

    protected override void StageFourAttacks(int attack)
	{
		if (attack == 0)
		{
			leftArm.SetBool("S1Attack1", true);
            leftArm.GetComponent<BossAttack>().attacking = true;
        }
        if (attack == 1)
		{
			rightArm.SetBool("S1Attack1", true);
            rightArm.GetComponent<BossAttack>().attacking = true;
        }
        if (attack == 2)
		{
			leftArm.SetBool("S4Attack1", true);
            leftArm.GetComponent<BossAttack>().attacking = true;
        }
        if (attack == 3)
		{
			rightArm.SetBool("S4Attack1", true);
            rightArm.GetComponent<BossAttack>().attacking = true;
        }
        if (attack == 4)
		{
			leftArm.SetBool("S4Special", true);
            leftArm.GetComponent<BossAttack>().attacking = true;

            rightArm.SetBool("S4Special", true);
            rightArm.GetComponent<BossAttack>().attacking = true;
        }
        if (attack == 5)
		{
			rightArm.SetBool("S4Stun", true);
            rightArm.GetComponent<BossAttack>().attacking = true;

            leftArm.SetBool("S4Stun", true);
            leftArm.GetComponent<BossAttack>().attacking = true;

        }
    }// Selects the attack based on the given number
    #endregion

    #region Stage Five
    public override void OnStageFive()
    {
        //Sets the amount of attacks possible this stage
        attackCountStage = 4;
    }

    protected override void StageFiveAttacks(int attack)
	{
		if (attack == 0)
		{
			leftArm.SetBool("S5Attack1", true);
            leftArm.GetComponent<BossAttack>().attacking = true;
        }
        if (attack == 1)
		{
			rightArm.SetBool("S5Attack1", true);
            rightArm.GetComponent<BossAttack>().attacking = true;
        }
        if (attack == 2)
		{
			leftArm.SetBool("S5Attack2", true);
            leftArm.GetComponent<BossAttack>().attacking = true;
        }
        if (attack == 3)
		{
			rightArm.SetBool("S5Attack2", true);
            rightArm.GetComponent<BossAttack>().attacking = true;
        }
		if (attack == 4)
		{
			rightArm.SetBool("S5SpecialStun", true);
            rightArm.GetComponent<BossAttack>().attacking = true;

			leftArm.SetBool("S5SpecialStun", true);
            leftArm.GetComponent<BossAttack>().attacking = true;
        }
    }// Selects the attack based on the given number
	#endregion

    public override void Reset()
    {
        base.Reset();

		headAnim.Play("WakeUp", 0);
		leftArm.Play("WakeUp", 0);
		rightArm.Play("WakeUp", 0);

        for (int stage = 1; stage <= 5; stage++)
        {
            for (int attack = 1; attack <= 2; attack++)
            {
                leftArm.SetBool("S" + stage + "Attack" + attack, false);
                rightArm.SetBool("S" +stage + "Attack" + attack, false);
            }

            if (stage == 4)
            {
                leftArm.SetBool("S" + stage + "Special", false);
                rightArm.SetBool("S" + stage + "Special", false);

                leftArm.SetBool("S" + stage + "Stun", false);
                rightArm.SetBool("S" + stage + "Stun", false);

            }

            if (stage == 5)
            {
                leftArm.SetBool("S" + stage+ "SpecialStun", false);
                rightArm.SetBool("S" + stage + "SpecialStun", false);

            }
        }
    }
}
