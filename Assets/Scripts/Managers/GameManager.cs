using UnityEngine;

/// <summary>
/// Manages (No shit) the Game itself, handles logic relating to overall game e.g. 
/// handling the skipping of Boss stages when player causes a paradox
/// Created by      : Ian - 24/07/17
/// Last updated    : Ian - 06/04/18
/// </summary>
public class GameManager : MonoBehaviour {

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

    // Use this for initialization
    void Start ()
    {
        gameBounds = GetComponentInChildren<ArenaEntry>().GetComponent<BoxCollider2D>();
        bossHealth = GetComponentInChildren<BossHealthBar>(true).GetComponent<RectTransform>();

        if(trackCam == null)
            trackCam = GetComponentInChildren<Camera2DFollow>();

        BossManager.OnBossDeath += IncreaseScore;
        BossManager.OnBossDeath += OpenEndSlate;

        TimeObjectManager.OnNewRoundReady += OnNewRound;

        ArenaEntry.OnPlayerEnterArena += EnableBossUI;

        VillagerManager.OnNextVillager += TrackNewVillager;
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
                        Time.timeScale = 0;
                        Debug.Log("Game Over");
                        //TODO:On game over probably have Golem keep going and character just dies
                        if (OnGameOver != null)
                            OnGameOver();
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

                                vilManager.TrimVillagers();

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
