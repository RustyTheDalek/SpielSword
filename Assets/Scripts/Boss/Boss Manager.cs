using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;

public abstract class BossManager : MonoBehaviour
{
    public Head head;

    public const float MAXHEALTH = 400;
    public static float health = MAXHEALTH;

    public List<List<int>> stageAttacks = new List<List<int>>();

    public List<int> CurrentStageAtttacks
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

    /// <summary>
    /// Helps track when a stage is entered for accurate stage skipping checks
    /// </summary>
    public List<int> timeEnteredStage = new List<int>(5);

    public int TimeEnteredCurrentStage
    {
        get
        {
            return timeEnteredStage[(int)bossStage];
        }

        set
        {
            timeEnteredStage[(int)bossStage] = value;
        }
    }

    public List<bool> stageReplaying;

    public bool CurrentStageReplay
    {
        get
        {
            return stageReplaying[(int)bossStage];
        }

        set
        {
            stageReplaying[(int)bossStage] = value;
        }
}

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

    public BossStage bossStage = BossStage.None;

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
        for(int i = 0; i < 5; i++)
        {
            //Debug.Log(i);
            numberOAttacks.Add(0);
            stageAttacks.Add(new List<int>());
            stageReplaying.Add(true);
            timeEnteredStage.Add(0);
        }

        for (int i = 0; i < stageAttacks.Count; i++)
        {
            stageAttacks[i] = new List<int>();
        }

        //Get All objects that can be tracked
        //Currently assumes anything with an Animator needs to be tracked
        Animator[] objs = GetComponentsInChildren<Animator>();

        bossAnims.AddRange(objs);

        OnStageOne();

        switch (bossStage)
        {
            case BossStage.Two:

                OnStageTwo();
                health = MAXHEALTH * .79f;
                break;

            case BossStage.Three:

                OnStageThree();
                health = MAXHEALTH * .59f;
                break;

            case BossStage.Four:

                OnStageFour();
                health = MAXHEALTH * .39f;
                break;

            case BossStage.Five:

                OnStageFive();
                health = MAXHEALTH * .19f;
                break;
        }
    }

    #region Stage skipping ideas

    //Idea 1 : Boss Fast forwards between stages when skipping attacks aren't
    public void StartFastForward()
    {
        bossHealthBar.SetHealthBar(HealthBarState.Invincible);

        SetVHSEffect(true);
    }

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

    public void StopFastForward()
    {
        bossHealthBar.SetHealthBar(HealthBarState.Standard);

        SetVHSEffect(false);
    }

    //Idea 2 : Skip to next Stage
    public void NextStage()
    {
        bossStage += 1;
    }

    public void TrimStage()
    {
        for (int i = NumberOAttacks4Stage; i < CurrentStageAtttacks.Count; i++)
        {
            CurrentStageAtttacks.RemoveAt(i);
        }
    }

    #endregion

    /// <summary>
    /// This helps to see if stage was entered early by seeing if the boss still has 
    /// attacks to perform in this stage
    /// </summary>
    /// <returns></returns>
    bool EarlyStageCheck()
    {
        //If there isn't a time for when the stage was entered then there's no need to check
        if (TimeEnteredCurrentStage != 0 && Game.t < TimeEnteredCurrentStage)
        {
            Game.StageMetEarly = true;
            //We want to overwrite the Time entered current stage 
            TimeEnteredCurrentStage = Game.t;
        }
        else
        {
            Game.StageMetEarly = false;
        }

        return Game.StageMetEarly;
    }

    void StageListings(Action<int> stageAttacks)
    {
        //pulls up the next attack in sequence
        for (int i = NumberOAttacks4Stage; i <= CurrentStageAtttacks.Count; i++)
        {
            if (Attacking)
            {
                // makes sure a attack isn't already playing befor continuing
                return;
            }
            int attack = CurrentStageAtttacks[i];
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
            if (CurrentStageAtttacks.Count > NumberOAttacks4Stage && CurrentStageReplay)
            {
                StageListings(stageAttacks);
            }
            else
            {
                //Prevents access to the list once at this part
                CurrentStageReplay = false;
                // Rolls a random number based on no of attacks
                int newAttack = UnityEngine.Random.Range(0, attackCountStage);
                // Adds attack to list
                CurrentStageAtttacks.Add(newAttack);
                NumberOAttacks4Stage = CurrentStageAtttacks.Count;
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

                        StageAttackLogic(StageOneAttacks);

                        if (health < MAXHEALTH * .8f && !EarlyStageCheck())
                        {
                            TimeEnteredCurrentStage = Game.t;
                            bossStage = BossStage.Two;
                            OnStageTwo();
                        }
                        break;

                    case BossStage.Two:

                        StageAttackLogic(StageTwoAttacks);

                        if (health < MAXHEALTH * .6f && !EarlyStageCheck())
                        {
                            TimeEnteredCurrentStage = Game.t;
                            bossStage = BossStage.Three;
                            OnStageThree();
                        }

                        break;

                    case BossStage.Three:

                        StageAttackLogic(StageThreeAttacks);

                        if (health < MAXHEALTH * .4f && !EarlyStageCheck())
                        {
                            TimeEnteredCurrentStage = Game.t;
                            bossStage = BossStage.Four;
                            OnStageFour();
                        }

                        break;

                    case BossStage.Four:

                        StageAttackLogic(StageFourAttacks);

                        if (health < MAXHEALTH * .2f && !EarlyStageCheck())
                        {
                            TimeEnteredCurrentStage = Game.t;
                            bossStage = BossStage.Five;
                            OnStageFive();
                        }

                        break;

                    case BossStage.Five:

                        StageAttackLogic(StageFiveAttacks);

                        break;
                }

                if (Alive)
                {
                    if (Game.t < trackedHealth.Count)
                    {
                        //if (trackedHealth[(int)Game.t] < health)
                        //{
                        //    health = trackedHealth[(int)Game.t];
                        //}
                    }
                    else
                    {
                        trackedHealth.Add(health);
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

        bossStage = BossStage.One;
        OnStageOne();

        for (int i = 0; i < numberOAttacks.Count; i++)
        {
            numberOAttacks[i] = 0;
        }

        // sets the bool to true so they run first time
        for (int i = 0; i < stageReplaying.Count; i++)
        {
            stageReplaying[i] = true;
        }

        Game.PastTimeScale = 1;

        health = MAXHEALTH;

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
            string val = (enable == true) ? "VHSSprite" : "Sprite";
            part.material = AssetManager.SpriteMaterials[val];
            part.GetComponent<VHSEffect>().enabled = enable;
        }
    }

    /// <summary>
    /// Turns Boss animations parts into Triggers based on Boolean
    /// </summary>
    /// <param name="trigger"> Whether we want Colldiers to become triggers</param>
    public void SetTriggers(bool trigger)
    {
        foreach (Animator bossAttack in bossAnims)
        {
            if (bossAttack.GetComponent<BossAttack>())
            {
                bossAttack.GetComponent<Collider2D>().isTrigger = trigger;
            }
        }
    }

    #region Debug Functions

    public void Stage2()
    {
        health = MAXHEALTH* .7f;
    }

    public void Stage3()
    {
        health = MAXHEALTH * .5f;
    }

    public void Stage4()
    {
        health = MAXHEALTH * .3f;
    }

    public void Stage5()
    {
        health = MAXHEALTH * .1f;
    }

    #endregion
}
