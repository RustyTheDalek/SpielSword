﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Manages (No shit) the Level itself, handles logic relating to playing the level 
/// handling the skipping of Boss stages when player causes a paradox
/// Created by      : Ian - 24/07/17
/// Last updated    : Ian - 13/04/18
/// </summary>
public class LevelManager : MonoBehaviour {

    public SkipStageType skipStageType = SkipStageType.VillagerWipe;

    #region LevelObjects

    public BossManager currentBoss;

    public VillagerManager vilManager;

    public TimeObjectManager timeManager;

    public Camera2DFollow trackCam;

    ArenaEntry arenaEntryPoint;
    ArenaGate arenaGate;

    Text livesTxt;

    #endregion

    EndGamePnl endGamePnl;

    /// <summary>
    /// Bounds for the Arena
    /// </summary>
    static BoxCollider2D arenaBounds;

    /// <summary>
    /// Boss Health UI
    /// </summary>
    RectTransform bossHealth;

    #region Score Variables

    public const int MAXSCORE = 5;

    public int score = 0;

    public static bool ReachedStage3,
                        ReachedStage5,
                        LessThanTenLives;

    #endregion

    #region Events
    public delegate void PlayerDeathEvent();
    //Not ideal to be static but makes sense since player dying causes time to rewind
    public static event PlayerDeathEvent OnPlayerDeath;

    public delegate void GameOverEvent();
    public event GameOverEvent OnGameOver;

    #endregion

    /// <summary>
    /// Invcinibilty mode for testing
    /// </summary>
    public static bool GodMode = false;

    public static bool paused;

    private void Awake()
    {
        VillagerManager.OnNextVillager += TrackNewVillager;
    }

    // Use this for initialization
    void Start ()
    {
        try
        {
            arenaBounds = GetComponentInChildren<ArenaEntry>().GetComponent<BoxCollider2D>();
        }
        catch
        {
            Debug.LogWarning("No GameBounds cannot detect entry");
        }

        livesTxt = GameObject.Find("livesUsedTxt").GetComponent<Text>();

        bossHealth = GetComponentInChildren<BossHealthBar>(true).GetComponent<RectTransform>();

        currentBoss = GetComponentInChildren<BossManager>();

        if (trackCam == null)
            trackCam = GetComponentInChildren<Camera2DFollow>();

        endGamePnl = GetComponentInChildren<EndGamePnl>();

        arenaGate = GetComponentInChildren<ArenaGate>();
        arenaEntryPoint = GetComponentInChildren<ArenaEntry>();
        arenaEntryPoint.OnPlayerEnterArena += EnableBossUI;
        
        vilManager.Setup(currentBoss, arenaEntryPoint);
        arenaGate.Setup(arenaEntryPoint);
        currentBoss.Setup(arenaEntryPoint);
        timeManager.Setup(arenaEntryPoint);

        currentBoss.OnBossDeath += CalculateScore;

    }

    void EnableBossUI()
    {
        bossHealth.gameObject.SetActive(true);
    }
	
	// Update is called once per frame
	void Update ()
    {
        switch (TimeObjectManager.timeState)
        {
            case TimeState.Forward:

                livesTxt.text = "x" + vilManager.totalLives;

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
                        TimeObjectManager.timeState = TimeState.Backward;
                        currentBoss.SetAnimator(false);

                        OnPlayerDeath();
                    }

                }

                break;
        }
	}

    public void CalculateScore()
    {
        //Starts at 1 as Boss has obviously died
        score = 1;

        //Check to see if combo was used
        if(ShamanTotem.comboUsed || Aura.comboUsed)
        {
            score++;
        }

        //Did you finish the fight in a short number of lives
        if(vilManager.totalLives < 10 )
        {
            score++;
        }

        if(currentBoss.highestStageReached >= 3)
        {
            score++;

            if(currentBoss.highestStageReached >= 4)
            {
                score++;
            }
        }

        endGamePnl.OpenSlate(currentBoss.highestStageReached, VillagerManager.combosUsed, vilManager.totalLives);
    }

    void TrackNewVillager(Villager newVillager)
    {
        trackCam.target = newVillager.transform;
    }

    public static bool MoveRequest(CircleCollider2D[] colliders, Vector3 position)
    {
        foreach (CircleCollider2D coll in colliders)
        {
            if (!arenaBounds.bounds.Contains(position))
            {
                return false;
            }

            if (!arenaBounds.bounds.Intersects(coll.bounds))
            {
                Debug.Log("Postion will not be in map no teleport allowed");
                return false;
            }
        }
        return true;
    }

    private void OnDestroy()
    {
        arenaEntryPoint.OnPlayerEnterArena += EnableBossUI;

        vilManager.Unsubscribe(arenaEntryPoint);
        currentBoss.Unsubscribe(arenaEntryPoint);
        arenaGate.Unsubscribe(arenaEntryPoint);
        timeManager.Unsubscribe(arenaEntryPoint);
    }
}
