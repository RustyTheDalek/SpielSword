using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;

public abstract class BossManager : MonoBehaviour {

    public int stage = 0;

	public Head head;

    public static float health = 100;

    public List<int>    attackList,
                        attackList2,
                        attackList3,
                        attackList4,
                        attackList5 = new List<int>();

    public List<List<int>> stageAttacks = new List<List<int>>();

    public List<int> CurrentStageList
    {
        get
        {
            return stageAttacks[(int)bossStage];
        }
    }

	public int attackCountStage;
    // Values stored in these ints are used to track the progress in the attack list
    // number system determins the stage they are in charge of
    //Stores how many attacks are in each stage
    //public int currentCount, currentCount2, currentCount3, currentCount4, currentCount5;
    List<int> numberOAttacks = new List<int>(5);

    public int NumberOAttacks4Stage
    {
        get
        {
            return numberOAttacks[(int)bossStage];
        }

        set
        {
            numberOAttacks[(int)bossStage] = value;
        }
    }

    //public int currentCount, currentCount2, currentCount3, currentCount4, currentCount5;
	// Makes sure the list isn't run more then once per stage
	public bool playList, playList2, playList3, playList4, playList5;

    public AnimationCurve bossSpeedUp;

    public bool Alive
	{
		get
		{
			return health > 0;
		}
	}

	public List<SpriteRenderer> bossParts;

    public BossHealthBar bossHealthBar;

    protected List<int>     stageOneAttackSequence = new List<int>(),
                            stageTwoAttackSequence = new List<int>(),
                            stageThreeAttackSequence = new List<int>(),
                            stageFourAttackSequence = new List<int>(),
                            stageFiveAttackSequence = new List<int>();

    #region Variables for retracing the Boss' Actions
    //List of Boss parts that are tracked for rewind
    List<Animator> bossAnims = new List<Animator>();

    public List<float> trackedHealth = new List<float>();
    #endregion

    public BossStage bossStage = BossStage.One;

    /// <summary>
    /// Uses Normalised time from Boss animations to smooth the speed up for stage skipping
    /// </summary>
    float Smoothing
    {
        get
        {
            return Mathf.Repeat(bossAnims[0].GetCurrentAnimatorStateInfo(0).normalizedTime, 1);
        }
    }

    public abstract bool Attacking { get; }

    public virtual void Start ()
	{
        //Sets the counter for the list to zero
        for(int i = 0; i < numberOAttacks.Capacity; i++)
        {
            numberOAttacks.Add(0);
        }


        for (int i = 0; i < stageAttacks.Count; i++)
        {
            stageAttacks[i] = new List<int>();
        }
        // sets the bool to true so they run first time
        playList = true;
		playList2 = true;
		playList3 = true;
		playList4 = true;
		playList5 = true;

        //Get All objects that can be tracked
        //Currently assumes anything with an Animator needs to be tracked
        Animator[] objs = GetComponentsInChildren<Animator>();

        bossAnims.AddRange(objs);

        OnStageOne();
    }

    #region Stage skipping ideas

    //Idea 1 : Boss Fast forwards between stages when skipping attacks aren't
    public void FastforwardSkip()
    {
        //Here we're trying to get a percentage of how far into the Stage the boss is
        //We use the attack list as a rudimentry method then use the smoothing value 
        //to have it interoplate between the values
        float value = (float)((float)(numberOAttacks[(int)bossStage] + Smoothing) / 
            (float)stageAttacks[(int)bossStage].Count);
        //Debug.Log(value + " : " + Smoothing + " : " + bossSpeedUp.Evaluate(value));
        Game.PastTimeScale = bossSpeedUp.Evaluate(value);
    }

    //Idea 2 : Skip to next Stage
    public void NextStage()
    {
        bossStage += 1;
    }

    #endregion

    bool SkipStageCheck()
    {
        if (stageAttacks[(int)bossStage].Count != numberOAttacks[(int)bossStage])
        {
            Game.skippingStage = true;
        }
        else
        {
            Game.skippingStage = false;
        }

        SetColliders(Game.skippingStage);

        return Game.skippingStage;
    }

