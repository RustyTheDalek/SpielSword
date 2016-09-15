using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public abstract class BossManager : MonoBehaviour {


    public int stage = 0;

	public Head head;

    /// <summary>
    /// Whether the Boss is currently attackble by the Player 
    /// </summary>
    public bool attackable = true;

    public static float health = 100;

	public List<int> attackList, attackList2, attackList3, 
	attackList4, attackList5 = new List<int>();

	public int attackCountStage;
	// Values stored in these ints are used to track the progress in the attack list
	// number system determins the stage they are in charge of
	public int currentCount, currentCount2, currentCount3, currentCount4, currentCount5;
	// Makes sure the list isn't run more then once per stage
	public bool playList, playList2, playList3, playList4, playList5;
		
	public bool alive
	{
		get
		{
			return health > 0;
		}
	}

	public List<SpriteRenderer> bossParts;

    public BossHealthBar bossHealthBar;

    public List<int>    stageOneAttacks = new List<int>(),
                        stageTwoAttacks = new List<int>(),
                        stageThreeAttacks = new List<int>(),
                        stageFourAttacks = new List<int>(),
                        stageFiveAttacks = new List<int>();

    #region Variables for retracing the Boss' Actions
    //List of Boss parts that are tracked for rewind
    List<ObjectTracking> trackedBossObjs = new List<ObjectTracking>();

    public List<float> trackedHealth = new List<float>();
    #endregion

    public virtual void Start ()
	{
		//Sets the counter for the list to zero
		currentCount = 0;
		currentCount2 = 0;
		currentCount3 = 0;
		currentCount4 = 0;
		currentCount5 = 0;
		// sets the bool to true so they run first time
		playList = true;
		playList2 = true;
		playList3 = true;
		playList4 = true;
		playList5 = true;

        //Get All objects that can be tracked
        //Currently assumes anything with an Animator needs to be tracked
        Component[] objs = GetComponentsInChildren<Animator>();

        foreach (Component part in objs)
        {
            trackedBossObjs.Add(part.gameObject.AddComponent<ObjectTracking>());
        }
    }

    // Update is called once per frame
    public virtual void Update()
    {
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

                #region Stage select
                if (health > 80)
                {
                    StageOne();
                }
                else if (health < 80 && health > 60)
                {
                    if (attackList.Count != currentCount)
                    {
                        StageOne();
                        attackable = false;
                    }
                    else
                    {
                        attackable = true;
                        StageTwo();
                    }
                }
                else if (health < 60 && health > 40)
                {
                    if (attackList2.Count != currentCount2)
                    {
                        StageTwo();
                        attackable = false;
                    }
                    else
                    {
                        attackable = true;
                        StageThree();
                    }
                }
                else if (health < 40 && health > 20)
                {
                    if (attackList3.Count != currentCount3)
                    {
                        StageThree();
                        attackable = false;
                    }
                    else
                    {
                        attackable = true;
                        StageFour();
                    }
                }
                else if (health > 0 && health < 20)
                {
                    if (attackList4.Count != currentCount4)
                    {
                        StageFour();
                        attackable = false;
                    }
                    else
                    {
                        attackable = true;
                        StageFive();
                    }
                }
                #endregion

                if (alive)
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

                    if (attackable)
                    {
                        bossHealthBar.SetHealthBar(HealthBarState.Standard);
                    }
                    else
                    {
                        bossHealthBar.SetHealthBar(HealthBarState.Invincible);
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

        currentCount = 0;
        currentCount2 = 0;
        currentCount3 = 0;
        currentCount4 = 0;
        currentCount5 = 0;

        // sets the bool to true so they run first time
        playList = true;
        playList2 = true;
        playList3 = true;
        playList4 = true;
        playList5 = true;

        attackable = true;

        health = 100;

        foreach (ObjectTracking obj in trackedBossObjs)
        {
            obj.Reset();
        } 

    }

	public abstract void StageOne ();
	public abstract void StageTwo ();
	public abstract void StageThree ();
	public abstract void StageFour ();
	public abstract void StageFive ();

	public abstract void SetBossParts ();

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
        foreach (ObjectTracking obj in trackedBossObjs)
        {
            obj.GetComponent<Animator>().enabled = enabled;
        }
    }
}
