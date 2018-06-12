using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;

/// <summary>
/// Base class for Bosses
/// Created by : Sean Taylor    - 15/06/16
/// Updated by : Ian Jones      - 13/04/18
/// </summary>
public abstract class BossManager : MonoBehaviour
{
    public const float MAXHEALTH = 400;
    public float health = MAXHEALTH;

    public BossHealthBar bossHealthBar;

    public BossState bossState = BossState.Waking;

    #region Attack variables

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

    protected List<int> stageOneAttackSequence = new List<int>(),
                            stageTwoAttackSequence = new List<int>(),
                            stageThreeAttackSequence = new List<int>(),
                            stageFourAttackSequence = new List<int>(),
                            stageFiveAttackSequence = new List<int>();

    #endregion

    #region Stage variables

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

    public BossStage bossStage = BossStage.None;

    [SerializeField]
    StageFinishType stageFinishType = StageFinishType.Time;

    Timer stageFinishTimer;

    List<int> stageTimers;
    List<float> stageHealthLimits;

    #endregion

    /// <summary>
    /// Defines how the Boss Speeds up during a stage skip
    /// </summary>
    public AnimationCurve bossSpeedUp;

    public bool Alive
	{
		get
		{
			return health > 0;
		}
	}

	public List<SpriteRenderer> bossParts;

    #region Variables for retracing the Boss' Actions
    //List of Boss parts that are tracked for rewind
    List<Animator> bossAnims = new List<Animator>();

    public List<float> trackedHealth = new List<float>();
    #endregion

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

    public delegate void BossDeath();
    public static event BossDeath OnBossDeath;

    public bool immediateStart = true;

    public static bool ready = false;

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

        if (immediateStart)
        {
            foreach (Animator bossPart in bossAnims)
            {
                bossPart.enabled = true;
            }

            bossHealthBar.gameObject.SetActive(true);
        }

        OnStageOne();

        switch(stageFinishType)
        {
            case StageFinishType.Healthloss:

                stageHealthLimits = new List<float>(4)
                {
                    MAXHEALTH * .8f,
                    MAXHEALTH * .6f,
                    MAXHEALTH * .4f,
                    MAXHEALTH * .2f
                };

                break;

            case StageFinishType.Time:

                stageTimers = new List<int>(4)
                {
                    60,
                    60,
                    60,
                    60
                };

                break;
        }

        OnBossDeath += OnDeath;
        TimeObjectManager.OnNewRoundReady += Reset;

