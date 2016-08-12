using UnityEngine;
using System.Collections;

public class GolemBoss : BossManager {
	
	bool isAttacking;

	public override void Start () {
		base.Start();
		isAttacking = true;
	}

	public override void StageOne ()
	{
		//Sets the amount of attacks possible this stage
		attackCountStage = 4;
		Attacking(attackCountStage, 1);
	}

	public override void StageTwo ()
	{
		bossParts[0].sprite = headStages[0];
		bossParts[1].sprite = bodyStages[0];
		bossParts[2].sprite = utilityA[0];      
		bossParts[3].sprite = utilityB[0];
		bossParts[4].sprite = utilityC[0];
		bossParts[5].sprite = rArmStages[0];
		bossParts[6].sprite = lArmStages[0];

		attackCountStage = 5;
		Attacking(attackCountStage, 2);
	}

	public override void StageThree ()
	{
		attackCountStage = 4;
		Attacking(attackCountStage, 3);
	}

	public override void StageFour ()
	{
		bossParts[0].sprite = headStages[1];
		bossParts[1].sprite = bodyStages[1];
		bossParts[2].sprite = utilityA[1];
		bossParts[3].sprite = utilityB[1];
		bossParts[4].sprite = utilityC[1];
		bossParts[5].sprite = rArmStages[1];
		bossParts[6].sprite = lArmStages[1];

		attackCountStage = 6;
		Attacking(attackCountStage, 4);
	}

	public override void StageFive ()
	{
		attackCountStage = 4;
		Attacking(attackCountStage, 5);
	}
				
	void Attacking(int attackCountStage, int stageNo)
	{
		if(!isAttacking)
		{
			// Rolls a random number based on no of attacks
			int newAttack = Random.Range(0, attackCountStage);
			switch (stageNo)
			{
			case 1:
				// Checks to make sure the list hasn't been run and that there is a list
				if (attackList.Count > currentCount && playList)
				{
					CycleAttackHistory(stageNo);
				}
				else
				{
					//Prevents access to the list once at this part
					playList = false;
					// Adds attack to list
					attackList.Add(newAttack);
					// Runs the attack assosiated with that number
					AttackSelect(newAttack,"Attack1","Attack1","Attack2","Attack2","Nothing","Nothing");
				}
				break;
			case 2:
				if (attackList2.Count > currentCount2 && playList2)
				{
					CycleAttackHistory(stageNo);
				}
				else
				{
					playList2 = false;
					attackList2.Add(newAttack);
					AttackSelect(newAttack,"Attack1","Attack1","S2Attack1","S2Attack1","S2Attack2","S2Attack2");
				}
				break;
			case 3:
				if (attackList3.Count > currentCount3 && playList3)
				{
					CycleAttackHistory(stageNo);
				}
				else
				{
					playList3 = false;
					attackList3.Add(newAttack);
					AttackSelect(newAttack,"S3Attack1","S3Attack1","S3Attack2","S3Attack2","Nothing","Nothing");
				}
				break;
			case 4:
				if (attackList4.Count > currentCount4 && playList4)
				{
					CycleAttackHistory(stageNo);
				}
				else
				{
					playList4 = false;
					attackList4.Add(newAttack);
					AttackSelect(newAttack,"Attack1","Attack1","S4Attack1","S4Attack1","S4Special","S4Stun");
				}
				break;
			case 5:
				if (attackList5.Count > currentCount5 && playList5)
				{
					CycleAttackHistory(stageNo);
				}
				else
				{
					playList5 = false;
					attackList5.Add(newAttack);
					AttackSelect(newAttack,"S5Attack1","S5Attack1","S5Attack2","S5Attack2","SpecialStun","Nothing");
				}
				break;
			}

		}
	}

	void AttackSelect(int attack, string name, string name2, string name3,
		string name4, string name5, string name6)
	{
		if (attack == 0)
		{
			leftArm.SetBool(name, true);
		}
		if (attack == 1)
		{
			rightArm.SetBool(name2, true);
		}
		if (attack == 2)
		{
			leftArm.SetBool(name3, true);
		}
		if (attack == 3)
		{
			rightArm.SetBool(name4, true);
		}
		if (attack == 4)
		{
			rightArm.SetBool(name5, true);
		}
		if (attack == 5)
		{
			rightArm.SetBool(name6, true);
		}
	}

	void CycleAttackHistory(int stageNo)
	{
		switch(stageNo)
		{
		case 1:
			//pulls up the next attack in sequence
			for(int i = currentCount; i <= attackList.Count; i++)
			{
				if(isAttacking)
				{
					// makes sure a attack isn't already playing befor continuing
					return;
				}
				int attack = attackList[i];
				AttackSelect(attack,"Attack1","Attack1","Attack2","Attack2","Nothing","Nothing");
				// remembers the place in the list if exited out by above
				currentCount++;
				CheckIfAttacking();
			}
			break;
		case 2:
			for(int i = currentCount2; i <= attackList2.Count; i++)
			{
				if(isAttacking)
				{
					return;
				}
				int attack = attackList[i];
				AttackSelect(attack,"Attack1","Attack1","S2Attack1","S2Attack1","S2Attack2","S2Attack2");
				currentCount2++;
				CheckIfAttacking();
			}
			break;
		case 3:
			for(int i = currentCount3; i <= attackList3.Count; i++)
			{
				if(isAttacking)
				{
					return;
				}
				int attack = attackList[i];
				AttackSelect(attack,"S3Attack1","S3Attack1","S3Attack2","S3Attack2","Nothing","Nothing");
				currentCount3++;
				CheckIfAttacking();
			}
			break;
		case 4:
			for(int i = currentCount4; i <= attackList4.Count; i++)
			{
				if(isAttacking)
				{
					return;
				}
				int attack = attackList[i];
				AttackSelect(attack,"Attack1","Attack1","S4Attack1","S4Attack1","S4Special","S4Stun");
				currentCount4++;
				CheckIfAttacking();
			}
			break;
		case 5:
			for(int i = currentCount5; i <= attackList5.Count; i++)
			{
				if(isAttacking)
				{
					return;
				}
				int attack = attackList[i];
				AttackSelect(attack,"S5Attack1","S5Attack1","S5Attack2","S5Attack2","SpecialStun","Nothing");
				currentCount5++;
				CheckIfAttacking();
			}
			break;
		}
	}

	void CheckIfAttacking()
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
	}

	public override void SetBossParts (){}
	// Update is called once per frame
	public override void Update ()
	{
		CheckIfAttacking();
		base.Update();
	}
}