    void StageListings(Action<int> stageAttacks)
    {
        //pulls up the next attack in sequence
        for (int i = NumberOAttacks4Stage; i <= CurrentStageList.Count; i++)
        {
            if (Attacking)
            {
                // makes sure a attack isn't already playing befor continuing
                return;
            }
            int attack = CurrentStageList[i];
            stageAttacks(attack);
            // remembers the place in the list if exited out by above
            NumberOAttacks4Stage++;
        }
    }

    void StageAttackLogic(Action<int> stageAttacks)
    {
        // checks to make sure an attack is possible
        if (!Attacking)
        {
            // Checks to make sure the list hasn't been run and that there is a list
            if (attackList.Count > NumberOAttacks4Stage && playList)
            {
                StageListings(stageAttacks);
            }
            else
            {
                //Prevents access to the list once at this part
                playList = false;
                // Rolls a random number based on no of attacks
                int newAttack = UnityEngine.Random.Range(0, attackCountStage);
                // Adds attack to list
                attackList.Add(newAttack);
                NumberOAttacks4Stage = attackList.Count;
                // Runs the attack assosiated with that number
                stageAttacks(newAttack);
            }
        }
    }

    // Update is called once per frame
    public virtual void Update()
    {
        //attackList = stageAttacks[0];
        #region Debug options for testing
        if (Input.GetKeyDown(KeyCode.K))
        {
            Reset();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            health = 100;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            health = 70;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            health = 50;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            health = 30;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            health = 10;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            health = 0;
        }
        #endregion

        switch (Game.timeState)
        {
            case TimeState.Forward:

                switch (bossStage)
                {
                    case BossStage.One:

                        if (health > 80)
                        {
                            StageAttackLogic(StageOneAttacks);
                        }
                        else
                        {
                            //This check helps decide if the Boss has entered a stage early
                            //If The attackList doesn't match the current amount of 
                            //attacks then it was entered early
                            if (SkipStageCheck())
                            {
                                StageAttackLogic(StageOneAttacks);
                            }
                            else
                            {
                                bossStage = BossStage.Two;
                                OnStageTwo();
                            }
                        }

                        break;

                    case BossStage.Two:

                        if (health > 60)
                        {
                            StageAttackLogic(StageOneAttacks);
                        }
                        else
                        {
                            if (SkipStageCheck())
                            {
                                StageAttackLogic(StageTwoAttacks);
                            }
                            else
                            {
                                bossStage = BossStage.Three;
                                OnStageThree();
                            }
                        }

                        break;

                    case BossStage.Three:

                        if (health > 40)
                        {
                            StageAttackLogic(StageOneAttacks);
                        }
                        else
                        {
                            if (SkipStageCheck())
                            {
                                StageAttackLogic(StageThreeAttacks);
                            }
                            else
                            {
                                bossStage = BossStage.Four;
                                OnStageFour();
                            }
                        }
                        break;

                    case BossStage.Four:

                        if (health > 20)
                        {
                            StageAttackLogic(StageOneAttacks);
                        }
                        else
                        {
                            if (SkipStageCheck())
                            {
                                StageAttackLogic(StageFourAttacks);

                            }
                            else
                            {
                                bossStage = BossStage.Five;
                                OnStageFive();
                            }
                        }
                        break;

                    case BossStage.Five:

                        if (health > 0)
                        {
                            StageAttackLogic(StageFiveAttacks);
                        }
                        break;
                }
                //#region Stage select
                //if (health > 80)
                //{
                //    StageOne();
                //}
                //else if (health < 80 && health > 60)
                //{
                //    if (attackList.Count != currentCount)
                //    {
                //        StageOne();
                //        Game.skippingStage = true;
                //        SetColliders(Game.skippingStage);
                //    }
                //    else
                //    {
                //        //Game.PastTimeScale = 1;
                //        Game.skippingStage = false;
                //        SetColliders(Game.skippingStage);
                //        Game.PastTimeScale = 1;
                //        StageTwo();
                //    }
                //}
                //else if (health < 60 && health > 40)
                //{
                //    if (attackList2.Count != currentCount2)
                //    {
                //        StageTwo();
                //        Game.skippingStage = true;
                //        SetColliders(Game.skippingStage);
                //    }
                //    else
                //    {
                //        Game.skippingStage = false;
                //        SetColliders(Game.skippingStage);
                //        Game.PastTimeScale = 1;
                //        StageThree();
                //    }
                //}
                //else if (health < 40 && health > 20)
                //{
                //    if (attackList3.Count != currentCount3)
                //    {
                //        StageThree();
                //        Game.skippingStage = true;
                //        SetColliders(Game.skippingStage);
                //    }
                //    else
                //    {
                //        Game.skippingStage = false;
                //        SetColliders(Game.skippingStage);
                //        Game.PastTimeScale = 1;
                //        StageFour();
                //    }
                //}
                //else if (health > 0 && health < 20)
                //{
                //    if (attackList4.Count != currentCount4)
                //    {
                //        StageFour();
                //        Game.skippingStage = true;
                //        SetColliders(Game.skippingStage);
                //    }
                //    else
                //    {
                //        Game.skippingStage = false;
                //        SetColliders(Game.skippingStage);
                //        Game.PastTimeScale = 1;
                //        StageFive();
                //    }
                //}
                //#endregion

                if (Alive)
                {
                    if (Game.t < trackedHealth.Count)
                    {
                        if (trackedHealth[(int)Game.t] < health)
                        {
                            health = trackedHealth[(int)Game.t];
                        }
                    }
                    else
                    {
                        trackedHealth.Add(health);
                    }

                    if (!Game.skippingStage)
                    {
                        bossHealthBar.SetHealthBar(HealthBarState.Standard);

                        SetVHSEffect(false);
                    }
                    else
                    {
                        bossHealthBar.SetHealthBar(HealthBarState.Invincible);

                        SetVHSEffect(true);
                    }
                }
                else//Death of boss
                {
                    Detach(gameObject);
                }

                break;

            case TimeState.Backward:

                if (Game.t < trackedHealth.Count && Game.t >= 0)
                {
                    Golem.health = trackedHealth[(int)Game.t];
                }
                break;
        }
    }