        ArenaEntry.OnPlayerEnterArena += StartFight;
    }

    #region Stage skipping ideas

    //Idea 1 : Boss Fast forwards between stages when skipping attacks aren't
    public void StartFastForward()
    {
        bossHealthBar.SetHealthBar(HealthBarState.Invincible);

        SetVHSEffect(true);
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

        if(stageFinishType == StageFinishType.Time)
        {
            Debug.Log(bossStage + " : " + (int)bossStage);
            stageFinishTimer = gameObject.AddComponent<Timer>();
            stageFinishTimer.Setup("StageCountdown", stageTimers[(int)bossStage], true);
            stageFinishTimer.StartTimer();
        }
    }

    public void TrimStage()
    {
        for (int i = NumberOAttacks4Stage; i < CurrentStageAtttacks.Count; i++)
        {
            CurrentStageAtttacks.RemoveAt(i);
        }
    }

    #endregion

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

    bool ReqsForNextStage(bool checkforNextStage = true)
    {
        switch(stageFinishType)
        {
            case StageFinishType.Healthloss:

                if (health < stageHealthLimits[(int)bossStage])
                    return true;

                break;

            case StageFinishType.Time:

                if (stageFinishTimer.complete)
                {
                    if (checkforNextStage)
                    {
                        stageFinishTimer.Setup("StageCountdown", stageTimers[(int)bossStage], true);
                        stageFinishTimer.Reset();
                        stageFinishTimer.StartTimer();
                    }

                    return true;
                }

                break;
        }

        return false;
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

        switch (TimeObjectManager.timeState)
        {
            case TimeState.Forward:

                bossHealthBar.UpdateFill(health, MAXHEALTH);

                if (health <= MAXHEALTH * .8f)
                {
                    DamageBoss(0);
                }

                if (health <= MAXHEALTH * .4f)
                {
                    DamageBoss(1);
                }

                switch (bossState)
                {

                    case BossState.Waking:

                        if(ready)
                        {
                            NextStage();
                            bossState = BossState.Attacking;
                        }

                        break;

                    case BossState.Attacking:

                        switch (bossStage)
                        {
                            case BossStage.One:

                                StageAttackLogic(StageOneAttacks);

                                if (ReqsForNextStage())
                                {
                                    TimeEnteredCurrentStage = TimeObjectManager.t;
                                    bossStage = BossStage.Two;
                                    OnStageTwo();
                                }
                                break;

                            case BossStage.Two:

                                StageAttackLogic(StageTwoAttacks);

                                if (ReqsForNextStage())
                                {
                                    TimeEnteredCurrentStage = TimeObjectManager.t;
                                    bossStage = BossStage.Three;
                                    OnStageThree();
                                }

                                break;

                            case BossStage.Three:

                                StageAttackLogic(StageThreeAttacks);

                                if (ReqsForNextStage())
                                {
                                    TimeEnteredCurrentStage = TimeObjectManager.t;
                                    bossStage = BossStage.Four;
                                    OnStageFour();
                                }

                                break;

                            case BossStage.Four:

                                StageAttackLogic(StageFourAttacks);

                                if (ReqsForNextStage(false))
                                {
                                    TimeEnteredCurrentStage = TimeObjectManager.t;
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
                            if (TimeObjectManager.t < trackedHealth.Count)
                            {
                                //if (trackedHealth[(int)TimeObjectManager.t] < health)
                                //{
                                //    health = trackedHealth[(int)TimeObjectManager.t];
                                //}
                            }
                            else
                            {
                                trackedHealth.Add(health);
                            }
                        }
                        else//Death of boss
                        {
                            OnBossDeath();
                        }

                        break;
                }   

                break;

            case TimeState.Backward:

                if (TimeObjectManager.t < trackedHealth.Count && TimeObjectManager.t >= 0)
                {
                    health = trackedHealth[(int)TimeObjectManager.t];
                }
                break;
        }
    }
    
    /// <summary>
    /// Resets Boss for start of next fight
    /// </summary>
    public virtual void Reset()
    {
        ready = false;
        bossState = BossState.Waking;   

        SetAnimators(true);

        bossStage = BossStage.None;

        for (int i = 0; i < numberOAttacks.Count; i++)
        {
            numberOAttacks[i] = 0;
        }

        // sets the bool to true so they run first time
        for (int i = 0; i < stageReplaying.Count; i++)
        {
            stageReplaying[i] = true;
        }

        health = MAXHEALTH;

        //foreach (ObjectTracking obj in trackedBossObjs)
        //{
        //    obj.Reset();
        //} 
    }

    public void StartFight()
    {
        Debug.Log("Starting fight!");

        //TODO:Maybe move this to a different event function as it's not strictly 
        //something that the Boss Manager could track
        LevelManager.fightStart = TimeObjectManager.t;

        foreach(Animator bossPart in bossAnims)
        {
            bossPart.enabled = true;
        }
        //NextStage();
        //Game.bossState = BossState.Attacking;

        ArenaEntry.OnPlayerEnterArena -= StartFight;
    }

    //What happens the first time a stage is entered
    public abstract void OnStageOne();

    public virtual void OnStageTwo()
    {
        LevelManager.stageReached = 2;
    }

    public virtual void OnStageThree()
    {
        LevelManager.IncreaseScore();
        LevelManager.stageReached = 3;
    }
    public virtual void OnStageFour()
    {
        LevelManager.stageReached = 4;
    }

    public virtual void OnStageFive()
    {
        LevelManager.IncreaseScore();
        LevelManager.stageReached = 5;
    }

    public void OnDeath()
    {
        bossState = BossState.Dead;

        Detach(gameObject);
    }

    protected abstract void StageOneAttacks(int attack);
    protected abstract void StageTwoAttacks(int attack);
    protected abstract void StageThreeAttacks(int attack);
    protected abstract void StageFourAttacks(int attack);
    protected abstract void StageFiveAttacks(int attack);

    protected abstract void DamageBoss(int num);

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

    public void ToStage(int stage)
    { 
        switch (stageFinishType)
        {
            case StageFinishType.Healthloss:

                health = stageHealthLimits[stage-1];
                break;

            case StageFinishType.Time:

                bossStage = (BossStage)stage-1;
                stageFinishTimer.complete = true;

                break;
        }
    }

    #endregion
}
