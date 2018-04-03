using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages (No shit) the Game itself, handles logic relating to overall game e.g. 
/// handling the skipping of Boss stages when player causes a paradox
/// Created by      : Ian - 24/07/17
/// Last updated    : Ian - 15/03/18
/// </summary>
public class GameManager : MonoBehaviour {

    public BossManager  bossTemplate,
                        currentBoss;

    public VillagerManager vilManager;

    public TimeObjectManager timeManager;

    public Camera2DFollow trackCam;

    public static BoxCollider2D gameBounds;

    public delegate void PlayerDeathEvent();
    public static event PlayerDeathEvent OnPlayerDeath;

    public delegate void GameOverEvent();
    public static event GameOverEvent OnGameOver;

    // Use this for initialization
    void Start ()
    {
        try
        {
            gameBounds = GameObject.Find("ArenaBounds").GetComponent<BoxCollider2D>();
        }
        catch
        {
            Debug.LogWarning("No Game Bounds found, functions that rely on this will not work");
        }

        if(trackCam == null)
            trackCam = GetComponentInChildren<Camera2DFollow>();

        BossManager.OnBossDeath += IncreaseScore;
        BossManager.OnBossDeath += OpenEndSlate;

        VillagerManager.OnNextVillager += TrackNewVillager;
    }

    void OnNewRound()
    {
        Game.bossReady = false;
        Game.bossState = BossState.Waking;
    }
	
	// Update is called once per frame
	void Update ()
    {
        switch (Game.timeState)
        {
            case TimeState.Forward:

                if (!vilManager.activeVillager.Alive)
                {
                    if (vilManager.RemainingVillagers <= 0)
                    {
                        Time.timeScale = 0;
                        Debug.Log("Game Over");
                        //TODO:On game over probably have Golem keep going and character just dies
                        if (OnGameOver != null)
                            OnGameOver();
                    }
                    else
                    {
                        Game.timeState = TimeState.Backward;
                        currentBoss.GetComponent<BossManager>().SetAnimators(false);

                        OnPlayerDeath();

                        //TimeObjectManager.SoftReset();
                        //vilManager.OnVillagerDeath();
                    }

                }

                switch (Game.bossState)
                {
                    case BossState.Waking:

                        if (Game.bossReady)
                        {
                            currentBoss.NextStage();
                            Game.bossState = BossState.Attacking;
                        }

                        break;

                    case BossState.Attacking:

                        if (Game.StageMetEarly)
                        {
                            Game.bossState = BossState.StartSkippingStage;
                        }
                        break;

                    case BossState.StartSkippingStage:

                        switch (Game.skipStageType)
                        {
                            case SkipStageType.FastForward:

                                currentBoss.SetTriggers(Game.StageMetEarly);
                                currentBoss.StartFastForward();
                                Game.bossState = BossState.SkippingStage;
                                break;

                            case SkipStageType.VillagerWipe:

                                currentBoss.TrimStage();
                                currentBoss.NextStage();

                                vilManager.TrimVillagers();
                                vilManager.TrimSpawnables();

                                Game.bossState = BossState.SkippingStage;

                                break;
                        }

                        break;

                    case BossState.SkippingStage:

                        switch (Game.skipStageType)
                        {
                            case SkipStageType.FastForward:

                                if (Game.StageMetEarly)
                                {
                                    currentBoss.FastforwardSkip();
                                }
                                else
                                {
                                    currentBoss.StopFastForward();
                                    Game.PastTimeScale = 1;
                                }
                                break;

                            case SkipStageType.VillagerWipe:

                                Game.bossState = BossState.Attacking;
                                break;
                        }
                        break;

                }
                break;

            //case TimeState.Backward:

            //    if (timeManager.newRoundReady)
            //    {
            //        currentBoss.Reset();
            //        vilManager.OnNewRound();

            //        timeManager.newRoundReady = false;
            //    }
            //    break;
        }
	}

    void IncreaseScore()
    {
        Game.IncScore();
    }

    void TrackNewVillager(Villager newVillager)
    {
        trackCam.target = newVillager.transform;
    }

    /// <summary>
    /// Opens End-game slate showing score
    /// </summary>
    void OpenEndSlate()
    {
        string finish = "You won, score " + Game.TotalScore + " Out of " + Game.MAXSCORE;

        if(Game.ReachedStage3)
        {
            finish += " You reached Stage 3";
        }

        if(Game.ReachedStage5)
        {
            finish += " You reached Stage 5";
        }

        if(Game.ComboAchieved)
        {
            finish += " You did a combo";
        }

        if(Game.LessThanTenLives)
        {
            finish += " In less than 10 lives too";
        }

        Debug.Log(finish);
    }

    public static bool MoveRequest(CircleCollider2D[] colliders, Vector3 position)
    {
        foreach (CircleCollider2D coll in colliders)
        {
            if (!gameBounds.bounds.Contains(position))
            {
                return false;
            }

            if (!gameBounds.bounds.Intersects(coll.bounds))
            {
                Debug.Log("Postion will not be in map no teleport allowed");
                return false;
            }
        }
        return true;
    }
}
