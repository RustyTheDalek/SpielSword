using UnityEngine;


public class Golem : BossManager {

	//public Animator leftArm, rightArm;

	Timer LAttack1, RAttack1,
	LAttack2, RAttack2;

	bool playList;

	int totalSweeps = 3, currentSweeps = 0,
	totalSlams = 4, currentSlams = 0;

	//public List<Sprite> headStages, chestStages, lArmStages, rArmStages, c2, c3, c4;

	void Start () {

		playList = true;

		attackManager = GameObject.Find ("Attack Manager");
		attackList = attackManager.GetComponent<AttackStorage> ().stageOneAttacks;

		LAttack1 = gameObject.AddComponent<Timer>();
		LAttack1.Setup("LeftHandAttack", 2, true);
		
		LAttack1.StartTimer();
		
		RAttack1 = gameObject.AddComponent<Timer>();
		RAttack1.Setup("RightHandAttack", 2.5f, true);
		//RAttack1.enabled = false;
		
		LAttack2 = gameObject.AddComponent<Timer>();
		LAttack2.Setup("LHandSlam", 1f, true);
		
		RAttack2 = gameObject.AddComponent<Timer>();
		RAttack2.Setup("RHandSlam", 1f, true);
	}

	public override void StageOne ()
	{
		attackCountStage = 4;
		// prevent steping in
		if(leftArm.GetBool("Attack1") == false && rightArm.GetBool("Attack1") == false &&
			leftArm.GetBool("Attack2") == false && rightArm.GetBool("Attack2") == false)
		{
			if (attackList.Count > 0 && playList)
			{
				
				foreach (int attack in attackList)
				{
					
					StageOneAttack(attack);
					// Cooldown
				}
				playList = false;
			}
			//exit = true;
			int newAttack = Random.Range(0, attackCountStage);
			StageOneAttack(newAttack);
			// Cooldown
			attackList.Add(newAttack);
		}
	}

	void StageOneAttack(int attack)
	{
		if (attack == 0)
		{
			//LAttack1.Reset();
			leftArm.SetBool("Attack1", true);

			//LAttack1.enabled = false;

			//RAttack1.enabled = true;
			//RAttack1.StartTimer();
		}
		if (attack == 1)
		{
			rightArm.SetBool("Attack1", true);
			//RAttack1.Reset();

			//RAttack1.enabled = false;

			//LAttack1.enabled = true;
			//LAttack1.StartTimer();
		}
		if (attack == 2)
		{
			//LAttack2.Reset();
			leftArm.SetBool("Attack2", true);

			//LAttack2.enabled = false;

			//RAttack2.enabled = true;
			//RAttack2.StartTimer();
		}
		if (attack == 3)
		{
			rightArm.SetBool("Attack2", true);
			//RAttack2.Reset();

			//RAttack2.enabled = false;

			//LAttack2.enabled = true;
			//LAttack2.StartTimer();
		}
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
	}
	public override void StageThree ()
	{

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
	}
	public override void StageFive ()
	{
		
	}

	public override void SetBossParts ()
	{
		
	}

	public override void Update ()
	{
		base.Update();
		/*
		if (currentSweeps <= totalSweeps)
		{
			if (LAttack1.complete)
			{
				currentSweeps++;
				LAttack1.Reset();
				leftArm.SetBool("Attack1", true);
				
				LAttack1.enabled = false;
				
				RAttack1.enabled = true;
				RAttack1.StartTimer();
			}
			
			if (RAttack1.complete)
			{
				currentSweeps++;
				rightArm.SetBool("Attack1", true);
				RAttack1.Reset();
				RAttack1.enabled = false;
				
				LAttack1.enabled = true;
				LAttack1.StartTimer();
			}
		}
		else if (currentSlams <= totalSlams)
		{
			if (currentSlams == 0 && !LAttack2.active && !LAttack2.complete)
			{
				LAttack2.StartTimer();
			}
			if (LAttack2.complete)
			{
				currentSlams++;
				LAttack2.Reset();
				leftArm.SetBool("Attack2", true);
				
				LAttack2.enabled = false;
				
				RAttack2.enabled = true;
				RAttack2.StartTimer();
			}
			
			if (RAttack2.complete)
			{
				currentSlams++;
				rightArm.SetBool("Attack2", true);
				RAttack2.Reset();
				RAttack2.enabled = false;
				
				LAttack2.enabled = true;
				LAttack2.StartTimer();
			}
		}
		else
		{
			currentSweeps = 0;
			currentSlams = 0;
		}
		*/
	}
}
