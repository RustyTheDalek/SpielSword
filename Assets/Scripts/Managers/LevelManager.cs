﻿using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages (No shit) the Level itself, handles logic relating to playing the level 
/// handling the skipping of Boss stages when player causes a paradox
/// Created by      : Ian - 24/07/17
/// Last updated    : Ian - 13/04/18
/// </summary>
public class LevelManager : MonoBehaviour {

    public BossManager currentBoss;

    public VillagerManager vilManager;

    public TimeObjectManager timeManager;

    public Camera2DFollow trackCam;

    /// <summary>
    /// Bounds for the Arena at present
    /// </summary>
    public static BoxCollider2D gameBounds;

    /// <summary>
    /// Boss Health UI
    /// </summary>
    RectTransform bossHealth;

    #region Events
    public delegate void PlayerDeathEvent();
    public static event PlayerDeathEvent OnPlayerDeath;

    public delegate void GameOverEvent();
    public static event GameOverEvent OnGameOver;

    #endregion

    private void Awake()
    {
        BossManager.OnBossDeath += IncreaseScore;

        TimeObjectManager.OnNewRoundReady += OnNewRound;

        ArenaEntry.OnPlayerEnterArena += EnableBossUI;

        VillagerManager.OnNextVillager += TrackNewVillager;
    }

    // Use this for initialization
    void Start ()
    {
        try
        {
            gameBounds = GetComponentInChildren<ArenaEntry>().GetComponent<BoxCollider2D>();
        }
        catch
        {
            Debug.LogWarning("No GameBounds cannot detect entry");
        }

        bossHealth = GetComponentInChildren<BossHealthBar>(true).GetComponent<RectTransform>();

        if (trackCam == null)
            trackCam = GetComponentInChildren<Camera2DFollow>();
    }

    void OnNewRound()
    {
        Game.bossReady = false;
        Game.bossState = BossState.Waking;
    }

    void EnableBossUI()
    {
        bossHealth.gameObject.SetActive(true);
    }
	
	// Update is called once per frame
	void Update ()
    {
        switch (Game.timeState)
        {
            case TimeState.Forward:

                if (!vilManager.activeVillager.Alive)
                {
                    //Game over if no lives
                    if (vilManager.RemainingVillagers <= 0)
                    {
                        Debug.Log("Game Over");
                        //TODO:On game over probably have Golem keep going and character just dies
                        if (OnGameOver != null)
                            OnGameOver();

                        SceneManager.LoadScene("Village");
                    }
                    else //Otherwise start reversing time
                    {
                        Game.timeState = TimeState.Backward;
                        currentBoss.GetComponent<BossManager>().SetAnimators(false);

                        OnPlayerDeath();
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
