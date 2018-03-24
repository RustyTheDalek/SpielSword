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

    public UnityStandardAssets._2D.Camera2DFollow trackCam;

    public static BoxCollider2D gameBounds;

    public delegate void PlayerDeathEvent();
    public static event PlayerDeathEvent OnPlayerDeath;

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
	}

    void OnNewRound()
    {
        currentBoss.Reset();
        vilManager.OnNewRound();
        Game.bossReady = false;
        Game.bossState = BossState.Waking;

        TimeObjectManager.NewRoundReady -= OnNewRound;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (trackCam.target == null || trackCam.target != vilManager.activeVillager)
        {
            trackCam.target = vilManager.activeVillager.transform;
        }

        switch (Game.timeState)
        {
            case TimeState.Forward:

                if (!vilManager.activeVillager.Alive)
                {
                    Game.timeState = TimeState.Backward;
                    currentBoss.GetComponent<BossManager>().SetAnimators(false);

                    OnPlayerDeath();
                    TimeObjectManager.NewRoundReady += OnNewRound;

                    //TimeObjectManager.SoftReset();
                    //vilManager.OnVillagerDeath();

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
