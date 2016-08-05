using UnityEngine;

public class Golem : BossManager {

	bool isAttacking;

	public override void Start () {
		base.Start();
		isAttacking = false;
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
			leftArm.SetBool("Attack1", true);
		}
		if (attack == 1)
		{
			rightArm.SetBool("Attack1", true);
		}
		if (attack == 2)
		{
			leftArm.SetBool("Attack2", true);
		}
		if (attack == 3)
		{
			rightArm.SetBool("Attack2", true);
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
			leftArm.SetBool("Attack1", true);
		}
		if (attack == 1)
		{
			rightArm.SetBool("Attack1", true);
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
			leftArm.SetBool("Attack1", true);
		}
		if (attack == 1)
		{
			rightArm.SetBool("Attack1", true);
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

	public override void Update ()
	{
		if(leftArm.GetBool("Attack1") == true || rightArm.GetBool("Attack1") == true 
			|| leftArm.GetBool("Attack2") == true || rightArm.GetBool("Attack2") == true
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
			isAttacking = true;
		}
		else
		{
			isAttacking = false;
		}
		base.Update();
	}
}
