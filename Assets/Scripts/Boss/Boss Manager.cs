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
public abstract class BossManager : LivingObject
{

    public BossState bossState = BossState.Waking;

    [Header("References")]
    public BossHealthBar bossHealthBar;

    public Sprite originalHead;
    public Sprite damageHead;

    public Animator animator;

    /// <summary>
    /// Parts of Boss that can be damaged
    /// </summary>
    DamageableSprite[] bossLimbs;

    int damageStage = 0;

    Timer damageTimer;

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

    public int highestStageReached = 1;

    #endregion

    /// <summary>
    /// Defines how the Boss Speeds up during a stage skip
    /// </summary>
    public AnimationCurve bossSpeedUp;

    /// <summary>
    /// All parts of Boss including attacks 
    /// </summary>
    List<SpriteRenderer> bossComponents;

    #region Variables for retracing the Boss' Actions

    protected AnimatorStateInfo animatorStateInfo;

    public List<float> trackedHealth = new List<float>();
    #endregion

    /// <summary>
    /// Uses Normalised time from Boss animations to smooth the speed up for stage skipping
    /// </summary>
    float Smoothing
    {
        get
        {
            return Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1);
        }
    }

    public bool attacking;

    public delegate void BossDeath();
    public event BossDeath OnBossDeath;

    public bool immediateStart = true;

    public virtual void Setup(ArenaEntry arenaEntry, VillagerManager villagerManager, TimeObjectManager timeManager)
	{
        animator = GetComponent<Animator>();

        if (immediateStart)
            animator.enabled = true;

        OnStageOne();

        OnBossDeath += OnDeath;
        timeManager.OnRestartLevel += Reset;

        arenaEntry.OnPlayerEnterArena += StartFight;
        villagerManager.OnActiveDeath += DisableAnimator;
    }

    #region Stage skipping ideas

    //Idea 1 : Boss Fast forwards between stages when skipping attacks aren't
    public void StartFastForward()
    {
        bossHealthBar.SetHealthBar(HealthBarState.Invincible);

        //SetVHSEffect(true);
    }

    public void StopFastForward()
    {
        bossHealthBar.SetHealthBar(HealthBarState.Standard);

        //SetVHSEffect(false);
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
            if (attacking)
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
        if (!attacking)
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

    protected override void Awake()
    {
        base.Awake();

        bossLimbs = GetComponentsInChildren<DamageableSprite>();
    }

    // Use this for initialization
    protected void Start()
    {
        health = MaxHealth;
        damageTimer = gameObject.AddComponent<Timer>();
        damageTimer.Setup("Damager", .25f, true);

        switch (stageFinishType)
        {
            case StageFinishType.Healthloss:

                stageHealthLimits = new List<float>(4)
                {
                    MaxHealth * .8f,
                    MaxHealth * .6f,
                    MaxHealth * .4f,
                    MaxHealth * .2f
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

        //Sets the counter for the list to zero
        for (int i = 0; i < 5; i++)
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
    }

    protected virtual void OnEnable()
    {
        DamageBoss(0);
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

                if (damageTimer.complete)
                {
                    bossLimbs[0].m_Sprite.sprite = originalHead;
                }

                if(bossHealthBar)
                    bossHealthBar.UpdateFill(health, MaxHealth);

                animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);

                if (health <= MaxHealth * .8f)
                {
                    DamageBoss(1);
                }

                if (health <= MaxHealth * .4f)
                {
                    DamageBoss(2);
                }

                switch (bossState)
                {

                    case BossState.Waking:

                        if(animatorStateInfo.IsName("Idle"))
                        {
                            Debug.Log("Ready");
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
                                    TimeEnteredCurrentStage = (int)TimeObjectManager.t;
                                    bossStage = BossStage.Two;
                                    OnStageTwo();
                                }
                                break;

                            case BossStage.Two:

                                StageAttackLogic(StageTwoAttacks);

                                if (ReqsForNextStage())
                                {
                                    TimeEnteredCurrentStage = (int)TimeObjectManager.t;
                                    bossStage = BossStage.Three;
                                    OnStageThree();
                                }

                                break;

                            case BossStage.Three:

                                StageAttackLogic(StageThreeAttacks);

                                if (ReqsForNextStage())
                                {
                                    TimeEnteredCurrentStage = (int)TimeObjectManager.t;
                                    bossStage = BossStage.Four;
                                    OnStageFour();
                                }

                                break;

                            case BossStage.Four:

                                StageAttackLogic(StageFourAttacks);

                                if (ReqsForNextStage(false))
                                {
                                    TimeEnteredCurrentStage = (int)TimeObjectManager.t;
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
        if (TimeObjectManager.startT != 0)
        {
            bossState = BossState.Waking;

            animator.enabled = true;

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

            health = MaxHealth;
        }
    }

    public void DisableAnimator()
    {
        animator.enabled = false;
    }

    public void StartFight()
    {
        Debug.Log("Starting fight!");

        animator.enabled = true;
        GetComponent<TimeObject>().enabled = true;

        //NextStage();
        //Game.bossState = BossState.Attacking;
    }

    //What happens the first time a stage is entered
    public abstract void OnStageOne();

    public virtual void OnStageTwo()
    {
        highestStageReached++;
    }

    public virtual void OnStageThree()
    {
        highestStageReached++;
    }

    public virtual void OnStageFour()
    {
        highestStageReached++;
    }

    public virtual void OnStageFive()
    {
        highestStageReached++;
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

    protected virtual void DamageBoss(int num)
    {
        if (damageStage != num)
        {
            Debug.Log("Setting damage to Stage " + num);
            foreach (DamageableSprite bossLimb in bossLimbs)
            {
                bossLimb.SetDamageSprite(num);
            }
            damageStage = num;
        }
    }

    public virtual void SetBossParts() { }

	void Detach(GameObject target)
	{
		foreach (DamageableSprite sprite in bossLimbs)
		{
			if (!sprite.GetComponent<Rigidbody2D>())
			{
				sprite.gameObject.AddComponent<Rigidbody2D>();
			}
			
			sprite.GetComponent<Rigidbody2D>().isKinematic = false;
			
			sprite.GetComponent<PolygonCollider2D>().enabled = true;
			
			if(sprite.GetComponent<BossAttack>())
			{
				sprite.GetComponent<BossAttack>().enabled = false;
			}
			sprite.transform.DetachChildren();
		}
		
		transform.DetachChildren();  
	}

    /// <summary>
    /// Turns Boss animations parts into Triggers based on Boolean
    /// </summary>
    /// <param name="trigger"> Whether we want Colldiers to become triggers</param>
    public void SetTriggers(bool trigger)
    {
        foreach (DamageableSprite bossAttack in bossLimbs)
        {
            if (bossAttack.GetComponent<BossAttack>())
            {
                bossAttack.GetComponent<Collider2D>().isTrigger = trigger;
            }
        }
    }

    public virtual void Unsubscribe(ArenaEntry arenaEntry, VillagerManager villagerManager, TimeObjectManager timeManager)
    {
        OnBossDeath -= OnDeath;
        arenaEntry.OnPlayerEnterArena -= StartFight;
        villagerManager.OnActiveDeath -= DisableAnimator;
        timeManager.OnRestartLevel -= Reset;
    }

    public virtual void PlayEffect() { }

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

    public override void OnHit(Vector2 attackDirection, float damageMultiplier)
    {
        base.OnHit(attackDirection, 1 * damageMultiplier);
        //Bit hardcoded but first item in list should be head
        bossLimbs[0].m_Sprite.sprite = damageHead;
        damageTimer.StartTimer();
    }
}