    public virtual void Reset()
    {
        SetAnimators(true);

        for (int i = 0; i < numberOAttacks.Count; i++)
        {
            numberOAttacks[i] = 0;
        }

        // sets the bool to true so they run first time
        playList = true;
        playList2 = true;
        playList3 = true;
        playList4 = true;
        playList5 = true;

        Game.PastTimeScale = 1;

        health = 100;

        //foreach (ObjectTracking obj in trackedBossObjs)
        //{
        //    obj.Reset();
        //} 

    }

    //What happens the first time a stage is entered
    public abstract void OnStageOne();
    public abstract void OnStageTwo();
    public abstract void OnStageThree();
    public abstract void OnStageFour();
    public abstract void OnStageFive();

    protected abstract void StageOneAttacks(int attack);
    protected abstract void StageTwoAttacks(int attack);
    protected abstract void StageThreeAttacks(int attack);
    protected abstract void StageFourAttacks(int attack);
    protected abstract void StageFiveAttacks(int attack);


    public virtual void SetBossParts() { }

	void Detach(GameObject target)
	{
		foreach (SpriteRenderer sprite in bossParts)
		{
			if (!sprite.GetComponent<Rigidbody2D>())
			{
				sprite.gameObject.AddComponent<Rigidbody2D>();
			}
			
			sprite.GetComponent<Rigidbody2D>().isKinematic = false;
			
			if (sprite.GetComponent<Animator>())
			{
				sprite.GetComponent<Animator>().enabled = false;
			}
			sprite.GetComponent<PolygonCollider2D>().enabled = true;
			
			if(sprite.GetComponent<BossAttack>())
			{
				sprite.GetComponent<BossAttack>().enabled = false;
			}
			sprite.transform.DetachChildren();
		}
		
		transform.DetachChildren();  
	}// Controls the death of the boss

    public void SetAnimators(bool enabled)
    {
        foreach (Animator obj in bossAnims)
        {
            obj.GetComponent<Animator>().enabled = enabled;
        }
    }

    void SetVHSEffect(bool enable)
    {
        foreach (SpriteRenderer part in bossParts)
        {
            int val = (enable == true) ? 1 : 0;
            part.material = AssetManager.SpriteMaterials[val];
            part.GetComponent<VHSEffect>().enabled = enable;
        }
    }

    void SetColliders(bool active)
    {
        foreach (Animator bossAttack in bossAnims)
        {
            if (bossAttack.GetComponent<BossAttack>())
            {
                bossAttack.GetComponent<Collider2D>().isTrigger = active;
            }
        }
    }
}
