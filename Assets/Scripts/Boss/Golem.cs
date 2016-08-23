using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// Script to control the Golem Boss
/// </summary>
public class Golem : BossManager {

	public Animator lArmRock1, lArmRock2, lArmRock3,
	rArmRock1, rArmRock2, rArmRock3, headAnim, leftArm, rightArm,
	leftCrystal, rightCrystal;

	public List<Sprite> headStages, bodyStages, lArmStages, rArmStages,
						utilityA, utilityB, utilityC;

    /// <summary>
    /// Checks whether the the Boss is attacking
    /// </summary>
    public bool isAttacking
    {
        get
        {
            if (leftArm.GetBool("S1Attack1") == true || rightArm.GetBool("S1Attack1") == true
                || leftArm.GetBool("S1Attack2") == true || rightArm.GetBool("S1Attack2") == true
                || leftArm.GetBool("S2Attack1") == true || rightArm.GetBool("S2Attack1") == true
                || leftArm.GetBool("S2Attack2") == true || rightArm.GetBool("S2Attack2") == true
                || leftArm.GetBool("S3Attack1") == true || rightArm.GetBool("S3Attack1") == true
                || leftArm.GetBool("S3Attack2") == true || rightArm.GetBool("S3Attack2") == true
                || leftArm.GetBool("S4Attack1") == true || rightArm.GetBool("S4Attack1") == true
                || leftArm.GetBool("S4Special") == true || rightArm.GetBool("S4Special") == true
                || leftArm.GetBool("S4Stun") == true || rightArm.GetBool("S4Stun") == true
                || leftArm.GetBool("S5Attack1") == true || rightArm.GetBool("S5Attack1") == true
                || leftArm.GetBool("S5Attack2") == true || rightArm.GetBool("S5Attack2") == true
                || leftArm.GetBool("S5SpecialStun") == true || rightArm.GetBool("S5SpecialStun") == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public override void Start ()
    {
		base.Start();
	}

    #region Stage One
    public override void StageOne ()
	{
		//Sets the amount of attacks possible this stage
		attackCountStage = 4;
		// checks to make sure an attack is possible
		if(!isAttacking)
		{
			// Checks to make sure the list hasn't been run and that there is a list
			if (attackList.Count > currentCount && playList)
			{
				StageOneListings();
			}
			else
			{
				//Prevents access to the list once at this part
				playList = false;
				// Rolls a random number based on no of attacks
				int newAttack = Random.Range(0, attackCountStage);
				// Adds attack to list
				attackList.Add(newAttack);
				currentCount = attackList.Count;
				// Runs the attack assosiated with that number
				StageOneAttack(newAttack);
			}
		}
	}//Attack system for the first stadge of the boss fight

	void StageOneListings()
	{
		//pulls up the next attack in sequence
		for(int i = currentCount; i <= attackList.Count; i++)
		{
			if(isAttacking)
			{
				// makes sure a attack isn't already playing befor continuing
				return;
			}
			int attack = attackList[i];
			StageOneAttack(attack);
			// remembers the place in the list if exited out by above
			currentCount++;
		}
	}// Handles the List reading and exit upon exsisting attack

	void StageOneAttack(int attack)
	{
		if (attack == 0)
		{
			leftArm.SetBool("S1Attack1", true);
		}
		if (attack == 1)
		{
			rightArm.SetBool("S1Attack1", true);
		}
		if (attack == 2)
		{
			leftArm.SetBool("S1Attack2", true);
		}
		if (attack == 3)
		{
			rightArm.SetBool("S1Attack2", true);
		}
	}// Selects the attack based on the given number
	#endregion

	#region Stage Two
	public override void StageTwo ()
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
		// checks to make sure an attack is possible
		if(!isAttacking)
		{
			// Checks to make sure the list hasn't been run and that there is a list
			if (attackList2.Count > currentCount2 && playList2)
			{
				StageTwoListings();
			}
			else
			{
				//Prevents access to the list once at this part
				playList2 = false;
				// Rolls a random number based on no of attacks
				int newAttack = Random.Range(0, attackCountStage);
				// Adds attack to list
				attackList2.Add(newAttack);
				currentCount2 = attackList2.Count;
				// Runs the attack assosiated with that number
				StageTwoAttack(newAttack);
			}
		}
	}

	void StageTwoListings()
	{
		//pulls up the next attack in sequence
		for(int i = currentCount2; i <= attackList2.Count; i++)
		{
			if(isAttacking)
			{
				// makes sure a attack isn't already playing befor continuing
				return;
			}
			int attack = attackList[i];
			StageTwoAttack(attack);
			// remembers the place in the list if exited out by above
			currentCount2++;
		}
	}// Handles the List reading and exit upon exsisting attack

	void StageTwoAttack(int attack)
	{
		if (attack == 0)
		{
			leftArm.SetBool("S1Attack1", true);
		}
		if (attack == 1)
		{
			rightArm.SetBool("S1Attack1", true);
		}
		if (attack == 2)
		{
			leftArm.SetBool("S2Attack1", true);
		}
		if (attack == 3)
		{
			rightArm.SetBool("S2Attack1", true);
		}
		if (attack == 4)
		{
			rightArm.SetBool("S2Attack2", true);
			leftArm.SetBool("S2Attack2", true);
		}

	}// Selects the attack based on the given number
	#endregion

	#region Stage Three
	public override void StageThree ()
	{
		//Sets the amount of attacks possible this stage
		attackCountStage = 4;
		// checks to make sure an attack is possible
		if(!isAttacking)
		{
			// Checks to make sure the list hasn't been run and that there is a list
			if (attackList3.Count > currentCount3 && playList3)
			{
				StageThreeListings();
			}
			else
			{
				//Prevents access to the list once at this part
				playList3 = false;
				// Rolls a random number based on no of attacks
				int newAttack = Random.Range(0, attackCountStage);
				// Adds attack to list
				attackList3.Add(newAttack);
				currentCount3 = attackList3.Count;
				// Runs the attack assosiated with that number
				StageThreeAttack(newAttack);
			}
		}
	}

	void StageThreeListings()
	{
		//pulls up the next attack in sequence
		for(int i = currentCount3; i <= attackList3.Count; i++)
		{
			if(isAttacking)
			{
				// makes sure a attack isn't already playing befor continuing
				return;
			}
			int attack = attackList[i];
			StageThreeAttack(attack);
			// remembers the place in the list if exited out by above
			currentCount3++;
		}
	}// Handles the List reading and exit upon exsisting attack

	void StageThreeAttack(int attack)
	{
		if (attack == 0)
		{
			leftArm.SetBool("S3Attack1", true);
		}
		if (attack == 1)
		{
			rightArm.SetBool("S3Attack1", true);
		}
		if (attack == 2)
		{
			leftArm.SetBool("S3Attack2", true);
		}
		if (attack == 3)
		{
			rightArm.SetBool("S3Attack2", true);
		}
	}// Selects the attack based on the given number
	#endregion

	#region Stage Four
	public override void StageFour ()
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
		// checks to make sure an attack is possible
		if(!isAttacking)
		{
			// Checks to make sure the list hasn't been run and that there is a list
			if (attackList4.Count > currentCount4 && playList4)
			{
				StageFourListings();
			}
			else
			{
				//Prevents access to the list once at this part
				playList4 = false;
				// Rolls a random number based on no of attacks
				int newAttack = Random.Range(0, attackCountStage);
				// Adds attack to list
				attackList4.Add(newAttack);
				currentCount4 = attackList4.Count;
				// Runs the attack assosiated with that number
				StageFourAttack(newAttack);
			}
		}
	}

	void StageFourListings()
	{
		//pulls up the next attack in sequence
		for(int i = currentCount4; i <= attackList4.Count; i++)
		{
			if(isAttacking)
			{
				// makes sure a attack isn't already playing befor continuing
				return;
			}
			int attack = attackList[i];
			StageFourAttack(attack);
			// remembers the place in the list if exited out by above
			currentCount4++;
		}
	}// Handles the List reading and exit upon exsisting attack

	void StageFourAttack(int attack)
	{
		if (attack == 0)
		{
			leftArm.SetBool("S1Attack1", true);
		}
		if (attack == 1)
		{
			rightArm.SetBool("S1Attack1", true);
		}
		if (attack == 2)
		{
			leftArm.SetBool("S4Attack1", true);
		}
		if (attack == 3)
		{
			rightArm.SetBool("S4Attack1", true);
		}
		if (attack == 4)
		{
			leftArm.SetBool("S4Special", true);
			rightArm.SetBool("S4Special", true);
		}
		if (attack == 5)
		{
			rightArm.SetBool("S4Stun", true);
			leftArm.SetBool("S4Stun", true);
		}
	}// Selects the attack based on the given number
	#endregion

	#region Stage Five
	public override void StageFive ()
	{
		//Sets the amount of attacks possible this stage
		attackCountStage = 4;
		// checks to make sure an attack is possible
		if(!isAttacking)
		{
			// Checks to make sure the list hasn't been run and that there is a list
			if (attackList5.Count > currentCount5 && playList5)
			{
				StageFiveListings();
			}
			else
			{
				//Prevents access to the list once at this part
				playList5 = false;
				// Rolls a random number based on no of attacks
				int newAttack = Random.Range(0, attackCountStage);
				// Adds attack to list
				attackList5.Add(newAttack);
				// Runs the attack assosiated with that number
				StageFiveAttack(newAttack);
			}
		}
	}

	void StageFiveListings()
	{
		//pulls up the next attack in sequence
		for(int i = currentCount5; i <= attackList5.Count; i++)
		{
			if(isAttacking)
			{
				// makes sure a attack isn't already playing befor continuing
				return;
			}
			int attack = attackList[i];
			StageFiveAttack(attack);
			// remembers the place in the list if exited out by above
			currentCount5++;
		}
	}// Handles the List reading and exit upon exsisting attack

	void StageFiveAttack(int attack)
	{
		if (attack == 0)
		{
			leftArm.SetBool("S5Attack1", true);
		}
		if (attack == 1)
		{
			rightArm.SetBool("S5Attack1", true);
		}
		if (attack == 2)
		{
			leftArm.SetBool("S5Attack2", true);
		}
		if (attack == 3)
		{
			rightArm.SetBool("S5Attack2", true);
		}
		if (attack == 4)
		{
			rightArm.SetBool("S5SpecialStun", true);
			leftArm.SetBool("S5SpecialStun", true);
		}
	}// Selects the attack based on the given number
	#endregion

	public override void SetBossParts ()
	{
		
	}

	//Sync support attacks with sequenced attacks for additional effects
	void SupportAttacks()
	{
		if(leftArm.GetCurrentAnimatorStateInfo(0).IsName("LeftSlamRightRock") &&
			leftArm.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
		{
			lArmRock1.SetBool("Fall", true);
			lArmRock2.SetBool("Fall", true);
			lArmRock3.SetBool("Fall", true);
		}

		if(rightArm.GetCurrentAnimatorStateInfo(0).IsName("RightSlamLeftRock") &&
			rightArm.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
		{
			rArmRock1.SetBool("Fall", true);
			rArmRock2.SetBool("Fall", true);
			rArmRock3.SetBool("Fall", true);
		}

		if(leftArm.GetCurrentAnimatorStateInfo(0).IsName("LeftSlamCrystal") ||
			leftArm.GetCurrentAnimatorStateInfo(0).IsName("LeftSpecial"))
		{
			if(leftArm.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.3f &&
				leftArm.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.4f)
			{
				rightCrystal.SetBool("Rise", true);
			}
		}

		if(rightArm.GetCurrentAnimatorStateInfo(0).IsName("RightSlamCrystal") ||
			rightArm.GetCurrentAnimatorStateInfo(0).IsName("RightSpecial"))
		{
			if(rightArm.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.3f &&
				rightArm.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.4f)
			{
				leftCrystal.SetBool("Rise", true);
			}
		}
	}

	public override void Update ()
	{
		SupportAttacks();
        base.Update();
	}

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
