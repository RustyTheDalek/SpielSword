﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages (No shit) the Game itself, handles logic relating to overall game e.g. 
/// handling the skipping of Boss stages when player causes a paradox
/// Created by      : Ian - 24/07/17
/// Last updated    : Ian - 24/07/17
/// </summary>
public class GameManager : MonoBehaviour {

    public BossManager  bossTemplate,
                        currentBoss;

    public VillagerManager vilManager;

    public TimeObjectManager timeManager;

    public UnityStandardAssets._2D.Camera2DFollow trackCam;

    // Use this for initialization
    void Start () {
		
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

                if (timeManager.newRoundReady)
                {
                    currentBoss.Reset();
                    vilManager.OnNewRound();

                    timeManager.newRoundReady = false;
                }

                if (!vilManager.activeVillager.Alive)
                {
                    Game.timeState = TimeState.Backward;
                    vilManager.OnVillagerDeath();
                    currentBoss.GetComponent<BossManager>().SetAnimators(false);
                    TimeObjectManager.SoftReset();
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
                                    Game.PastTimeScale = 1;
                                }
                                break;

                            case SkipStageType.VillagerWipe:


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
}
