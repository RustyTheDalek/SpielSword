using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages (No shit) the Level itself, handles logic relating to playing the level 
/// handling the skipping of Boss stages when player causes a paradox
/// Created by      : Ian - 24/07/17
/// Last updated    : Ian - 13/04/18
/// </summary>
public class LevelManager : MonoBehaviour {

    public SkipStageType skipStageType = SkipStageType.VillagerWipe;

    public BossManager currentBoss;

    public VillagerManager vilManager;

    public TimeObjectManager timeManager;

    public Camera2DFollow trackCam;

    /// <summary>
    /// Bounds for the Arena at present
    /// </summary>
    public static BoxCollider2D gameBounds;

    /// <summary>
    /// What time the fight started
    /// </summary>
    public static int fightStart = 0;

    /// <summary>
    /// Boss Health UI
    /// </summary>
    RectTransform bossHealth;

    #region Score Variables

    public const int MAXSCORE = 5;

    private static int _Score = 0;

    public static int stageReached = 1, combosUsed, livesUsed;

    public static bool ReachedStage3,
                        ReachedStage5,
                        LessThanTenLives;

    public static int TotalScore
    {
        get
        {
            return _Score;
        }
    }

    #endregion

    #region Events
    public delegate void PlayerDeathEvent();
    public static event PlayerDeathEvent OnPlayerDeath;

    public delegate void GameOverEvent();
    public static event GameOverEvent OnGameOver;

    #endregion

    /// <summary>
    /// Invcinibilty mode for testing
    /// </summary>
    public static bool GodMode = false;

    public static bool paused;

    private void Awake()
    {
        BossManager.OnBossDeath += IncreaseScore;

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
        currentBoss = GetComponentInChildren<BossManager>();

        if (trackCam == null)
            trackCam = GetComponentInChildren<Camera2DFollow>();
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
                        currentBoss.GetComponent<BossManager>().SetAnimators(false);

                        OnPlayerDeath();
                    }

                }

                break;
        }
	}

    public static void IncreaseScore()
    {
        _Score++;

        Debug.Log("Score increased");
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
